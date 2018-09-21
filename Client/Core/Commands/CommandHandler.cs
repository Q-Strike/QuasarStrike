using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using xClient.Core.Registry;
using xClient.Core.Utilities;
using xClient.Core.Helper;
using xClient.Core.Commands;
using xClient.Core.Data;
using xClient.Core.NetSerializer;
using xClient.Core.Packets;
using System;

namespace xClient.Core.Commands
{
    /* THIS PARTIAL CLASS SHOULD CONTAIN VARIABLES NECESSARY FOR VARIOUS COMMANDS (if needed). */
    public static partial class CommandHandler
    {
        //Could I store the Handles here?
        public static UnsafeStreamCodec StreamCodec;
        private static Shell _shell;
        private static System.Diagnostics.Process _impShell;
        private static Dictionary<int, string> _renamedFiles = new Dictionary<int, string>();
        private static Dictionary<int, string> _canceledDownloads = new Dictionary<int, string>();
        private const string DELIMITER = "$E$";
        private static readonly Semaphore _limitThreads = new Semaphore(2, 2); // maximum simultaneous file downloads
        public static Dictionary<string, ImpersonationSafeHandle> impersonatedUsers = new Dictionary<string, ImpersonationSafeHandle>();
        public static bool impersonate = false;
        public static string impersonatedUser = "";
        //SafeHandle iToken = new SafeHandle(IntPtr.Zero);
         
    }
}