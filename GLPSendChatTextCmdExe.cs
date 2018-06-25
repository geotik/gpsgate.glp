using Franson.Reflection;
using GpsGate.Chat.Command;
using NLog;
using System.Text;
using GpsGate.Chat;

using System;
using System.Data;
using System.Configuration;

using Franson.Message;

using System.Threading;

namespace GpsGate.GLP
{
    [Loadable(Installable = true, Description = "_SendChatText for GLP devices.")]
    public class GLPSendChatTextCmdExe : SendChatTextCmdExe
    {
        private static Logger m_nlog = LogManager.GetCurrentClassLogger();
        public override string Namespace
        {
            get
            {
                return "GLP";
            }
        }

        public override void Commit()
        {
            m_Commit(null, null);
            base.Commit();
        }

        /// <summary>
        /// true for multi step commands.
        /// </summary>
        public override bool RequiresAck
        {
            get
            {
                // Return true if this is a "multi-step" command. 
                // E.g. a command that requires an ACK from the device to complete.
                return true;
            }
        }
        public override QueueExecuteMode QueueExecuteMode
        {
            get
            {
                return QueueExecuteMode.ForceAsync;
            }
        }

        private bool m_Commit(object oDeviceData, string strCustomState)
        {
            bool result = false;

            GLPBase report = oDeviceData as GLPBase;

            string outgoingMessage = string.Empty;

            //to nie odpowiedz wiec, sprawdzamy połaczenie
            if (report == null)
            {
                byte[] arrToSend = UTF8Encoding.UTF8.GetBytes(".ttc\r\n");

                try
                {
                    SendToDevice(arrToSend, "tcp", true);
                    UpdateProgress(2, 3, "Connection OK", "");

                }
                catch
                {
                    UpdateProgress(1, 3, "Waiting for connection...", "");
                }
            }
            else if (report.TerminalStatus != 3) //poleczenie ok brak garmina
            {
                UpdateProgress(2, 3, "Connection OK but terminal not connected", "");
            }
            else if (report.TerminalStatus == 3)    //polaczenie i garmin ok
            {
                outgoingMessage = ".tt " + ChatTextID + ",\"" + ChatText + "\"";

                if (CannedResponseList.Count > 0)
                {
                    int newFlag = 1;

                    ChatReader reader = new ChatReader();
                    foreach (ChatResponseBag response in reader.GetAllResponses())
                    {
                        uint iResponseID = (uint)response.ID;

                        if (CannedResponseList.Contains(iResponseID))
                        {
                            outgoingMessage += ",\"" + response.ResponseText + "\"";
                        }

                        newFlag = 0;
                    }


                    if (newFlag == 1)
                    {
                        uint[] CannedResponseID = new uint[CannedResponseList.Count];
                        for (int i = 0; i < CannedResponseList.Count; i++)
                        {
                            CannedResponseID[i] = CannedResponseList[i];
                        }

                        string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

                        System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection();
                        conn.ConnectionString = ConnStr;
                        try
                        {
                            conn.Open();

                            for (int i = 0; i < CannedResponseList.Count; i++)
                            {
                                System.Data.Odbc.OdbcCommand cmd = new System.Data.Odbc.OdbcCommand(
                                @"SELECT body FROM chat_profile_message WHERE chat_profile_message_id = " + CannedResponseID[i] + ";"
                                , conn);

                                outgoingMessage += ",\"" + cmd.ExecuteScalar().ToString() + "\"";
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }

                outgoingMessage += "\r\n";

                byte[] arrToSend = UTF8Encoding.UTF8.GetBytes(outgoingMessage);

                try
                {
                    SendToDevice(arrToSend, "tcp", true);
                    UpdateProgress(3, 3, "Completed", "");
                    Thread.Sleep(2000);
                    result = true;
                }
                catch
                {
                    UpdateProgress(1, 3, "Waiting for connection...", "");
                }
            }


            return result;

        }

        public override bool PendingCommit(object oDeviceData, string strCustomState)
        {
            return this.m_Commit(oDeviceData, strCustomState);
        }

    }
}
