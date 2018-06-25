using System;
using System.Collections.Generic;
using System.Text;

namespace GpsGate.GLP
{
    public class GLPBinaryPictureData : GLPBase
    {
        private static NLog.Logger m_nlog = NLog.LogManager.GetCurrentClassLogger();

        private DateTime m_dtRTC;

		private int m_bPackageIndex;

		private int m_bTotalPackages;

		private int m_iPictureDataSize;

		private byte[] m_arrPictureData;

        public GLPBinaryPictureData(byte[] arrDeviceID, byte[] arrData) :
            base(arrDeviceID, arrData)
        {
        }

		public DateTime RTC
		{
			get
			{
				return this.m_dtRTC;
			}
		}

		public int PackageIndex
		{
			get
			{
				return this.m_bPackageIndex;
			}
		}

		public int TotalPackages
		{
			get
			{
				return this.m_bTotalPackages;
			}
		}

		public int PackageSize
		{
			get
			{
				return this.m_iPictureDataSize;
			}
		}

		public byte[] PictureData
		{
			get
			{
				return this.m_arrPictureData;
			}
		}

        protected override void Parse(byte[] arrData)
		{
			if (arrData == null)
			{
				throw new ArgumentNullException("arrData");
			}
            try
            {
                parseError = false;

                int iLength = 0;

                // Info byte
                int iInfo = arrData[0];
                switch (iInfo)
                {
                    case 1:
                        iLength += 1;
                        CameraStatus = 1;
                        break;
                    case 2:
                        iLength += 1;
                        CameraStatus = 2;
                        break;
                    case 3:
                        iLength += 1;
                        CameraStatus = 3;
                        
                        UInt32 iUnixTime = BitConverter.ToUInt32(arrData, iLength);
                        this.m_dtRTC = ConvertFromUnixTimestamp(iUnixTime);
                        iLength += 4;

                        this.m_bPackageIndex = BitConverter.ToInt16(arrData, iLength);
                        iLength += 2;
                        this.m_bTotalPackages = BitConverter.ToInt16(arrData, iLength);
                        iLength += 2;
                        this.m_iPictureDataSize = BitConverter.ToInt16(arrData, iLength);
                        iLength += 2;
                        if (arrData.Length - 13 != this.m_iPictureDataSize)
                        {
                            throw new InvalidOperationException("Incomplete GLP picture data packet.");
                        }
                        this.m_arrPictureData = new byte[this.m_iPictureDataSize];
                        Buffer.BlockCopy(arrData, 11, this.m_arrPictureData, 0, this.m_iPictureDataSize);
                        iLength += this.m_iPictureDataSize;

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
