namespace xServer.Forms
{
    partial class FrmImpersonate
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
            this.lblProcessID = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtProcID = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnDoStuff = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mskPassword = new System.Windows.Forms.MaskedTextBox();
            this.line6 = new xServer.Controls.Line();
            this.line1 = new xServer.Controls.Line();
            this.button1 = new System.Windows.Forms.Button();
            this.txtDomain = new System.Windows.Forms.MaskedTextBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProcessID
            // 
            this.lblProcessID.AutoSize = true;
            this.lblProcessID.Location = new System.Drawing.Point(12, 79);
            this.lblProcessID.Name = "lblProcessID";
            this.lblProcessID.Size = new System.Drawing.Size(62, 13);
            this.lblProcessID.TabIndex = 0;
            this.lblProcessID.Text = "ProcessID: ";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(13, 69);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(61, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username: ";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(15, 96);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password: ";
            // 
            // txtProcID
            // 
            this.txtProcID.Location = new System.Drawing.Point(80, 76);
            this.txtProcID.Name = "txtProcID";
            this.txtProcID.Size = new System.Drawing.Size(100, 20);
            this.txtProcID.TabIndex = 4;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(80, 66);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 5;
            // 
            // btnDoStuff
            // 
            this.btnDoStuff.Location = new System.Drawing.Point(186, 66);
            this.btnDoStuff.Name = "btnDoStuff";
            this.btnDoStuff.Size = new System.Drawing.Size(75, 23);
            this.btnDoStuff.TabIndex = 6;
            this.btnDoStuff.Text = "Execute";
            this.btnDoStuff.UseVisualStyleBackColor = true;
            this.btnDoStuff.Click += new System.EventHandler(this.btnDoStuff_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 39);
            this.label1.TabIndex = 7;
            this.label1.Text = "Description: \r\nThis module will alter the context of the selected Agent. You can " +
    "use the \r\nRevert To Self module to return to the original context.";
            // 
            // mskPassword
            // 
            this.mskPassword.Location = new System.Drawing.Point(80, 93);
            this.mskPassword.Name = "mskPassword";
            this.mskPassword.Size = new System.Drawing.Size(100, 20);
            this.mskPassword.TabIndex = 8;
            // 
            // line6
            // 
            this.line6.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line6.Location = new System.Drawing.Point(78, 9);
            this.line6.Name = "line6";
            this.line6.Size = new System.Drawing.Size(285, 13);
            this.line6.TabIndex = 21;
            this.line6.TabStop = false;
            // 
            // line1
            // 
            this.line1.LineAlignment = xServer.Controls.Line.Alignment.Horizontal;
            this.line1.Location = new System.Drawing.Point(12, 51);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(351, 13);
            this.line1.TabIndex = 22;
            this.line1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(80, 115);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(100, 20);
            this.txtDomain.TabIndex = 25;
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(25, 118);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(49, 13);
            this.lblDomain.TabIndex = 24;
            this.lblDomain.Text = "Domain: ";
            // 
            // FrmImpersonate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 148);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.lblDomain);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.line6);
            this.Controls.Add(this.mskPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDoStuff);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtProcID);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblProcessID);
            this.Name = "FrmImpersonate";
            this.Text = "FrmImpersonate";
            this.Load += new System.EventHandler(this.FrmImpersonate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessID;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtProcID;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnDoStuff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mskPassword;
        private Controls.Line line6;
        private Controls.Line line1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MaskedTextBox txtDomain;
        private System.Windows.Forms.Label lblDomain;
    }
}