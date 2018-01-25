using System;
using xLightClient.Core.Networking;
using xLightClient.Core.Packets;

namespace xLightClient.Core.ReverseProxy.Packets
{
    [Serializable]
    public class ReverseProxyDisconnect : IPacket
    {
        public int ConnectionId { get; set; }

        public ReverseProxyDisconnect(int connectionId)
        {
            this.ConnectionId = connectionId;
        }

        public ReverseProxyDisconnect()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
