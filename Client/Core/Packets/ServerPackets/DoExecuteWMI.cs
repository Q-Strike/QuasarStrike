using System;
using System.Collections.Generic;
using xClient.Core.Networking;
using System.Linq;
using System.Text;

namespace xClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoExecuteWMI : IPacket
    {
        public string WMINameSpace { get; set;}
        public string command { get; set; }
        public string target { get; set; }
        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
