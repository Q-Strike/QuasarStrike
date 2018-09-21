﻿using xServer.Controls;

namespace xServer.Forms
{
    partial class FrmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationGatheringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemInformationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.startupManagerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.taskManagerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tCPConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.situationalAwarenessChecksToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.interactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileManagerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteShellToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.powerPickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registryEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runLocalWMICommandToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.impersonationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stealProcessTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beginImpersonationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertToSelfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.privilegeEscalationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.privilegeEscalationChecksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.askToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.credentialHarvestingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mimikatzToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionGopherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.persistenceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.wMIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registryRunKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduledTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverseProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtLine = new System.Windows.Forms.ToolStripSeparator();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standbyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationGatheringToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.activeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getCurrentDomainControllerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getFileServersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enumerateGPOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getDomainUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enumerateSessionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lateralMovementToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.passTheHashToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.wMIExecToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sMBExecToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.wMIMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dCOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSWORDDDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelDDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMC20ApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sHELLWINDOWSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shellBrowserWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visioAddonExecutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outlookExecutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arbitraryLibraryLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordWLLAddInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outlookScriptExecutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visioExecuteLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officeMacroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.persistenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surveillanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteWebcamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordRecoveryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyloggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miscellaneousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteExecuteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localFileInMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nETAssemblyInMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMessageboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openChatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgFlags = new System.Windows.Forms.ImageList(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.listenToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lstClients = new xServer.Controls.AeroListView();
            this.hIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hUserPC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hUserStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hCountry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hOS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hAccountType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.builderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hostFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionToolStripMenuItem,
            this.systemToolStripMenuItem,
            this.networkToolStripMenuItem,
            this.surveillanceToolStripMenuItem,
            this.miscellaneousToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.selectAllToolStripMenuItem});
            this.contextMenuStrip.Name = "ctxtMenu";
            this.contextMenuStrip.Size = new System.Drawing.Size(162, 164);
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.reconnectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.uninstallToolStripMenuItem,
            this.viewLogToolStripMenuItem});
            this.connectionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("connectionToolStripMenuItem.Image")));
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.connectionToolStripMenuItem.Text = "Connection";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("updateToolStripMenuItem.Image")));
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // reconnectToolStripMenuItem
            // 
            this.reconnectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reconnectToolStripMenuItem.Image")));
            this.reconnectToolStripMenuItem.Name = "reconnectToolStripMenuItem";
            this.reconnectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.reconnectToolStripMenuItem.Text = "Reconnect";
            this.reconnectToolStripMenuItem.Click += new System.EventHandler(this.reconnectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("disconnectToolStripMenuItem.Image")));
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uninstallToolStripMenuItem.Image")));
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.uninstallToolStripMenuItem.Text = "Uninstall";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Image = global::xServer.Properties.Resources.copy;
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informationGatheringToolStripMenuItem,
            this.interactionToolStripMenuItem,
            this.privilegeEscalationToolStripMenuItem,
            this.credentialHarvestingToolStripMenuItem,
            this.persistenceToolStripMenuItem1,
            this.reverseProxyToolStripMenuItem,
            this.ctxtLine,
            this.actionsToolStripMenuItem});
            this.systemToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("systemToolStripMenuItem.Image")));
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.systemToolStripMenuItem.Text = "Host";
            // 
            // informationGatheringToolStripMenuItem
            // 
            this.informationGatheringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemInformationToolStripMenuItem1,
            this.startupManagerToolStripMenuItem1,
            this.taskManagerToolStripMenuItem1,
            this.tCPConnectionsToolStripMenuItem,
            this.situationalAwarenessChecksToolStripMenuItem1});
            this.informationGatheringToolStripMenuItem.Name = "informationGatheringToolStripMenuItem";
            this.informationGatheringToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.informationGatheringToolStripMenuItem.Text = "Information Gathering";
            // 
            // systemInformationToolStripMenuItem1
            // 
            this.systemInformationToolStripMenuItem1.Name = "systemInformationToolStripMenuItem1";
            this.systemInformationToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.systemInformationToolStripMenuItem1.Text = "System Information";
            this.systemInformationToolStripMenuItem1.Click += new System.EventHandler(this.systemInformationToolStripMenuItem1_Click);
            // 
            // startupManagerToolStripMenuItem1
            // 
            this.startupManagerToolStripMenuItem1.Name = "startupManagerToolStripMenuItem1";
            this.startupManagerToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.startupManagerToolStripMenuItem1.Text = "Startup Manager";
            this.startupManagerToolStripMenuItem1.Click += new System.EventHandler(this.startupManagerToolStripMenuItem1_Click);
            // 
            // taskManagerToolStripMenuItem1
            // 
            this.taskManagerToolStripMenuItem1.Name = "taskManagerToolStripMenuItem1";
            this.taskManagerToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.taskManagerToolStripMenuItem1.Text = "Task Manager";
            this.taskManagerToolStripMenuItem1.Click += new System.EventHandler(this.taskManagerToolStripMenuItem1_Click);
            // 
            // tCPConnectionsToolStripMenuItem
            // 
            this.tCPConnectionsToolStripMenuItem.Name = "tCPConnectionsToolStripMenuItem";
            this.tCPConnectionsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.tCPConnectionsToolStripMenuItem.Text = "TCP Connections";
            this.tCPConnectionsToolStripMenuItem.Click += new System.EventHandler(this.tCPConnectionsToolStripMenuItem_Click);
            // 
            // situationalAwarenessChecksToolStripMenuItem1
            // 
            this.situationalAwarenessChecksToolStripMenuItem1.Name = "situationalAwarenessChecksToolStripMenuItem1";
            this.situationalAwarenessChecksToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.situationalAwarenessChecksToolStripMenuItem1.Text = "Situational Awareness Checks";
            // 
            // interactionToolStripMenuItem
            // 
            this.interactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileManagerToolStripMenuItem1,
            this.remoteShellToolStripMenuItem1,
            this.powerPickToolStripMenuItem,
            this.registryEditorToolStripMenuItem1,
            this.runLocalWMICommandToolStripMenuItem1,
            this.impersonationToolStripMenuItem});
            this.interactionToolStripMenuItem.Name = "interactionToolStripMenuItem";
            this.interactionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.interactionToolStripMenuItem.Text = "Interaction";
            // 
            // fileManagerToolStripMenuItem1
            // 
            this.fileManagerToolStripMenuItem1.Name = "fileManagerToolStripMenuItem1";
            this.fileManagerToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            this.fileManagerToolStripMenuItem1.Text = "File Manager";
            this.fileManagerToolStripMenuItem1.Click += new System.EventHandler(this.fileManagerToolStripMenuItem1_Click);
            // 
            // remoteShellToolStripMenuItem1
            // 
            this.remoteShellToolStripMenuItem1.Name = "remoteShellToolStripMenuItem1";
            this.remoteShellToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            this.remoteShellToolStripMenuItem1.Text = "Remote Shell";
            this.remoteShellToolStripMenuItem1.Click += new System.EventHandler(this.remoteShellToolStripMenuItem1_Click);
            // 
            // powerPickToolStripMenuItem
            // 
            this.powerPickToolStripMenuItem.Name = "powerPickToolStripMenuItem";
            this.powerPickToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.powerPickToolStripMenuItem.Text = "PowerPick";
            this.powerPickToolStripMenuItem.Click += new System.EventHandler(this.powerPickToolStripMenuItem_Click);
            // 
            // registryEditorToolStripMenuItem1
            // 
            this.registryEditorToolStripMenuItem1.Name = "registryEditorToolStripMenuItem1";
            this.registryEditorToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            this.registryEditorToolStripMenuItem1.Text = "Registry Editor";
            this.registryEditorToolStripMenuItem1.Click += new System.EventHandler(this.registryEditorToolStripMenuItem1_Click);
            // 
            // runLocalWMICommandToolStripMenuItem1
            // 
            this.runLocalWMICommandToolStripMenuItem1.Name = "runLocalWMICommandToolStripMenuItem1";
            this.runLocalWMICommandToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            this.runLocalWMICommandToolStripMenuItem1.Text = "Run WMI Command";
            this.runLocalWMICommandToolStripMenuItem1.Click += new System.EventHandler(this.runLocalWMICommandToolStripMenuItem1_Click);
            // 
            // impersonationToolStripMenuItem
            // 
            this.impersonationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.makeTokenToolStripMenuItem,
            this.stealProcessTokenToolStripMenuItem,
            this.beginImpersonationToolStripMenuItem,
            this.revertToSelfToolStripMenuItem});
            this.impersonationToolStripMenuItem.Name = "impersonationToolStripMenuItem";
            this.impersonationToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.impersonationToolStripMenuItem.Text = "Impersonation";
            // 
            // makeTokenToolStripMenuItem
            // 
            this.makeTokenToolStripMenuItem.Name = "makeTokenToolStripMenuItem";
            this.makeTokenToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.makeTokenToolStripMenuItem.Text = "Make Token";
            this.makeTokenToolStripMenuItem.Click += new System.EventHandler(this.makeTokenToolStripMenuItem_Click);
            // 
            // stealProcessTokenToolStripMenuItem
            // 
            this.stealProcessTokenToolStripMenuItem.Name = "stealProcessTokenToolStripMenuItem";
            this.stealProcessTokenToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.stealProcessTokenToolStripMenuItem.Text = "Steal Process Token";
            this.stealProcessTokenToolStripMenuItem.Click += new System.EventHandler(this.stealProcessTokenToolStripMenuItem_Click);
            // 
            // beginImpersonationToolStripMenuItem
            // 
            this.beginImpersonationToolStripMenuItem.Name = "beginImpersonationToolStripMenuItem";
            this.beginImpersonationToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.beginImpersonationToolStripMenuItem.Text = "Impersonation Menu";
            this.beginImpersonationToolStripMenuItem.Click += new System.EventHandler(this.beginImpersonationToolStripMenuItem_Click);
            // 
            // revertToSelfToolStripMenuItem
            // 
            this.revertToSelfToolStripMenuItem.Name = "revertToSelfToolStripMenuItem";
            this.revertToSelfToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.revertToSelfToolStripMenuItem.Text = "Revert To Self";
            this.revertToSelfToolStripMenuItem.Click += new System.EventHandler(this.revertToSelfToolStripMenuItem_Click);
            // 
            // privilegeEscalationToolStripMenuItem
            // 
            this.privilegeEscalationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.privilegeEscalationChecksToolStripMenuItem,
            this.askToolStripMenuItem});
            this.privilegeEscalationToolStripMenuItem.Name = "privilegeEscalationToolStripMenuItem";
            this.privilegeEscalationToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.privilegeEscalationToolStripMenuItem.Text = "Privilege Escalation";
            // 
            // privilegeEscalationChecksToolStripMenuItem
            // 
            this.privilegeEscalationChecksToolStripMenuItem.Name = "privilegeEscalationChecksToolStripMenuItem";
            this.privilegeEscalationChecksToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.privilegeEscalationChecksToolStripMenuItem.Text = "Privilege Escalation Checks";
            // 
            // askToolStripMenuItem
            // 
            this.askToolStripMenuItem.Name = "askToolStripMenuItem";
            this.askToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.askToolStripMenuItem.Text = "UAC Ask";
            this.askToolStripMenuItem.Click += new System.EventHandler(this.askToolStripMenuItem_Click);
            // 
            // credentialHarvestingToolStripMenuItem
            // 
            this.credentialHarvestingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mimikatzToolStripMenuItem1,
            this.sessionGopherToolStripMenuItem});
            this.credentialHarvestingToolStripMenuItem.Name = "credentialHarvestingToolStripMenuItem";
            this.credentialHarvestingToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.credentialHarvestingToolStripMenuItem.Text = "Credential Harvesting";
            // 
            // mimikatzToolStripMenuItem1
            // 
            this.mimikatzToolStripMenuItem1.Name = "mimikatzToolStripMenuItem1";
            this.mimikatzToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.mimikatzToolStripMenuItem1.Text = "Mimikatz";
            // 
            // sessionGopherToolStripMenuItem
            // 
            this.sessionGopherToolStripMenuItem.Name = "sessionGopherToolStripMenuItem";
            this.sessionGopherToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.sessionGopherToolStripMenuItem.Text = "Session Gopher";
            // 
            // persistenceToolStripMenuItem1
            // 
            this.persistenceToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wMIToolStripMenuItem,
            this.registryRunKeyToolStripMenuItem,
            this.scheduledTasksToolStripMenuItem,
            this.newServiceToolStripMenuItem});
            this.persistenceToolStripMenuItem1.Name = "persistenceToolStripMenuItem1";
            this.persistenceToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            this.persistenceToolStripMenuItem1.Text = "Persistence";
            // 
            // wMIToolStripMenuItem
            // 
            this.wMIToolStripMenuItem.Name = "wMIToolStripMenuItem";
            this.wMIToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.wMIToolStripMenuItem.Text = "WMI";
            this.wMIToolStripMenuItem.Click += new System.EventHandler(this.wMIToolStripMenuItem_Click);
            // 
            // registryRunKeyToolStripMenuItem
            // 
            this.registryRunKeyToolStripMenuItem.Name = "registryRunKeyToolStripMenuItem";
            this.registryRunKeyToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.registryRunKeyToolStripMenuItem.Text = "Registry Run Key";
            // 
            // scheduledTasksToolStripMenuItem
            // 
            this.scheduledTasksToolStripMenuItem.Name = "scheduledTasksToolStripMenuItem";
            this.scheduledTasksToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.scheduledTasksToolStripMenuItem.Text = "Scheduled Tasks";
            // 
            // newServiceToolStripMenuItem
            // 
            this.newServiceToolStripMenuItem.Name = "newServiceToolStripMenuItem";
            this.newServiceToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newServiceToolStripMenuItem.Text = "New Service";
            // 
            // reverseProxyToolStripMenuItem
            // 
            this.reverseProxyToolStripMenuItem.Image = global::xServer.Properties.Resources.server_link;
            this.reverseProxyToolStripMenuItem.Name = "reverseProxyToolStripMenuItem";
            this.reverseProxyToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.reverseProxyToolStripMenuItem.Text = "Reverse Proxy";
            this.reverseProxyToolStripMenuItem.Click += new System.EventHandler(this.reverseProxyToolStripMenuItem_Click);
            // 
            // ctxtLine
            // 
            this.ctxtLine.Name = "ctxtLine";
            this.ctxtLine.Size = new System.Drawing.Size(189, 6);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shutdownToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.standbyToolStripMenuItem});
            this.actionsToolStripMenuItem.Image = global::xServer.Properties.Resources.actions;
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.actionsToolStripMenuItem.Text = "Actions";
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Image = global::xServer.Properties.Resources.shutdown;
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.shutdownToolStripMenuItem.Text = "Shutdown";
            this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.shutdownToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Image = global::xServer.Properties.Resources.restart;
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // standbyToolStripMenuItem
            // 
            this.standbyToolStripMenuItem.Image = global::xServer.Properties.Resources.standby;
            this.standbyToolStripMenuItem.Name = "standbyToolStripMenuItem";
            this.standbyToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.standbyToolStripMenuItem.Text = "Standby";
            this.standbyToolStripMenuItem.Click += new System.EventHandler(this.standbyToolStripMenuItem_Click);
            // 
            // networkToolStripMenuItem
            // 
            this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informationGatheringToolStripMenuItem1,
            this.lateralMovementToolStripMenuItem2,
            this.persistenceToolStripMenuItem});
            this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
            this.networkToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.networkToolStripMenuItem.Text = "Network";
            // 
            // informationGatheringToolStripMenuItem1
            // 
            this.informationGatheringToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activeDirectoryToolStripMenuItem,
            this.hostToolStripMenuItem});
            this.informationGatheringToolStripMenuItem1.Name = "informationGatheringToolStripMenuItem1";
            this.informationGatheringToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            this.informationGatheringToolStripMenuItem1.Text = "Information Gathering";
            // 
            // activeDirectoryToolStripMenuItem
            // 
            this.activeDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getCurrentDomainControllerToolStripMenuItem,
            this.getFileServersToolStripMenuItem,
            this.enumerateGPOToolStripMenuItem,
            this.getDomainUserToolStripMenuItem});
            this.activeDirectoryToolStripMenuItem.Name = "activeDirectoryToolStripMenuItem";
            this.activeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.activeDirectoryToolStripMenuItem.Text = "Active Directory";
            // 
            // getCurrentDomainControllerToolStripMenuItem
            // 
            this.getCurrentDomainControllerToolStripMenuItem.Name = "getCurrentDomainControllerToolStripMenuItem";
            this.getCurrentDomainControllerToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.getCurrentDomainControllerToolStripMenuItem.Text = "Get Current Domain Controller";
            // 
            // getFileServersToolStripMenuItem
            // 
            this.getFileServersToolStripMenuItem.Name = "getFileServersToolStripMenuItem";
            this.getFileServersToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.getFileServersToolStripMenuItem.Text = "Get File Servers";
            // 
            // enumerateGPOToolStripMenuItem
            // 
            this.enumerateGPOToolStripMenuItem.Name = "enumerateGPOToolStripMenuItem";
            this.enumerateGPOToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.enumerateGPOToolStripMenuItem.Text = "Enumerate GPO";
            // 
            // getDomainUserToolStripMenuItem
            // 
            this.getDomainUserToolStripMenuItem.Name = "getDomainUserToolStripMenuItem";
            this.getDomainUserToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.getDomainUserToolStripMenuItem.Text = "Get Domain User";
            // 
            // hostToolStripMenuItem
            // 
            this.hostToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portScanToolStripMenuItem,
            this.enumerateSessionsToolStripMenuItem});
            this.hostToolStripMenuItem.Name = "hostToolStripMenuItem";
            this.hostToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.hostToolStripMenuItem.Text = "Host";
            // 
            // portScanToolStripMenuItem
            // 
            this.portScanToolStripMenuItem.Name = "portScanToolStripMenuItem";
            this.portScanToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.portScanToolStripMenuItem.Text = "Port Scan";
            // 
            // enumerateSessionsToolStripMenuItem
            // 
            this.enumerateSessionsToolStripMenuItem.Name = "enumerateSessionsToolStripMenuItem";
            this.enumerateSessionsToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.enumerateSessionsToolStripMenuItem.Text = "Enumerate Sessions";
            // 
            // lateralMovementToolStripMenuItem2
            // 
            this.lateralMovementToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.passTheHashToolStripMenuItem1,
            this.wMIMethodToolStripMenuItem,
            this.dCOMToolStripMenuItem});
            this.lateralMovementToolStripMenuItem2.Name = "lateralMovementToolStripMenuItem2";
            this.lateralMovementToolStripMenuItem2.Size = new System.Drawing.Size(192, 22);
            this.lateralMovementToolStripMenuItem2.Text = "Lateral Movement";
            // 
            // passTheHashToolStripMenuItem1
            // 
            this.passTheHashToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wMIExecToolStripMenuItem1,
            this.sMBExecToolStripMenuItem1});
            this.passTheHashToolStripMenuItem1.Name = "passTheHashToolStripMenuItem1";
            this.passTheHashToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            this.passTheHashToolStripMenuItem1.Text = "Pass the Hash";
            // 
            // wMIExecToolStripMenuItem1
            // 
            this.wMIExecToolStripMenuItem1.Name = "wMIExecToolStripMenuItem1";
            this.wMIExecToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.wMIExecToolStripMenuItem1.Text = "WMI Command Exec";
            this.wMIExecToolStripMenuItem1.Click += new System.EventHandler(this.wMIExecToolStripMenuItem1_Click);
            // 
            // sMBExecToolStripMenuItem1
            // 
            this.sMBExecToolStripMenuItem1.Name = "sMBExecToolStripMenuItem1";
            this.sMBExecToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.sMBExecToolStripMenuItem1.Text = "SMB Command Exec";
            this.sMBExecToolStripMenuItem1.Click += new System.EventHandler(this.sMBExecToolStripMenuItem1_Click);
            // 
            // wMIMethodToolStripMenuItem
            // 
            this.wMIMethodToolStripMenuItem.Name = "wMIMethodToolStripMenuItem";
            this.wMIMethodToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.wMIMethodToolStripMenuItem.Text = "WMI";
            // 
            // dCOMToolStripMenuItem
            // 
            this.dCOMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSWORDDDEToolStripMenuItem,
            this.excelDDEToolStripMenuItem,
            this.mMC20ApplicationToolStripMenuItem,
            this.sHELLWINDOWSToolStripMenuItem,
            this.shellBrowserWindowToolStripMenuItem,
            this.visioAddonExecutionToolStripMenuItem,
            this.outlookExecutionToolStripMenuItem,
            this.arbitraryLibraryLoadToolStripMenuItem,
            this.wordWLLAddInToolStripMenuItem,
            this.outlookScriptExecutionToolStripMenuItem,
            this.visioExecuteLineToolStripMenuItem,
            this.officeMacroToolStripMenuItem});
            this.dCOMToolStripMenuItem.Name = "dCOMToolStripMenuItem";
            this.dCOMToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.dCOMToolStripMenuItem.Text = "DCOM";
            // 
            // mSWORDDDEToolStripMenuItem
            // 
            this.mSWORDDDEToolStripMenuItem.Name = "mSWORDDDEToolStripMenuItem";
            this.mSWORDDDEToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.mSWORDDDEToolStripMenuItem.Text = "MSWORD DDE";
            this.mSWORDDDEToolStripMenuItem.Click += new System.EventHandler(this.mSWORDDDEToolStripMenuItem_Click);
            // 
            // excelDDEToolStripMenuItem
            // 
            this.excelDDEToolStripMenuItem.Name = "excelDDEToolStripMenuItem";
            this.excelDDEToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.excelDDEToolStripMenuItem.Text = "Excel DDE";
            // 
            // mMC20ApplicationToolStripMenuItem
            // 
            this.mMC20ApplicationToolStripMenuItem.Name = "mMC20ApplicationToolStripMenuItem";
            this.mMC20ApplicationToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.mMC20ApplicationToolStripMenuItem.Text = "MMC20.Application";
            // 
            // sHELLWINDOWSToolStripMenuItem
            // 
            this.sHELLWINDOWSToolStripMenuItem.Name = "sHELLWINDOWSToolStripMenuItem";
            this.sHELLWINDOWSToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.sHELLWINDOWSToolStripMenuItem.Text = "ShellWindows";
            this.sHELLWINDOWSToolStripMenuItem.Click += new System.EventHandler(this.sHELLWINDOWSToolStripMenuItem_Click);
            // 
            // shellBrowserWindowToolStripMenuItem
            // 
            this.shellBrowserWindowToolStripMenuItem.Name = "shellBrowserWindowToolStripMenuItem";
            this.shellBrowserWindowToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.shellBrowserWindowToolStripMenuItem.Text = "ShellBrowserWindow";
            // 
            // visioAddonExecutionToolStripMenuItem
            // 
            this.visioAddonExecutionToolStripMenuItem.Name = "visioAddonExecutionToolStripMenuItem";
            this.visioAddonExecutionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.visioAddonExecutionToolStripMenuItem.Text = "Visio Addon Execution";
            // 
            // outlookExecutionToolStripMenuItem
            // 
            this.outlookExecutionToolStripMenuItem.Name = "outlookExecutionToolStripMenuItem";
            this.outlookExecutionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.outlookExecutionToolStripMenuItem.Text = "Outlook Execution";
            // 
            // arbitraryLibraryLoadToolStripMenuItem
            // 
            this.arbitraryLibraryLoadToolStripMenuItem.Name = "arbitraryLibraryLoadToolStripMenuItem";
            this.arbitraryLibraryLoadToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.arbitraryLibraryLoadToolStripMenuItem.Text = "Arbitrary Library Load";
            // 
            // wordWLLAddInToolStripMenuItem
            // 
            this.wordWLLAddInToolStripMenuItem.Name = "wordWLLAddInToolStripMenuItem";
            this.wordWLLAddInToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.wordWLLAddInToolStripMenuItem.Text = "Word WLL Add-In";
            // 
            // outlookScriptExecutionToolStripMenuItem
            // 
            this.outlookScriptExecutionToolStripMenuItem.Name = "outlookScriptExecutionToolStripMenuItem";
            this.outlookScriptExecutionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.outlookScriptExecutionToolStripMenuItem.Text = "OutlookScript Execution";
            // 
            // visioExecuteLineToolStripMenuItem
            // 
            this.visioExecuteLineToolStripMenuItem.Name = "visioExecuteLineToolStripMenuItem";
            this.visioExecuteLineToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.visioExecuteLineToolStripMenuItem.Text = "Visio ExecuteLine";
            // 
            // officeMacroToolStripMenuItem
            // 
            this.officeMacroToolStripMenuItem.Name = "officeMacroToolStripMenuItem";
            this.officeMacroToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.officeMacroToolStripMenuItem.Text = "Office Macro";
            // 
            // persistenceToolStripMenuItem
            // 
            this.persistenceToolStripMenuItem.Name = "persistenceToolStripMenuItem";
            this.persistenceToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.persistenceToolStripMenuItem.Text = "Persistence";
            // 
            // surveillanceToolStripMenuItem
            // 
            this.surveillanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteDesktopToolStripMenuItem,
            this.remoteWebcamToolStripMenuItem,
            this.passwordRecoveryToolStripMenuItem,
            this.keyloggerToolStripMenuItem});
            this.surveillanceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("surveillanceToolStripMenuItem.Image")));
            this.surveillanceToolStripMenuItem.Name = "surveillanceToolStripMenuItem";
            this.surveillanceToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.surveillanceToolStripMenuItem.Text = "Surveillance";
            // 
            // remoteDesktopToolStripMenuItem
            // 
            this.remoteDesktopToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("remoteDesktopToolStripMenuItem.Image")));
            this.remoteDesktopToolStripMenuItem.Name = "remoteDesktopToolStripMenuItem";
            this.remoteDesktopToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.remoteDesktopToolStripMenuItem.Text = "Remote Desktop";
            this.remoteDesktopToolStripMenuItem.Click += new System.EventHandler(this.remoteDesktopToolStripMenuItem_Click);
            // 
            // remoteWebcamToolStripMenuItem
            // 
            this.remoteWebcamToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("remoteWebcamToolStripMenuItem.Image")));
            this.remoteWebcamToolStripMenuItem.Name = "remoteWebcamToolStripMenuItem";
            this.remoteWebcamToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.remoteWebcamToolStripMenuItem.Text = "Remote Webcam";
            this.remoteWebcamToolStripMenuItem.Click += new System.EventHandler(this.remoteWebcamToolStripMenuItem_Click);
            // 
            // passwordRecoveryToolStripMenuItem
            // 
            this.passwordRecoveryToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("passwordRecoveryToolStripMenuItem.Image")));
            this.passwordRecoveryToolStripMenuItem.Name = "passwordRecoveryToolStripMenuItem";
            this.passwordRecoveryToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.passwordRecoveryToolStripMenuItem.Text = "Browser Password Recovery";
            this.passwordRecoveryToolStripMenuItem.Click += new System.EventHandler(this.passwordRecoveryToolStripMenuItem_Click);
            // 
            // keyloggerToolStripMenuItem
            // 
            this.keyloggerToolStripMenuItem.Image = global::xServer.Properties.Resources.logger;
            this.keyloggerToolStripMenuItem.Name = "keyloggerToolStripMenuItem";
            this.keyloggerToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.keyloggerToolStripMenuItem.Text = "Keylogger";
            this.keyloggerToolStripMenuItem.Click += new System.EventHandler(this.keyloggerToolStripMenuItem_Click);
            // 
            // miscellaneousToolStripMenuItem
            // 
            this.miscellaneousToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteExecuteToolStripMenuItem,
            this.visitWebsiteToolStripMenuItem,
            this.showMessageboxToolStripMenuItem,
            this.openChatToolStripMenuItem});
            this.miscellaneousToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("miscellaneousToolStripMenuItem.Image")));
            this.miscellaneousToolStripMenuItem.Name = "miscellaneousToolStripMenuItem";
            this.miscellaneousToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.miscellaneousToolStripMenuItem.Text = "Miscellaneous";
            // 
            // remoteExecuteToolStripMenuItem
            // 
            this.remoteExecuteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localFileToolStripMenuItem,
            this.webFileToolStripMenuItem,
            this.localFileInMemoryToolStripMenuItem,
            this.nETAssemblyInMemoryToolStripMenuItem});
            this.remoteExecuteToolStripMenuItem.Image = global::xServer.Properties.Resources.lightning;
            this.remoteExecuteToolStripMenuItem.Name = "remoteExecuteToolStripMenuItem";
            this.remoteExecuteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.remoteExecuteToolStripMenuItem.Text = "Remote Execute";
            // 
            // localFileToolStripMenuItem
            // 
            this.localFileToolStripMenuItem.Image = global::xServer.Properties.Resources.drive_go;
            this.localFileToolStripMenuItem.Name = "localFileToolStripMenuItem";
            this.localFileToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.localFileToolStripMenuItem.Text = "Local File...";
            this.localFileToolStripMenuItem.Click += new System.EventHandler(this.localFileToolStripMenuItem_Click);
            // 
            // webFileToolStripMenuItem
            // 
            this.webFileToolStripMenuItem.Image = global::xServer.Properties.Resources.world_go;
            this.webFileToolStripMenuItem.Name = "webFileToolStripMenuItem";
            this.webFileToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.webFileToolStripMenuItem.Text = "Web File...";
            this.webFileToolStripMenuItem.Click += new System.EventHandler(this.webFileToolStripMenuItem_Click);
            // 
            // localFileInMemoryToolStripMenuItem
            // 
            this.localFileInMemoryToolStripMenuItem.Name = "localFileInMemoryToolStripMenuItem";
            this.localFileInMemoryToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.localFileInMemoryToolStripMenuItem.Text = "Local PE File (In Memory)";
            // 
            // nETAssemblyInMemoryToolStripMenuItem
            // 
            this.nETAssemblyInMemoryToolStripMenuItem.Name = "nETAssemblyInMemoryToolStripMenuItem";
            this.nETAssemblyInMemoryToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.nETAssemblyInMemoryToolStripMenuItem.Text = ".NET Assembly (In Memory)";
            this.nETAssemblyInMemoryToolStripMenuItem.Click += new System.EventHandler(this.nETAssemblyInMemoryToolStripMenuItem_Click);
            // 
            // visitWebsiteToolStripMenuItem
            // 
            this.visitWebsiteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("visitWebsiteToolStripMenuItem.Image")));
            this.visitWebsiteToolStripMenuItem.Name = "visitWebsiteToolStripMenuItem";
            this.visitWebsiteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.visitWebsiteToolStripMenuItem.Text = "Visit Website";
            this.visitWebsiteToolStripMenuItem.Click += new System.EventHandler(this.visitWebsiteToolStripMenuItem_Click);
            // 
            // showMessageboxToolStripMenuItem
            // 
            this.showMessageboxToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showMessageboxToolStripMenuItem.Image")));
            this.showMessageboxToolStripMenuItem.Name = "showMessageboxToolStripMenuItem";
            this.showMessageboxToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.showMessageboxToolStripMenuItem.Text = "Show Messagebox";
            this.showMessageboxToolStripMenuItem.Click += new System.EventHandler(this.showMessageboxToolStripMenuItem_Click);
            // 
            // openChatToolStripMenuItem
            // 
            this.openChatToolStripMenuItem.Name = "openChatToolStripMenuItem";
            this.openChatToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.openChatToolStripMenuItem.Text = "Open Chat";
            this.openChatToolStripMenuItem.Click += new System.EventHandler(this.openChatToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(158, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.selectAllToolStripMenuItem.Text = "Select All Clients";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // imgFlags
            // 
            this.imgFlags.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFlags.ImageStream")));
            this.imgFlags.TransparentColor = System.Drawing.Color.Transparent;
            this.imgFlags.Images.SetKeyName(0, "ad.png");
            this.imgFlags.Images.SetKeyName(1, "ae.png");
            this.imgFlags.Images.SetKeyName(2, "af.png");
            this.imgFlags.Images.SetKeyName(3, "ag.png");
            this.imgFlags.Images.SetKeyName(4, "ai.png");
            this.imgFlags.Images.SetKeyName(5, "al.png");
            this.imgFlags.Images.SetKeyName(6, "am.png");
            this.imgFlags.Images.SetKeyName(7, "an.png");
            this.imgFlags.Images.SetKeyName(8, "ao.png");
            this.imgFlags.Images.SetKeyName(9, "ar.png");
            this.imgFlags.Images.SetKeyName(10, "as.png");
            this.imgFlags.Images.SetKeyName(11, "at.png");
            this.imgFlags.Images.SetKeyName(12, "au.png");
            this.imgFlags.Images.SetKeyName(13, "aw.png");
            this.imgFlags.Images.SetKeyName(14, "ax.png");
            this.imgFlags.Images.SetKeyName(15, "az.png");
            this.imgFlags.Images.SetKeyName(16, "ba.png");
            this.imgFlags.Images.SetKeyName(17, "bb.png");
            this.imgFlags.Images.SetKeyName(18, "bd.png");
            this.imgFlags.Images.SetKeyName(19, "be.png");
            this.imgFlags.Images.SetKeyName(20, "bf.png");
            this.imgFlags.Images.SetKeyName(21, "bg.png");
            this.imgFlags.Images.SetKeyName(22, "bh.png");
            this.imgFlags.Images.SetKeyName(23, "bi.png");
            this.imgFlags.Images.SetKeyName(24, "bj.png");
            this.imgFlags.Images.SetKeyName(25, "bm.png");
            this.imgFlags.Images.SetKeyName(26, "bn.png");
            this.imgFlags.Images.SetKeyName(27, "bo.png");
            this.imgFlags.Images.SetKeyName(28, "br.png");
            this.imgFlags.Images.SetKeyName(29, "bs.png");
            this.imgFlags.Images.SetKeyName(30, "bt.png");
            this.imgFlags.Images.SetKeyName(31, "bv.png");
            this.imgFlags.Images.SetKeyName(32, "bw.png");
            this.imgFlags.Images.SetKeyName(33, "by.png");
            this.imgFlags.Images.SetKeyName(34, "bz.png");
            this.imgFlags.Images.SetKeyName(35, "ca.png");
            this.imgFlags.Images.SetKeyName(36, "catalonia.png");
            this.imgFlags.Images.SetKeyName(37, "cc.png");
            this.imgFlags.Images.SetKeyName(38, "cd.png");
            this.imgFlags.Images.SetKeyName(39, "cf.png");
            this.imgFlags.Images.SetKeyName(40, "cg.png");
            this.imgFlags.Images.SetKeyName(41, "ch.png");
            this.imgFlags.Images.SetKeyName(42, "ci.png");
            this.imgFlags.Images.SetKeyName(43, "ck.png");
            this.imgFlags.Images.SetKeyName(44, "cl.png");
            this.imgFlags.Images.SetKeyName(45, "cm.png");
            this.imgFlags.Images.SetKeyName(46, "cn.png");
            this.imgFlags.Images.SetKeyName(47, "co.png");
            this.imgFlags.Images.SetKeyName(48, "cr.png");
            this.imgFlags.Images.SetKeyName(49, "cs.png");
            this.imgFlags.Images.SetKeyName(50, "cu.png");
            this.imgFlags.Images.SetKeyName(51, "cv.png");
            this.imgFlags.Images.SetKeyName(52, "cx.png");
            this.imgFlags.Images.SetKeyName(53, "cy.png");
            this.imgFlags.Images.SetKeyName(54, "cz.png");
            this.imgFlags.Images.SetKeyName(55, "de.png");
            this.imgFlags.Images.SetKeyName(56, "dj.png");
            this.imgFlags.Images.SetKeyName(57, "dk.png");
            this.imgFlags.Images.SetKeyName(58, "dm.png");
            this.imgFlags.Images.SetKeyName(59, "do.png");
            this.imgFlags.Images.SetKeyName(60, "dz.png");
            this.imgFlags.Images.SetKeyName(61, "ec.png");
            this.imgFlags.Images.SetKeyName(62, "ee.png");
            this.imgFlags.Images.SetKeyName(63, "eg.png");
            this.imgFlags.Images.SetKeyName(64, "eh.png");
            this.imgFlags.Images.SetKeyName(65, "england.png");
            this.imgFlags.Images.SetKeyName(66, "er.png");
            this.imgFlags.Images.SetKeyName(67, "es.png");
            this.imgFlags.Images.SetKeyName(68, "et.png");
            this.imgFlags.Images.SetKeyName(69, "europeanunion.png");
            this.imgFlags.Images.SetKeyName(70, "fam.png");
            this.imgFlags.Images.SetKeyName(71, "fi.png");
            this.imgFlags.Images.SetKeyName(72, "fj.png");
            this.imgFlags.Images.SetKeyName(73, "fk.png");
            this.imgFlags.Images.SetKeyName(74, "fm.png");
            this.imgFlags.Images.SetKeyName(75, "fo.png");
            this.imgFlags.Images.SetKeyName(76, "fr.png");
            this.imgFlags.Images.SetKeyName(77, "ga.png");
            this.imgFlags.Images.SetKeyName(78, "gb.png");
            this.imgFlags.Images.SetKeyName(79, "gd.png");
            this.imgFlags.Images.SetKeyName(80, "ge.png");
            this.imgFlags.Images.SetKeyName(81, "gf.png");
            this.imgFlags.Images.SetKeyName(82, "gh.png");
            this.imgFlags.Images.SetKeyName(83, "gi.png");
            this.imgFlags.Images.SetKeyName(84, "gl.png");
            this.imgFlags.Images.SetKeyName(85, "gm.png");
            this.imgFlags.Images.SetKeyName(86, "gn.png");
            this.imgFlags.Images.SetKeyName(87, "gp.png");
            this.imgFlags.Images.SetKeyName(88, "gq.png");
            this.imgFlags.Images.SetKeyName(89, "gr.png");
            this.imgFlags.Images.SetKeyName(90, "gs.png");
            this.imgFlags.Images.SetKeyName(91, "gt.png");
            this.imgFlags.Images.SetKeyName(92, "gu.png");
            this.imgFlags.Images.SetKeyName(93, "gw.png");
            this.imgFlags.Images.SetKeyName(94, "gy.png");
            this.imgFlags.Images.SetKeyName(95, "hk.png");
            this.imgFlags.Images.SetKeyName(96, "hm.png");
            this.imgFlags.Images.SetKeyName(97, "hn.png");
            this.imgFlags.Images.SetKeyName(98, "hr.png");
            this.imgFlags.Images.SetKeyName(99, "ht.png");
            this.imgFlags.Images.SetKeyName(100, "hu.png");
            this.imgFlags.Images.SetKeyName(101, "id.png");
            this.imgFlags.Images.SetKeyName(102, "ie.png");
            this.imgFlags.Images.SetKeyName(103, "il.png");
            this.imgFlags.Images.SetKeyName(104, "in.png");
            this.imgFlags.Images.SetKeyName(105, "io.png");
            this.imgFlags.Images.SetKeyName(106, "iq.png");
            this.imgFlags.Images.SetKeyName(107, "ir.png");
            this.imgFlags.Images.SetKeyName(108, "is.png");
            this.imgFlags.Images.SetKeyName(109, "it.png");
            this.imgFlags.Images.SetKeyName(110, "jm.png");
            this.imgFlags.Images.SetKeyName(111, "jo.png");
            this.imgFlags.Images.SetKeyName(112, "jp.png");
            this.imgFlags.Images.SetKeyName(113, "ke.png");
            this.imgFlags.Images.SetKeyName(114, "kg.png");
            this.imgFlags.Images.SetKeyName(115, "kh.png");
            this.imgFlags.Images.SetKeyName(116, "ki.png");
            this.imgFlags.Images.SetKeyName(117, "km.png");
            this.imgFlags.Images.SetKeyName(118, "kn.png");
            this.imgFlags.Images.SetKeyName(119, "kp.png");
            this.imgFlags.Images.SetKeyName(120, "kr.png");
            this.imgFlags.Images.SetKeyName(121, "kw.png");
            this.imgFlags.Images.SetKeyName(122, "ky.png");
            this.imgFlags.Images.SetKeyName(123, "kz.png");
            this.imgFlags.Images.SetKeyName(124, "la.png");
            this.imgFlags.Images.SetKeyName(125, "lb.png");
            this.imgFlags.Images.SetKeyName(126, "lc.png");
            this.imgFlags.Images.SetKeyName(127, "li.png");
            this.imgFlags.Images.SetKeyName(128, "lk.png");
            this.imgFlags.Images.SetKeyName(129, "lr.png");
            this.imgFlags.Images.SetKeyName(130, "ls.png");
            this.imgFlags.Images.SetKeyName(131, "lt.png");
            this.imgFlags.Images.SetKeyName(132, "lu.png");
            this.imgFlags.Images.SetKeyName(133, "lv.png");
            this.imgFlags.Images.SetKeyName(134, "ly.png");
            this.imgFlags.Images.SetKeyName(135, "ma.png");
            this.imgFlags.Images.SetKeyName(136, "mc.png");
            this.imgFlags.Images.SetKeyName(137, "md.png");
            this.imgFlags.Images.SetKeyName(138, "me.png");
            this.imgFlags.Images.SetKeyName(139, "mg.png");
            this.imgFlags.Images.SetKeyName(140, "mh.png");
            this.imgFlags.Images.SetKeyName(141, "mk.png");
            this.imgFlags.Images.SetKeyName(142, "ml.png");
            this.imgFlags.Images.SetKeyName(143, "mm.png");
            this.imgFlags.Images.SetKeyName(144, "mn.png");
            this.imgFlags.Images.SetKeyName(145, "mo.png");
            this.imgFlags.Images.SetKeyName(146, "mp.png");
            this.imgFlags.Images.SetKeyName(147, "mq.png");
            this.imgFlags.Images.SetKeyName(148, "mr.png");
            this.imgFlags.Images.SetKeyName(149, "ms.png");
            this.imgFlags.Images.SetKeyName(150, "mt.png");
            this.imgFlags.Images.SetKeyName(151, "mu.png");
            this.imgFlags.Images.SetKeyName(152, "mv.png");
            this.imgFlags.Images.SetKeyName(153, "mw.png");
            this.imgFlags.Images.SetKeyName(154, "mx.png");
            this.imgFlags.Images.SetKeyName(155, "my.png");
            this.imgFlags.Images.SetKeyName(156, "mz.png");
            this.imgFlags.Images.SetKeyName(157, "na.png");
            this.imgFlags.Images.SetKeyName(158, "nc.png");
            this.imgFlags.Images.SetKeyName(159, "ne.png");
            this.imgFlags.Images.SetKeyName(160, "nf.png");
            this.imgFlags.Images.SetKeyName(161, "ng.png");
            this.imgFlags.Images.SetKeyName(162, "ni.png");
            this.imgFlags.Images.SetKeyName(163, "nl.png");
            this.imgFlags.Images.SetKeyName(164, "no.png");
            this.imgFlags.Images.SetKeyName(165, "np.png");
            this.imgFlags.Images.SetKeyName(166, "nr.png");
            this.imgFlags.Images.SetKeyName(167, "nu.png");
            this.imgFlags.Images.SetKeyName(168, "nz.png");
            this.imgFlags.Images.SetKeyName(169, "om.png");
            this.imgFlags.Images.SetKeyName(170, "pa.png");
            this.imgFlags.Images.SetKeyName(171, "pe.png");
            this.imgFlags.Images.SetKeyName(172, "pf.png");
            this.imgFlags.Images.SetKeyName(173, "pg.png");
            this.imgFlags.Images.SetKeyName(174, "ph.png");
            this.imgFlags.Images.SetKeyName(175, "pk.png");
            this.imgFlags.Images.SetKeyName(176, "pl.png");
            this.imgFlags.Images.SetKeyName(177, "pm.png");
            this.imgFlags.Images.SetKeyName(178, "pn.png");
            this.imgFlags.Images.SetKeyName(179, "pr.png");
            this.imgFlags.Images.SetKeyName(180, "ps.png");
            this.imgFlags.Images.SetKeyName(181, "pt.png");
            this.imgFlags.Images.SetKeyName(182, "pw.png");
            this.imgFlags.Images.SetKeyName(183, "py.png");
            this.imgFlags.Images.SetKeyName(184, "qa.png");
            this.imgFlags.Images.SetKeyName(185, "re.png");
            this.imgFlags.Images.SetKeyName(186, "ro.png");
            this.imgFlags.Images.SetKeyName(187, "rs.png");
            this.imgFlags.Images.SetKeyName(188, "ru.png");
            this.imgFlags.Images.SetKeyName(189, "rw.png");
            this.imgFlags.Images.SetKeyName(190, "sa.png");
            this.imgFlags.Images.SetKeyName(191, "sb.png");
            this.imgFlags.Images.SetKeyName(192, "sc.png");
            this.imgFlags.Images.SetKeyName(193, "scotland.png");
            this.imgFlags.Images.SetKeyName(194, "sd.png");
            this.imgFlags.Images.SetKeyName(195, "se.png");
            this.imgFlags.Images.SetKeyName(196, "sg.png");
            this.imgFlags.Images.SetKeyName(197, "sh.png");
            this.imgFlags.Images.SetKeyName(198, "si.png");
            this.imgFlags.Images.SetKeyName(199, "sj.png");
            this.imgFlags.Images.SetKeyName(200, "sk.png");
            this.imgFlags.Images.SetKeyName(201, "sl.png");
            this.imgFlags.Images.SetKeyName(202, "sm.png");
            this.imgFlags.Images.SetKeyName(203, "sn.png");
            this.imgFlags.Images.SetKeyName(204, "so.png");
            this.imgFlags.Images.SetKeyName(205, "sr.png");
            this.imgFlags.Images.SetKeyName(206, "st.png");
            this.imgFlags.Images.SetKeyName(207, "sv.png");
            this.imgFlags.Images.SetKeyName(208, "sy.png");
            this.imgFlags.Images.SetKeyName(209, "sz.png");
            this.imgFlags.Images.SetKeyName(210, "tc.png");
            this.imgFlags.Images.SetKeyName(211, "td.png");
            this.imgFlags.Images.SetKeyName(212, "tf.png");
            this.imgFlags.Images.SetKeyName(213, "tg.png");
            this.imgFlags.Images.SetKeyName(214, "th.png");
            this.imgFlags.Images.SetKeyName(215, "tj.png");
            this.imgFlags.Images.SetKeyName(216, "tk.png");
            this.imgFlags.Images.SetKeyName(217, "tl.png");
            this.imgFlags.Images.SetKeyName(218, "tm.png");
            this.imgFlags.Images.SetKeyName(219, "tn.png");
            this.imgFlags.Images.SetKeyName(220, "to.png");
            this.imgFlags.Images.SetKeyName(221, "tr.png");
            this.imgFlags.Images.SetKeyName(222, "tt.png");
            this.imgFlags.Images.SetKeyName(223, "tv.png");
            this.imgFlags.Images.SetKeyName(224, "tw.png");
            this.imgFlags.Images.SetKeyName(225, "tz.png");
            this.imgFlags.Images.SetKeyName(226, "ua.png");
            this.imgFlags.Images.SetKeyName(227, "ug.png");
            this.imgFlags.Images.SetKeyName(228, "um.png");
            this.imgFlags.Images.SetKeyName(229, "us.png");
            this.imgFlags.Images.SetKeyName(230, "uy.png");
            this.imgFlags.Images.SetKeyName(231, "uz.png");
            this.imgFlags.Images.SetKeyName(232, "va.png");
            this.imgFlags.Images.SetKeyName(233, "vc.png");
            this.imgFlags.Images.SetKeyName(234, "ve.png");
            this.imgFlags.Images.SetKeyName(235, "vg.png");
            this.imgFlags.Images.SetKeyName(236, "vi.png");
            this.imgFlags.Images.SetKeyName(237, "vn.png");
            this.imgFlags.Images.SetKeyName(238, "vu.png");
            this.imgFlags.Images.SetKeyName(239, "wales.png");
            this.imgFlags.Images.SetKeyName(240, "wf.png");
            this.imgFlags.Images.SetKeyName(241, "ws.png");
            this.imgFlags.Images.SetKeyName(242, "ye.png");
            this.imgFlags.Images.SetKeyName(243, "yt.png");
            this.imgFlags.Images.SetKeyName(244, "za.png");
            this.imgFlags.Images.SetKeyName(245, "zm.png");
            this.imgFlags.Images.SetKeyName(246, "zw.png");
            this.imgFlags.Images.SetKeyName(247, "xy.png");
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Quasar";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.statusStrip, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lstClients, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.menuStrip, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1022, 458);
            this.tableLayoutPanel.TabIndex = 6;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listenToolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 436);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1022, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip_ItemClicked);
            // 
            // listenToolStripStatusLabel
            // 
            this.listenToolStripStatusLabel.Name = "listenToolStripStatusLabel";
            this.listenToolStripStatusLabel.Size = new System.Drawing.Size(87, 17);
            this.listenToolStripStatusLabel.Text = "Listening: False";
            // 
            // lstClients
            // 
            this.lstClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hIP,
            this.hTag,
            this.hUserPC,
            this.hVersion,
            this.hStatus,
            this.hUserStatus,
            this.hCountry,
            this.hOS,
            this.hAccountType});
            this.lstClients.ContextMenuStrip = this.contextMenuStrip;
            this.lstClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstClients.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstClients.FullRowSelect = true;
            this.lstClients.Location = new System.Drawing.Point(3, 28);
            this.lstClients.Name = "lstClients";
            this.lstClients.ShowItemToolTips = true;
            this.lstClients.Size = new System.Drawing.Size(1016, 405);
            this.lstClients.SmallImageList = this.imgFlags;
            this.lstClients.TabIndex = 1;
            this.lstClients.UseCompatibleStateImageBehavior = false;
            this.lstClients.View = System.Windows.Forms.View.Details;
            this.lstClients.SelectedIndexChanged += new System.EventHandler(this.lstClients_SelectedIndexChanged);
            // 
            // hIP
            // 
            this.hIP.Text = "IP Address";
            this.hIP.Width = 112;
            // 
            // hTag
            // 
            this.hTag.Text = "Tag";
            // 
            // hUserPC
            // 
            this.hUserPC.Text = "User@PC";
            this.hUserPC.Width = 175;
            // 
            // hVersion
            // 
            this.hVersion.Text = "Version";
            this.hVersion.Width = 66;
            // 
            // hStatus
            // 
            this.hStatus.Text = "Status";
            this.hStatus.Width = 78;
            // 
            // hUserStatus
            // 
            this.hUserStatus.Text = "User Status";
            this.hUserStatus.Width = 72;
            // 
            // hCountry
            // 
            this.hCountry.Text = "Country";
            this.hCountry.Width = 117;
            // 
            // hOS
            // 
            this.hOS.Text = "Operating System";
            this.hOS.Width = 222;
            // 
            // hAccountType
            // 
            this.hAccountType.Text = "Account Type";
            this.hAccountType.Width = 100;
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.builderToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.serverToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(267, 25);
            this.menuStrip.TabIndex = 2;
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fIleToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // builderToolStripMenuItem
            // 
            this.builderToolStripMenuItem.Name = "builderToolStripMenuItem";
            this.builderToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.builderToolStripMenuItem.Text = "Builder";
            this.builderToolStripMenuItem.Click += new System.EventHandler(this.builderToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hostFileToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // hostFileToolStripMenuItem
            // 
            this.hostFileToolStripMenuItem.Name = "hostFileToolStripMenuItem";
            this.hostFileToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.hostFileToolStripMenuItem.Text = "Host File";
            this.hostFileToolStripMenuItem.Click += new System.EventHandler(this.hostFileToolStripMenuItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1022, 458);
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(680, 415);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quasar - Connected: 0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader hIP;
        private System.Windows.Forms.ColumnHeader hVersion;
        private System.Windows.Forms.ColumnHeader hCountry;
        private System.Windows.Forms.ColumnHeader hOS;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader hTag;
        private System.Windows.Forms.ImageList imgFlags;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader hStatus;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surveillanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteDesktopToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader hAccountType;
        private System.Windows.Forms.ColumnHeader hUserStatus;
        private System.Windows.Forms.ToolStripMenuItem miscellaneousToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem passwordRecoveryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMessageboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator ctxtLine;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem standbyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteExecuteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyloggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverseProxyToolStripMenuItem;
        private AeroListView lstClients;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ColumnHeader hUserPC;
        private System.Windows.Forms.ToolStripSeparator lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fIleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem builderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel listenToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem remoteWebcamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openChatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informationGatheringToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem activeDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lateralMovementToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem passTheHashToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem wMIExecToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sMBExecToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem privilegeEscalationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem privilegeEscalationChecksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem credentialHarvestingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portScanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem persistenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getCurrentDomainControllerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getFileServersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enumerateGPOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getDomainUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enumerateSessionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dCOMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wMIMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelDDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mMC20ApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sHELLWINDOWSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shellBrowserWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visioAddonExecutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outlookExecutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arbitraryLibraryLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWLLAddInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outlookScriptExecutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visioExecuteLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officeMacroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informationGatheringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemInformationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem askToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileManagerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem startupManagerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem remoteShellToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem taskManagerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tCPConnectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registryEditorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runLocalWMICommandToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem situationalAwarenessChecksToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mimikatzToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem persistenceToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem wMIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registryRunKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scheduledTasksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hostFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localFileInMemoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nETAssemblyInMemoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionGopherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSWORDDDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem impersonationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stealProcessTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertToSelfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beginImpersonationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerPickToolStripMenuItem;
    }
}

