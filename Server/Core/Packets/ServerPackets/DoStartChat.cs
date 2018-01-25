using System;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ServerPackets
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
