namespace xServer.Forms
{
    partial class FrmPTH
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.line5 = new xServer.Controls.Line();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.txtHash = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtService = new System.Windows.Forms.TextBox();
            this.chkComspec = new System.Windows.Forms.CheckBox();
            this.chkSMB = new System.Windows.Forms.CheckBox();
            this.txtSleep = new System.Windows.Forms.TextBox();
            this.line1 = new xServer.Controls.Line();
            this.label8 = new System.Windows.Forms.Label();
            this.btnExecutePTH = new System.Windows.Forms.Button();
            this.lblService = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Command";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Hash";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Domain";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Username";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Required Parameters";
            // 
            // line5
            // 
            this.line5.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line5.Location = new System.Drawing.Point(115, 12);
            this.line5.Name = "line5";
            this.line5.Size = new System.Drawing.Size(219, 13);
            this.line5.TabIndex = 16;
            this.line5.TabStop = false;
            // 
            // txtTarget
            // 
            this.txtTarget.BackColor = System.Drawing.SystemColors.Window;
            this.txtTarget.Location = new System.Drawing.Point(86, 37);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(196, 20);
            this.txtTarget.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtTarget, "IP Address or Hostname");
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(86, 188);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(196, 20);
            this.txtDomain.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtDomain, "(Optional) Can additionall be specified in the username field as username@test.lo" +
        "cal");
            // 
            // txtHash
            // 
            this.txtHash.Location = new System.Drawing.Point(86, 89);
            this.txtHash.Name = "txtHash";
            this.txtHash.Size = new System.Drawing.Size(196, 20);
            this.txtHash.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtHash, "Can be either combined NTLM or just the NT portion.");
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(86, 214);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(196, 20);
            this.txtCommand.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtCommand, "If this field is not specified, the module will just check to ensure the Username" +
        " & Hash have access to WMI on the targt system.");
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(86, 63);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(196, 20);
            this.txtUsername.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtUsername, "Username can be in the format: username or username@test.local");
            // 
            // txtService
            // 
            this.txtService.Location = new System.Drawing.Point(86, 266);
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(196, 20);
            this.txtService.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtService, "Default = 20 Char Random Name\r\nSpecifies the service name that is created and del" +
        "eted on the target.\r\n");
            // 
            // chkComspec
            // 
            this.chkComspec.AutoSize = true;
            this.chkComspec.Location = new System.Drawing.Point(177, 290);
            this.chkComspec.Name = "chkComspec";
            this.chkComspec.Size = new System.Drawing.Size(121, 17);
            this.chkComspec.TabIndex = 9;
            this.chkComspec.Text = "Prepend COMSPEC";
            this.toolTip1.SetToolTip(this.chkComspec, "Prepends %COMSPEC% /C to the command");
            this.chkComspec.UseVisualStyleBackColor = true;
            // 
            // chkSMB
            // 
            this.chkSMB.AutoSize = true;
            this.chkSMB.Location = new System.Drawing.Point(86, 290);
            this.chkSMB.Name = "chkSMB";
            this.chkSMB.Size = new System.Drawing.Size(85, 17);
            this.chkSMB.TabIndex = 8;
            this.chkSMB.Text = "Force SMB1";
            this.toolTip1.SetToolTip(this.chkSMB, "Forces the command to execute using SMB1");
            this.chkSMB.UseVisualStyleBackColor = true;
            // 
            // txtSleep
            // 
            this.txtSleep.Location = new System.Drawing.Point(86, 240);
            this.txtSleep.Name = "txtSleep";
            this.txtSleep.Size = new System.Drawing.Size(196, 20);
            this.txtSleep.TabIndex = 6;
            this.toolTip1.SetToolTip(this.txtSleep, "Default = 150ms. \r\nSets the sleep value in milliseconds.");
            // 
            // line1
            // 
            this.line1.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line1.Location = new System.Drawing.Point(115, 161);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(219, 13);
            this.line1.TabIndex = 24;
            this.line1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Optional Parameters";
            // 
            // btnExecutePTH
            // 
            this.btnExecutePTH.Location = new System.Drawing.Point(259, 127);
            this.btnExecutePTH.Name = "btnExecutePTH";
            this.btnExecutePTH.Size = new System.Drawing.Size(75, 23);
            this.btnExecutePTH.TabIndex = 10;
            this.btnExecutePTH.Text = "Execute";
            this.btnExecutePTH.UseVisualStyleBackColor = true;
            this.btnExecutePTH.Click += new System.EventHandler(this.btnExecutePTHWmi_Click);
            // 
            // lblService
            // 
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(10, 269);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(74, 13);
            this.lblService.TabIndex = 26;
            this.lblService.Text = "Service Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 243);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Sleep";
            // 
            // FrmPTH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 318);
            this.Controls.Add(this.txtSleep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkSMB);
            this.Controls.Add(this.chkComspec);
            this.Controls.Add(this.txtService);
            this.Controls.Add(this.lblService);
            this.Controls.Add(this.btnExecutePTH);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.txtHash);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.line5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "FrmPTH";
            this.Text = "Pass the Hash";
            this.Load += new System.EventHandler(this.FrmPTH_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Controls.Line line5;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.TextBox txtHash;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.TextBox txtUsername;
        private Controls.Line line1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExecutePTH;
        private System.Windows.Forms.TextBox txtService;
        private System.Windows.Forms.Label lblService;
        private System.Windows.Forms.CheckBox chkComspec;
        private System.Windows.Forms.CheckBox chkSMB;
        private System.Windows.Forms.TextBox txtSleep;
        private System.Windows.Forms.Label label2;
    }
}