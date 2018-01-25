using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using xClient.Core.Networking;
using System.Windows.Forms;

namespace xClient.Core.Forms
{
    public partial class FrmAgentChat : Form
    {
        Client c;
        public FrmAgentChat(Client client)
        {
            c = client;
            InitializeComponent();
        }

        private void btnSubmitMessage_Click(object sender, EventArgs e)
        {
            txtChatLog.AppendText("Agent >> " + txtMessage.Text + "\r\n");
            new Core.Packets.ClientPackets.GetMessageResponse(txtMessage.Text, "Agent").Execute(c);
        }
        public void AddMessage(string message, string sender)
        {
            txtChatLog.AppendText(sender + " << " + message);
        }
    }
}