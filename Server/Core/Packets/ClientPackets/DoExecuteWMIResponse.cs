using System;
using System.Collections.Generic;
using xServer.Core.Networking;
using System.Linq;
using System.Text;

namespace xServer.Core.Packets.ClientPackets
{
    [Serializable]
    public class DoExecuteWMIResponse : IPacket
    {
        public string reponse { get; set; }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
