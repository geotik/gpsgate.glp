using System;
using System.Collections.Generic;
using System.Text;
using Franson.Buffer;
using Franson.Geo;

namespace GpsGate.GLP
{
    /// <summary>
    /// Report
    /// </summary>
    public class GLPReport : GLPBase
    {
        private static NLog.Logger m_nlog = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Create report from array.
        /// </summary>
        /// <param name="arrDeviceID"></param>
        /// <param name="arrData"></param>
        public GLPReport(byte[] arrDeviceID, byte[] arrData)
            : base(arrDeviceID, arrData)
        {
        }

        /// <summary>
        /// Returns true if data contains a complete report packet.
        /// </summary>
        /// <param name="arrData"></param>
        /// <returns></returns>
        public static bool CanParse(byte[] arrData)
        {
            if (arrData.Length >= 30)
            {
                int parno = arrData[23];
                if (arrData.Length >= (26 + parno * 4))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Parse data to report.
        /// </summary>
        /// <param name="arrData"></param>
        protected override void Parse(byte[] arrData)
        {
            try
            {
                parseError = false;
                int iLength = 0;
                bool SpeedHRMode = false;
                
                int noOfParams = arrData[iLength++];

                DateTime dtUTC = DateTime.MinValue;
                Single Latitude = 0, Longitude = 0;
                Int16 Altitude = 0;
                UInt16 Heading = 0;
                double Speed = 0.0;
                bool bValid = false;

                AddStatus("SavedUnix", Convert.ToDouble(ConvertToUnixTimestamp(DateTime.UtcNow)));

                for (int i = 0; i < noOfParams; i++)
                {
                    Byte paramNo = arrData[iLength++];

                    //czas
                    if (paramNo == 0)
                    {
                        UInt32 iUnixTime = BitConverter.ToUInt32(arrData, iLength);
                        dtUTC = ConvertFromUnixTimestamp(iUnixTime);
                        iLength += 4;

                        if ( (DateTime.UtcNow-dtUTC).TotalSeconds >  1800)
                            AddStatus("Historic", true);
                        else
                            AddStatus("Historic", false);

                        continue;
                    }

                    //wejscia i wyjscia
                    if (paramNo == 1)
                    {
                        uint parVal = BitConverter.ToUInt32(arrData, iLength);

                        AddStatus("Ignition", (parVal & (1 << 0)) != 0);
                        AddStatus("In1", (parVal & (1 << 1)) != 0);
                        AddStatus("In2", (parVal & (1 << 2)) != 0);
                        AddStatus("In3", (parVal & (1 << 3)) != 0);
                        AddStatus("In4", (parVal & (1 << 4)) != 0);
                        AddStatus("In5", (parVal & (1 << 5)) != 0);
                        AddStatus("In6", (parVal & (1 << 6)) != 0);
                        AddStatus("In7", (parVal & (1 << 7)) != 0);

                        AddStatus("Out1", (parVal & (1 << 8)) != 0);
                        AddStatus("Out2", (parVal & (1 << 9)) != 0);
                        AddStatus("Out3", (parVal & (1 << 10)) != 0);
                        AddStatus("Out4", (parVal & (1 << 11)) != 0);
                        AddStatus("Out5", (parVal & (1 << 12)) != 0);
                        AddStatus("Out6", (parVal & (1 << 13)) != 0);
                        AddStatus("Out7", (parVal & (1 << 14)) != 0);
                        AddStatus("Out8", (parVal & (1 << 15)) != 0);

                        AddStatus("Alarm0", (parVal & (1 << 16)) != 0);
                        AddStatus("Alarm1", (parVal & (1 << 17)) != 0);
                        AddStatus("Alarm2", (parVal & (1 << 18)) != 0);
                        AddStatus("Alarm3", (parVal & (1 << 19)) != 0);

                        AddStatus("HarshAcceleration", (parVal & (1 << 20)) != 0);
                        AddStatus("HarshBraking", (parVal & (1 << 21)) != 0);
                        AddStatus("HarshCornering", (parVal & (1 << 22)) != 0);
                        AddStatus("Accident", (parVal & (1 << 23)) != 0);

                        SpeedHRMode = (parVal & (1 << 31)) != 0;

                        iLength += 3;

                        AddStatus("Csq", (double)(((byte)arrData[iLength++]) & 0x7F));

                        continue;
                    }

                    //pozycja
                    if (paramNo == 192)
                    {
                        Latitude = BitConverter.ToSingle(arrData, iLength);
                        iLength += 4;

                        Longitude = BitConverter.ToSingle(arrData, iLength);
                        iLength += 4;

                        continue;
                    }

                    //wysokosc, predkosc, hdop itp
                    if (paramNo == 193)
                    {
                        Altitude = BitConverter.ToInt16(arrData, iLength);
                        iLength += 2;

                        Speed = (double)BitConverter.ToUInt16(arrData, iLength);
                        iLength += 2;

                        Heading = BitConverter.ToUInt16(arrData, iLength);
                        iLength += 2;

                        Byte SU = arrData[iLength];
                        bool iFix = false;
                        if ((SU & 0x80) == 0x80)
                        {
                            iFix = true;
                            bValid = true;
                            SU -= 0x80;
                        }

                        iLength++;

                        double HDOP = arrData[iLength] / 10d;

                        iLength++;

                        AddStatus("HDOP", HDOP);
                        AddStatus("Fix", iFix);
                        AddStatus("SatelliteCount", (double)SU);

                        continue;
                    }

                    if (paramNo == 11)
                    {
                        int parVal = BitConverter.ToInt32(arrData, iLength);
                        AddStatus("OdometerAcc", Convert.ToDouble(parVal) * 1000);
                    }

                    if (paramNo >= 2 && paramNo <= 63)
                    {
                        int parVal = BitConverter.ToInt32(arrData, iLength);
                        iLength += 4;

                        AddStatus("Parameter" + paramNo, Convert.ToDouble(parVal));

                        continue;
                    }

                    if (paramNo == 70)
                    {
                        TerminalStatus = Convert.ToInt32((SByte)arrData[iLength]);
                    }

                    if (paramNo >= 64 && paramNo <= 127)
                    {
                        SByte parVal = (SByte)arrData[iLength];
                        iLength++;

                        AddStatus("Parameter" + paramNo, Convert.ToDouble(parVal));

                        continue;
                    }

                    if (paramNo >= 128 && paramNo <= 191)
                    {
                        Int16 parVal = BitConverter.ToInt16(arrData, iLength);
                        iLength += 2;

                        AddStatus("Parameter" + paramNo, Convert.ToDouble(parVal));

                        continue;
                    }

                    if (paramNo >= 194 && paramNo <= 239)
                    {
                        Int64 parVal = BitConverter.ToInt64(arrData, iLength);
                        iLength += 8;

                        AddStatus("Parameter" + paramNo, Convert.ToDouble(parVal));

                        continue;
                    }

                    if (paramNo >= 240 && paramNo <= 255)
                    {
                        uint parLen = arrData[iLength];
                        iLength++;

                        byte[] data = new byte[parLen];
                        Array.Copy(arrData, iLength, data, 0, parLen);
                        iLength += (int)parLen;

                        AddStatus("Parameter" + paramNo, UTF8Encoding.UTF8.GetString(data));

                        continue;
                    }

                }

                if (SpeedHRMode) Speed /= 100.0;

                if (Longitude < -180.0 || Longitude > 180 || Latitude < -90 || Latitude > 90)
                {
                    Longitude = 0;
                    Latitude = 0;
                    bValid = false;
                }

                SetTrackPoint(new TrackPoint(
                    new Position(Longitude, Latitude, Altitude),
                    new Velocity(Speed, Heading, SpeedUnits.Kph),
                    dtUTC, bValid));


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
