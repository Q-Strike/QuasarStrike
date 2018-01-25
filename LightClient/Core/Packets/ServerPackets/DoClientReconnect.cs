using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoClientReconnect : IPacket
    {
        public DoClientReconnect()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}