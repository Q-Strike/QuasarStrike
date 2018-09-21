using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xClient.Core.Helper
{
    public class DCOMHelper
    {
        public static void checkDDEMSWORD()
        {
            //Check the value at \HKEY_CURRENT_USER\Software\Microsoft\Office<version>\Word\Security AllowDDE
            //0: Disable DDE
            //1: Allow DDE to an already running program, bu prevent DDE requests that require another executable to be launched.
            //2: Fully allow DDE
        }
    }
}
