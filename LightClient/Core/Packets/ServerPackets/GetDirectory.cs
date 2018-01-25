using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetDirectory : IPacket
    {
        public string RemotePath { get; set; }

        public GetDirectory()
        {
        }

        public GetDirectory(string remotepath)
        {
            this.RemotePath = remotepath;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}