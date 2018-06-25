using System;
using System.Collections.Generic;
using System.Text;

using GpsGate.Online.Net;
using GpsGate.Online.Message;
using GpsGate.Online;
using GpsGate.Online.Directory;
using Franson.Message;

using GpsGate.Online.Net.Direct;
using Franson.Nmea;
using GpsGate.Client;
using Franson.Nmea.Command;

using GpsGate.Dispatch;
using GpsGate.Dispatch.Command;

namespace GpsGate.GLP
{
    /// <summary>
    /// Base class for GLP protocol.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public class GLPProtocol : Protocol
    {
        #region Private variables

        private static NLog.Logger m_nlog = NLog.LogManager.GetCurrentClassLogger();
        private GLPParser m_parser = new GLPParser();

        #endregion

        #region Constructors

        /// <summary>
        /// Create GLP protocol object
        /// </summary>
        /// <param name="conn"></param>
        public GLPProtocol(NmeaConnection conn)
            : base(conn)
        {
        }
        #endregion

        #region Config

        /// <summary>
        /// Returns "GLP"
        /// </summary>
        public override string StringID
        {
            get { return "GLP"; }
        }

        #endregion

        #region SMS from device

        /// <summary>
        /// SMS from device.
        /// </summary>
        /// <param name="msg"></param>
        public override void TranslateFromDevice(ProviderMessage msg)
        {
            NmeaConnection.Login("MSISDN", msg.ClientAddress, null);

            ProcessPendingCommand(msg);

            // No framework processing of SMS implemented yet.
        }

        #endregion

        #region GPRS from device

        /// <summary>
        /// Data from device. TCP 
        /// </summary>
        /// <param name="arrData"></param>
        /// <param name="iStart"></param>
        /// <param name="iLength"></param>
        public override void TranslateFromDevice(byte[] arrData, int iStart, int iLength)
        {
            m_nlog.Info(GLPParser.LogBinary("GLP", arrData));

            m_parser.Write(arrData);

            GLPBase report = null;
            bool sendAck = false;

            while ((report = m_parser.NextReport()) != null)
            {
                if (report.sendAck)
                    sendAck = true;

                if (!report.parseError)
                {
                    if (report.loginPacket == false)
                    {
                        // Login
                        if (NmeaConnection.Session == null)
                        {
                            try
                            {
                                NmeaConnection.Login("IMEI", report.DeviceID, null);
                            }
                            catch (Franson.Directory.AuthenticationException)
                            {
                                throw;
                            }
                        }


                        // Pending outgoing commands.
                        ProcessPendingCommand(report);

                        if (report.CameraStatus == 2)
                        {
                            report.CameraStatus = 0;
                            NmeaConnection.ProtocolToDevice("ACK1");
                        }

                        if (report.CameraStatus == 3) //pobieranie zdjec
                        {
                            report.CameraStatus = 0;
                            GLPPictureProcessor glpPictureProcessor = CameraPluginServices.TryCreatePictureProcessor();
                            if (glpPictureProcessor != null)
                            {
                                try
                                {
                                    glpPictureProcessor.Process(report as GLPBinaryPictureData, this.NmeaConnection);
                                    NmeaConnection.ProtocolToDevice("ACK1");
                                }
                                catch
                                {
                                    NmeaConnection.ProtocolToDevice("ACK2");    //przerywamy z ponowieniem
                                }

                            }
                            else
                            {
                                NmeaConnection.ProtocolToDevice("ACK3"); //mnie ma w ogole zainstalowanej kamery wiec przerywamy calkiem
                            }
                        }
                        else if (report.TerminalData != null)
                        {
                            FRCMD fRCMD = null;
                            string text = UTF8Encoding.UTF8.GetString(report.TerminalData);
                            if (text == "GLP_UNDELIVERED\0")
                            {
                                text = Franson.Directory.Session.CurrentSession.Locale.Lang["GLP"].Server["GLP_UNDELIVERED"];
                            }
                            if (text == "GLP_DELIVERED\0")
                            {
                                text = Franson.Directory.Session.CurrentSession.Locale.Lang["GLP"].Server["GLP_DELIVERED"];
                            }


                            if (report.JobStatus == 100)
                            {
                                fRCMD = new ChangeAssignedWorkerStateCmdBuilder(CommandDirection.DeviceToGpsGate, report.JobID, NmeaConnection.Device.DeviceOwnerID, AssignedWorkerState.Active);
                            }
                            else if (report.JobStatus == 101)
                            {
                                fRCMD = new ChangeAssignedWorkerStateCmdBuilder(CommandDirection.DeviceToGpsGate, report.JobID, NmeaConnection.Device.DeviceOwnerID, AssignedWorkerState.Completed);
                            }
                            else if (report.JobStatus == 102)
                            {
                                fRCMD = new ChangeAssignedWorkerStateCmdBuilder(CommandDirection.DeviceToGpsGate, report.JobID, NmeaConnection.Device.DeviceOwnerID, AssignedWorkerState.Assigned);
                            }
                            else if (report.JobStatus == 103)
                            {
                                fRCMD = new ChangeAssignedWorkerStateCmdBuilder(CommandDirection.DeviceToGpsGate, report.JobID, NmeaConnection.Device.DeviceOwnerID, AssignedWorkerState.Assigned);
                            }
                            else if (report.JobStatus == 104)
                            {
                                fRCMD = new DeleteAssignedWorkerCmdBuilder(CommandDirection.DeviceToGpsGate, report.JobID, NmeaConnection.Device.DeviceOwnerID);
                            }
                            else
                                fRCMD = new FRCMD(null, "_ReceiveChatText", new string[] { text });
                            //serwer odbiera ale nie wysyła 
                            //GpsGateClientCmd client = new GpsGateClientCmd("localhost", 30175);
                            //client.Connect("IMEI", report.DeviceID, null);
                            //client.CmdToServer(cmd);

                            if (fRCMD != null)
                            {
                                GpsGateClientDirect client = new GpsGateClientDirect();
                                client.Connect(NmeaConnection.Device, this.NmeaConnection);
                                client.CmdToServer(fRCMD);
                                client.Disconnect();
                            }
                        }
                        else
                        {
                            // Report to GpsGate Framework.
                            ToGpsGate(report.TrackPoint, report.Status);
                        }

                    }
                    else
                    {
                        // Login
                        if (NmeaConnection.Session == null)
                            NmeaConnection.Login("imei", report.DeviceID, null);

                        // to keep connection alive
                        ToGpsGate(null, null);
                    }
                }
            }

            if (sendAck)
                NmeaConnection.ProtocolToDevice("ACK1");

        }


        #endregion

        #region Tunnel to device


        public override void TunnelToDevice(byte[] arrData)
        {
            NmeaConnection.ProtocolToDevice(arrData);
        }

        #endregion
    }
}
