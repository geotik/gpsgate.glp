using System;
using System.Collections.Generic;
using System.Text;

using Franson.Nmea;
using Franson.Geo;

namespace GpsGate.GLP
{
    /// <summary>
    /// Base class for all typed GLP sentences.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public abstract class GLPBase
    {
        #region Private variables

        private TrackPoint m_tp = null;
        private Dictionary<string, object> m_dicStatus = new Dictionary<string, object>();
        private string m_strDeviceID = null;
        private int m_iLength = 0;
        public bool loginPacket = false;
        public byte[] TerminalData;
        public int JobID=0, JobStatus=0, TerminalStatus=0, CameraStatus=0;
        public bool sendAck = false;
        public bool parseError = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Create from generic sentence
        /// </summary>
        /// <param name="arrDeviceID"></param>
        /// <param name="arrData"></param>
        protected GLPBase(byte[] arrDeviceID, byte[] arrData)
        {
            Parse(arrData);
            m_ParseDeviceID(arrDeviceID);
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// DeviceID
        /// </summary>
        public string DeviceID
        {
            get { return m_strDeviceID; }
        }

        /// <summary>
        /// Track point
        /// </summary>
        public TrackPoint TrackPoint
        {
            get { return m_tp; }
        }

        /// <summary>
        /// Ised by derived class to set track point
        /// </summary>
        /// <param name="tp"></param>
        protected void SetTrackPoint(TrackPoint tp)
        {
            m_tp = tp;
        }

        /// <summary>
        /// Status messages
        /// </summary>
        public Dictionary<string, object> Status
        {
            get { return m_dicStatus; }
        }

        /// <summary>
        /// Used by parser to add new status values.
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="oValue"></param>
        public void AddStatus(string strName, object oValue)
        {
            m_dicStatus[strName] = oValue;
        }

        /// <summary>
        /// Length in bytes of packet.
        /// </summary>
        public int Length
        {
            get { return m_iLength; }
        }

        /// <summary>
        /// Called by parser to set Length value. Default 0.
        /// </summary>
        /// <param name="iLength"></param>
        public void SetLength(int iLength)
        {
            m_iLength = iLength;
        }
        #endregion

        #region Public Methods
        public static DateTime ConvertFromUnixTimestamp(UInt32 timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static UInt32 ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date - origin;
            return (UInt32)Math.Floor(diff.TotalSeconds);
        }

        #endregion

        #region Parse

        /// <summary>
        /// Override to parse data
        /// </summary>
        /// <param name="arrData"></param>
        protected abstract void Parse(byte[] arrData);

        /// <summary>
        /// Parse Device ID
        /// </summary>
        /// <param name="arrDeviceID"></param>
        private void m_ParseDeviceID(byte[] arrDeviceID)
        {
            StringBuilder builder = new StringBuilder();
            //builder.Append("0x");

            for (int iIndex = 0; iIndex < arrDeviceID.Length; iIndex++)
            {
                builder.Append(string.Format("{0:X2}", arrDeviceID[iIndex]));
            }

            m_strDeviceID = builder.ToString();
        }

        #endregion
    }
}
