using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ClientPackets
{
    [Serializable]
    public class GetDrivesResponse : IPacket
    {
        public string[] DriveDisplayName { get; set; }

        public string[] RootDirectory { get; set; }

        public GetDrivesResponse()
        {
        }

        public GetDrivesResponse(string[] driveDisplayName, string[] rootDirectory)
        {
            this.DriveDisplayName = driveDisplayName;
            this.RootDirectory = rootDirectory;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}