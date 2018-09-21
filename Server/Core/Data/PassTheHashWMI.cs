using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xServer.Core.Data
{
    public class PassTheHashWMI 
    {
        public string target { get; set; }
        public string command { get; set; }
        public string username { get; set; }
        public string hash { get; set; }
        public string domain { get; set; }
        public int sleep { get; set; }
    }
}
