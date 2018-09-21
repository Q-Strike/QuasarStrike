using System;
using xClient.Core.Networking;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xClient.Core.Packets.ClientPackets
{
    [Serializable]
    public class DoWMIExecResponse : IPacket
    {
        public bool execSuccess { get; set; }
        public string message { get; set; }

        public DoWMIExecResponse()
        {

        }
        public DoWMIExecResponse(string message, bool execSuccess)
        {
            this.execSuccess = execSuccess;
            this.message = message;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
