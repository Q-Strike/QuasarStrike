using System;
using System.Collections.Generic;
using xServer.Core.Networking;
using System.Linq;
using System.Text;

namespace xServer.Core.Packets.ClientPackets
{
    [Serializable]
    public class DoSMBExecResponse : IPacket
    {
        public bool execSuccess { get; set; }
        public string message { get; set; }

        public DoSMBExecResponse()
        {

        }
        public DoSMBExecResponse(string message, bool execSuccess)
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
