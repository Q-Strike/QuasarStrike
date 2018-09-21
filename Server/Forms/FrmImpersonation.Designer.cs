namespace xServer.Forms
{
    partial class FrmImpersonation
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
            this.dgTokens = new System.Windows.Forms.DataGridView();
            this.colDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnImpersonate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStopImpersonation = new System.Windows.Forms.Button();
            this.btnCmdImpersonate = new System.Windows.Forms.Button();
            this.guid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgTokens)).BeginInit();
            this.SuspendLayout();
            // 
            // dgTokens
            // 
            this.dgTokens.AllowUserToAddRows = false;
            this.dgTokens.AllowUserToDeleteRows = false;
            this.dgTokens.AllowUserToResizeRows = false;
            this.dgTokens.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgTokens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgTokens.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTokens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDomain,
            this.colUsername,
            this.guid});
            this.dgTokens.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgTokens.Location = new System.Drawing.Point(4, 23);
            this.dgTokens.MultiSelect = false;
            this.dgTokens.Name = "dgTokens";
            this.dgTokens.ReadOnly = true;
            this.dgTokens.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTokens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTokens.Size = new System.Drawing.Size(345, 278);
            this.dgTokens.TabIndex = 0;
            this.dgTokens.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTokens_CellContentClick);
            // 
            // colDomain
            // 
            this.colDomain.HeaderText = "Domain";
            this.colDomain.Name = "colDomain";
            this.colDomain.ReadOnly = true;
            // 
            // colUsername
            // 
            this.colUsername.HeaderText = "Username";
            this.colUsername.Name = "colUsername";
            this.colUsername.ReadOnly = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(355, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(116, 45);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh Available Tokens";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnImpersonate
            // 
            this.btnImpersonate.Location = new System.Drawing.Point(355, 63);
            this.btnImpersonate.Name = "btnImpersonate";
            this.btnImpersonate.Size = new System.Drawing.Size(116, 45);
            this.btnImpersonate.TabIndex = 2;
            this.btnImpersonate.Text = "Impersonate";
            this.btnImpersonate.UseVisualStyleBackColor = true;
            this.btnImpersonate.Click += new System.EventHandler(this.btnImpersonate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "The following impersonation tokens are available:";
            // 
            // btnStopImpersonation
            // 
            this.btnStopImpersonation.Location = new System.Drawing.Point(355, 114);
            this.btnStopImpersonation.Name = "btnStopImpersonation";
            this.btnStopImpersonation.Size = new System.Drawing.Size(116, 45);
            this.btnStopImpersonation.TabIndex = 4;
            this.btnStopImpersonation.Text = "Stop Impersonation";
            this.btnStopImpersonation.UseVisualStyleBackColor = true;
            this.btnStopImpersonation.Click += new System.EventHandler(this.btnStopImpersonation_Click);
            // 
            // btnCmdImpersonate
            // 
            this.btnCmdImpersonate.Location = new System.Drawing.Point(355, 165);
            this.btnCmdImpersonate.Name = "btnCmdImpersonate";
            this.btnCmdImpersonate.Size = new System.Drawing.Size(116, 45);
            this.btnCmdImpersonate.TabIndex = 5;
            this.btnCmdImpersonate.Text = "Launch Interactive cmd.exe";
            this.btnCmdImpersonate.UseVisualStyleBackColor = true;
            this.btnCmdImpersonate.Click += new System.EventHandler(this.btnCmdImpersonate_Click);
            // 
            // guid
            // 
            this.guid.HeaderText = "TokenID";
            this.guid.Name = "guid";
            this.guid.ReadOnly = true;
            // 
            // FrmImpersonation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 311);
            this.Controls.Add(this.btnCmdImpersonate);
            this.Controls.Add(this.btnStopImpersonation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImpersonate);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgTokens);
            this.Name = "FrmImpersonation";
            this.Text = "Available impersonations for <client>";
            this.Load += new System.EventHandler(this.ImperonationMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTokens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgTokens;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDomain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnImpersonate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStopImpersonation;
        private System.Windows.Forms.Button btnCmdImpersonate;
        private System.Windows.Forms.DataGridViewTextBoxColumn guid;
    }
}