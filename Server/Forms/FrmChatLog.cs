using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xServer.Core.Networking;

namespace xServer.Forms
{
    public partial class FrmChatLog : Form
    {
        private Client c;
        public FrmChatLog(Client client)
        {
            c = client;
            InitializeComponent();
        }

        private void btnSubmitMessage_Click(object sender, EventArgs e)
        {
            txtChatLog.AppendText("Server >> " + txtMessage.Text + "\r\n");
            new Core.Packets.ServerPackets.GetMessage(txtMessage.Text, "Server").Execute(c);
        }
        public void AddMessage(string message, string sender)
        {
            txtChatLog.AppendText(sender + " << " + message);
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChatLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
