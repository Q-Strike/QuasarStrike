using System;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ClientPackets
{
    [Serializable]
    public class DoShellExecuteResponse : IPacket
    {
        public string Output { get; set; }

        public bool IsError { get; private set; }

        public bool Impersonate { get; set; }


        public DoShellExecuteResponse()
        {
        }

        public DoShellExecuteResponse(string output, bool impersonate = false, bool isError = false)
        {
            this.Output = output;
            this.IsError = isError;
            this.Impersonate = impersonate;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}