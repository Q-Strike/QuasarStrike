﻿using System;
using xLightClient.Core.Networking;

namespace xLightClient.Core.Packets.ServerPackets
{
    [Serializable]
    public class DoKeyboardEvent : IPacket
    {
        public byte Key { get; set; }

        public bool KeyDown { get; set; }

        public DoKeyboardEvent()
        {
        }

        public DoKeyboardEvent(byte key, bool keyDown)
        {
            this.Key = key;
            this.KeyDown = keyDown;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}