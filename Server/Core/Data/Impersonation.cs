using System;
using xServer.Core.Networking;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xServer.Core.Data
{
    public class Impersonation
    {
        public string domain { get; set; }
        public string username { get; set; }
        public string agent { get; set; }
        public string guid { get; set; }

        public Impersonation()
        {

        }
        public Impersonation(string domain, string username, string agent, string guid)
        {
            this.domain = domain;
            this.username = username;
            this.agent = agent;
            this.guid = guid;
        }
    }
}
