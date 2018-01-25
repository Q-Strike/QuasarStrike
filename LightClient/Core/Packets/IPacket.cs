using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets
{
    public interface IPacket
    {
        void Execute(Client client);
    }
}