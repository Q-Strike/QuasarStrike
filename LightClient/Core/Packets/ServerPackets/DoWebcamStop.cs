using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoWebcamStop : IPacket
    {
        public DoWebcamStop()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
