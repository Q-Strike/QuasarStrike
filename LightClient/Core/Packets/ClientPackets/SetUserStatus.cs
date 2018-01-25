using System;
using xLightClient.Core.Networking;
using xLightClient.Enums;

namespace xLightClient.Core.Packets.ClientPackets
{
    [Serializable]
    public class SetUserStatus : IPacket
    {
        public UserStatus Message { get; set; }

        public SetUserStatus()
        {
        }

        public SetUserStatus(UserStatus message)
        {
            Message = message;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}