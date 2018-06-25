using System;
using System.Collections.Generic;
using System.Text;

namespace GpsGate.GLP
{
    public class GLPTerminal : GLPBase
    {
        private static NLog.Logger m_nlog = NLog.LogManager.GetCurrentClassLogger();

        public GLPTerminal(byte[] arrDeviceID, byte[] arrData) :
            base(arrDeviceID, arrData)
        {
        }

        protected override void Parse(byte[] arrData)
        {
            try
            {
                parseError = false;

                int iLength = 0;

                // Info byte
                int iInfo = arrData[0];
                switch (iInfo)
                {
                    case 32:    //terminal state   
                        iLength += 1;
                        TerminalStatus = Convert.ToInt32((SByte)arrData[iLength]);
                        iLength += 1;
                        //if (arrData[1] == 0)
                        //    TerminalData = Encoding.UTF8.GetBytes("Terminal odłączony");
                        //if (arrData[1] == 1)
                        //    TerminalData = Encoding.UTF8.GetBytes("Terminal podłączony");
                        break;
                    case 33:    //message
                        iLength++;

                        UInt64 iMessageID = BitConverter.ToUInt64(arrData, iLength);
                        iLength += 8;

                        UInt16 iMessageSize = BitConverter.ToUInt16(arrData, iLength);
                        iLength += 2;

                        TerminalData = new byte[iMessageSize];
                        Array.Copy(arrData, iLength, TerminalData, 0, iMessageSize);
                        iLength += iMessageSize;

                        if (TerminalData[0] == (byte)'S' && TerminalData[1] == (byte)'T' && TerminalData[2] == (byte)'O' && TerminalData[3] == (byte)'P' && TerminalData[4] == (byte)':')
                        {
                            byte[] TerminalTemp = new byte[10];
                            int dwukropek = 4, rownosc = 0;
                            rownosc = Array.IndexOf(TerminalData, (byte)'=');
                            if (rownosc > 0)
                            {
                                Array.Copy(TerminalData, dwukropek + 1, TerminalTemp, 0, rownosc - dwukropek - 1);
                                int.TryParse(Encoding.ASCII.GetString(TerminalData, dwukropek + 1, rownosc - dwukropek - 1), out JobID);
                                int.TryParse(Encoding.ASCII.GetString(TerminalData, rownosc + 1, 3), out JobStatus);

                            }
                        }
                        break;
                    case 34:    //message received acknowledgement
                        iLength += 13;
                        break;
                    case 40:    //destination accepted
                        iLength++;
                        UInt64 iStopID = BitConverter.ToUInt64(arrData, iLength);
                        iLength += 8;
                        UInt32 iUnixTime = BitConverter.ToUInt32(arrData, iLength);
                        iLength += 4;
                        UInt32 iDistance = BitConverter.ToUInt32(arrData, iLength);
                        iLength += 4;
                        TerminalData = Encoding.UTF8.GetBytes("Cel " + iStopID + " zaakceptowany o " + iUnixTime + ". Odległość do celu: " + iDistance);
                        break;
                    case 41:    //destination point reached
                        iLength++;

                        iStopID = BitConverter.ToUInt64(arrData, iLength);
                        iLength += 8;
                        iUnixTime = BitConverter.ToUInt32(arrData, iLength);
                        iLength += 4;
                        TerminalData = Encoding.UTF8.GetBytes("Cel " + iStopID + " osiągnięty o " + iUnixTime);
                        break;
                    default:
                        break;
                }

                SetLength(iLength);
            }
            catch (Exception ex)
            {
                parseError = true;
                m_nlog.ErrorException(string.Format("Could not parse {0}", GLPParser.LogBinary(null, arrData)), ex);
                //throw new FormatException(string.Format("Could not parse {0}", GLPParser.LogBinary(null, arrData)), ex);
            }
        }
    }
}
