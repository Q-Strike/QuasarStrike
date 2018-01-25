using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetProcesses : IPacket
    {
        public GetProcesses()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}