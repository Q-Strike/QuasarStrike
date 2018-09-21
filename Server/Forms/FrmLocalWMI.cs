using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace xServer.Forms
{
    public partial class FrmLocalWMI : Form
    {
        public FrmLocalWMI()
        {
            InitializeComponent();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Need to figure out how to parse results.
            //https://msdn.microsoft.com/en-us/library/cc143254.aspx
            //Maybe include a "Properties" string list and have the user provide what properties they want.

            string[] properties = txtProperties.Text.Split(',');
            new Core.Packets.ServerPackets.DoExecuteWMI(txtNamespace.Text,txtQuery.Text,txtTarget.Text,properties);
        }

        public void addToTextBox(ManagementObject obj, string[] properties)
        {
            foreach(PropertyData property in obj.Properties)
            {
                txtResults.AppendText(property.Name + " " + property.Value);
            }
        }
    }
}
