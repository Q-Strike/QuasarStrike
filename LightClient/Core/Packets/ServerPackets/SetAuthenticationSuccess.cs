using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class SetAuthenticationSuccess : IPacket
    {
        public SetAuthenticationSuccess()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
