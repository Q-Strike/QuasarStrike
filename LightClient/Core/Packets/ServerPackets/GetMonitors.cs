using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetMonitors : IPacket
    {
        public GetMonitors()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}