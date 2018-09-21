using System;
using System.Collections.Generic;
using xServer.Core.Networking;
using System.Linq;
using System.Text;

namespace xServer.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoExecuteWMI : IPacket
    {
        public string WMINameSpace { get; set;}
        public string command { get; set; }
        public string target { get; set; }

        public DoExecuteWMI()
        {

        }
        public DoExecuteWMI(string WMINamespace, string command, string target, string[] properties)
        {

        }


        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
