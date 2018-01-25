namespace xClient.Core.Forms
{
    partial class FrmAgentChat
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
            this.txtChatLog = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSubmitMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtChatLog
            // 
            this.txtChatLog.Location = new System.Drawing.Point(12, 40);
            this.txtChatLog.Multiline = true;
            this.txtChatLog.Name = "txtChatLog";
            this.txtChatLog.ReadOnly = true;
            this.txtChatLog.Size = new System.Drawing.Size(260, 209);
            this.txtChatLog.TabIndex = 5;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 14);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(226, 20);
            this.txtMessage.TabIndex = 4;
            // 
            // btnSubmitMessage
            // 
            this.btnSubmitMessage.Location = new System.Drawing.Point(244, 12);
            this.btnSubmitMessage.Name = "btnSubmitMessage";
            this.btnSubmitMessage.Size = new System.Drawing.Size(28, 23);
            this.btnSubmitMessage.TabIndex = 3;
            this.btnSubmitMessage.Text = "->";
            this.btnSubmitMessage.UseVisualStyleBackColor = true;
            this.btnSubmitMessage.Click += new System.EventHandler(this.btnSubmitMessage_Click);
            // 
            // FrmAgentChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtChatLog);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSubmitMessage);
            this.Name = "FrmAgentChat";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChatLog;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSubmitMessage;
    }
}