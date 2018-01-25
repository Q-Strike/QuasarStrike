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
    public partial class frmClientLog : Form
    {
        private Client c;
        public frmClientLog(Client client)
        {
            c = client;
            InitializeComponent();
        }

        private void frmClientLog_Load(object sender, EventArgs e)
        {

            //Check if log already exists
            //If log exists, load it and fill the textbox
            //If log doesn't exist, create file and start appending to textbox.
            Form.ActiveForm.Text = c.Value.PCName + " Log.";
        }

        public void writeLog(string text)
        {
            txtConsoleOutput.AppendText(text);
        }
    }
}
