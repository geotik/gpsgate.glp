using System;
using System.Collections.Generic;
using System.Text;
using Franson.Buffer;

namespace GpsGate.GLP
{
    /// <summary>
    /// Parse GLP data.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public class GLPParser
    {
        #region Private variables

        private Circular m_cirBuf = new Circular(2048);
        private int m_iPacketCount = 0;
        private int m_iPacketType = 0;
        private int m_iPacketsProcessed = 0;
        private int m_iPacketLength = 0;
        private byte[] m_arrDeviceID = null;
        private bool m_bHeaderFound = false;
        private string m_FirmwareVersion;

        #endregion

        #region Write()

        /// <summary>
        /// Write more data to be parsed.
        /// </summary>
        /// <param name="arrData"></param>
        public void Write(byte[] arrData)
        {
            m_cirBuf.Write(arrData);
        }

        /// <summary>
        /// Write section of array to parser.
        /// </summary>
        /// <param name="arrData"></param>
        /// <param name="iStart"></param>
        /// <param name="iLength"></param>
        public void Write(byte[] arrData, int iStart, int iLength)
        {
            m_cirBuf.Write(arrData, iStart, iLength);
        }


        #endregion

        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            ushort CRCFull = 0xFFFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);
                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }

            CRC[1] = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = (byte)(CRCFull & 0xFF);
        }

        #region NextReport

        /// <summary>
        /// Next report. Or null if there is none.
        /// </summary>
        /// <returns></returns>
        public GLPBase NextReport()
        {
            GLPBase iBase = null;

            bool sendAck = false;
           
            if (m_bHeaderFound == false && m_cirBuf.UsedLength >= 14)
            {
                // Looking for header.
                byte[] arrPackets = m_cirBuf.Peek(0, 14);

                m_arrDeviceID = new byte[8];
                // Device ID
                Array.Copy(arrPackets, m_arrDeviceID, 8);
                // Packet type
                m_iPacketType = arrPackets[8];
                // Packet count
                m_iPacketCount = arrPackets[9];
                // Firmware version
                BitParser bp = new BitParser(2, true, false);
                bp.WriteData(arrPackets, 10, 2);
                int build = bp.ReadIntX(6);
                int major = bp.ReadIntX(7);
                int minor = bp.ReadIntX(3);
                m_FirmwareVersion = minor + "." + major + "." + build;

                ushort ackAndLength = BitConverter.ToUInt16(arrPackets, 12);

                if ((ackAndLength & 0x8000) > 0)
                {
                    sendAck = true;
                }
                else
                {
                    sendAck = false;
                }

                m_iPacketLength = ackAndLength & (ushort)0x7FFF;

                if (m_cirBuf.UsedLength >= m_iPacketLength)
                {
                    byte[] CRC = new byte[2];
                    byte[] packet = m_cirBuf.Peek(0, m_iPacketLength);

                    GetCRC(packet, ref CRC);

                    if (CRC[0] == packet[packet.Length - 2] && CRC[1] == packet[packet.Length - 1])
                    {
                        m_bHeaderFound = true;
                        m_iPacketsProcessed = 0;
                        m_cirBuf.Seek(14);
                    }
                    else
                    {
                        m_bHeaderFound = false;
                        m_cirBuf.Clear();
                    }

                   
                }
            }

            if (m_bHeaderFound)
            {
                byte[] arrPackets = m_cirBuf.Peek(0, m_cirBuf.UsedLength);
                if (m_iPacketType == 0)
                {
                    iBase = new GLPLogin(m_arrDeviceID, arrPackets);
                    //iBase.AddStatus("FirmwareVersion", m_FirmwareVersion);
                    iBase.loginPacket = true;
                    iBase.sendAck = sendAck;
                }

                if (m_iPacketType == 1)
                {
                    iBase = new GLPReport(m_arrDeviceID, arrPackets);
                    iBase.AddStatus("FirmwareVersion", m_FirmwareVersion);
                    m_iPacketsProcessed++;

                    // Forward to next packet start.
                    m_cirBuf.Seek(iBase != null ? iBase.Length : 0);
                    iBase.sendAck = sendAck;
                }

                if (m_iPacketType == 0x20)
                {
                    iBase = new GLPTerminal(m_arrDeviceID, arrPackets);
                    m_cirBuf.Seek(iBase != null ? iBase.Length : 0);
                    iBase.sendAck = sendAck;
                }

                if (m_iPacketType == 0x30)
                {
                    iBase = new GLPBinaryPictureData(m_arrDeviceID, arrPackets);
                    m_cirBuf.Seek(iBase != null ? iBase.Length : 0);
                    iBase.sendAck = sendAck;
                }

                if (m_iPacketType >= 0xF0 && m_iPacketType <= 0xFF)
                {
                    iBase = new GLPLogin(m_arrDeviceID, arrPackets);   
                    m_cirBuf.Seek(m_iPacketLength - 14); //bo nie wiemy jaka jest dlugosc
                    m_bHeaderFound = false;
                    iBase.loginPacket = true;
                    iBase.sendAck = sendAck;
                    return iBase;
                }

                if (m_iPacketsProcessed >= m_iPacketCount || iBase.parseError)
                {
                    // Forward to next packet start (jump CRC).
                    if (iBase.parseError)
                    {
                        m_cirBuf.Clear();
                    }
                    else
                    {
                        m_cirBuf.Seek(2);
                    }
                    
                    m_bHeaderFound = false;
                }
            }

            return iBase;
        }

        #endregion

        /// <summary>
        /// Returns accsii hex string. Optionally adds label in front.
        /// </summary>
        /// <param name="strLabel"></param>
        /// <param name="arrData"></param>
        /// <returns></returns>
        public static string LogBinary(string strLabel, byte[] arrData)
        {
            // Log raw binary message.
            StringBuilder builder = new StringBuilder();
            if (strLabel != null)
            {
                builder.Append(strLabel);
                builder.Append(" ");
            }
            for (int iIndex = 0; iIndex < arrData.Length; iIndex++)
            {
                builder.AppendFormat("{0:X2},", arrData[iIndex]);
            }
            return builder.ToString();
        }
    }
}
