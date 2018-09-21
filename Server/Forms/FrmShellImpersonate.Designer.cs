namespace xServer.Forms
{
    partial class FrmShellImpersonate
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
            this.txtConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.txtConsoleInput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConsoleOutput
            // 
            this.txtConsoleOutput.Location = new System.Drawing.Point(0, 0);
            this.txtConsoleOutput.Name = "txtConsoleOutput";
            this.txtConsoleOutput.Size = new System.Drawing.Size(100, 96);
            this.txtConsoleOutput.TabIndex = 5;
            this.txtConsoleOutput.Text = "";
            // 
            // txtConsoleInput
            // 
            this.txtConsoleInput.Location = new System.Drawing.Point(0, 0);
            this.txtConsoleInput.Name = "txtConsoleInput";
            this.txtConsoleInput.Size = new System.Drawing.Size(100, 20);
            this.txtConsoleInput.TabIndex = 4;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.txtConsole, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.txtInput, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(637, 294);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsole.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtConsole.Location = new System.Drawing.Point(3, 3);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(631, 268);
            this.txtConsole.TabIndex = 1;
            this.txtConsole.Text = "";
            // 
            // txtInput
            // 
            this.txtInput.BackColor = System.Drawing.Color.Black;
            this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtInput.Location = new System.Drawing.Point(3, 277);
            this.txtInput.MaxLength = 200;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(631, 16);
            this.txtInput.TabIndex = 0;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // FrmShellImpersonate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 294);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.txtConsoleInput);
            this.Controls.Add(this.txtConsoleOutput);
            this.Name = "FrmShellImpersonate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impersonation Shell []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmShellImpersonate_FormClosing);
            this.Load += new System.EventHandler(this.FrmShellImpersonate_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtConsoleOutput;
        private System.Windows.Forms.TextBox txtConsoleInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.TextBox txtInput;
    }
}