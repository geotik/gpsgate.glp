using System;
using System.Collections.Generic;
using System.Text;
using Franson.Reflection;
using GpsGate.Dispatch.Command;

using Franson.Message;

using System.Threading;

namespace GpsGate.GLP
{
    [Loadable(Installable = true, Description = "_SendStop for GLP.")]
    public class GLPSendStopCmdExe : SendStopCmdExe
    {
        public override string Namespace
        {
            get { return "GLP"; }
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
                outgoingMessage = ".ttp " + JobID + "," + Latitude.ToString("0.0000").Replace(',', '.') + "," + Longitude.ToString("0.0000").Replace(',', '.') + ",\"" + Text + "\"\r\n";

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
