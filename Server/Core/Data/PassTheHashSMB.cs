using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xServer.Core.Data
{
    public class PassTheHashSMB
    {
        public string command { get; set; }
        public string username { get; set; }
        public string hash { get; set; }
        public string target { get; set; }
        public string domain { get; set; }
        public string service { get; set; }
        public bool smb1 { get; set; }
        public bool comspec { get; set; }
        public int sleep { get; set; }
    }
}
