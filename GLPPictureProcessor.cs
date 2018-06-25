using Franson.Geo;
using GpsGate.Camera;
using GpsGate.Online.Net;
using GpsGate.Tracks;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GpsGate.GLP
{
    public class GLPPictureProcessor
    {
        public void Process(GLPBinaryPictureData picturePackage, NmeaConnection conn)
        {
            if (picturePackage == null)
            {
                throw new ArgumentNullException("Picture package is null.");
            }
            if (conn == null)
            {
                throw new ArgumentNullException("Connection is null.");
            }
            ISavePicture savePicture = new SavePicture();
            string strPictureName = picturePackage.RTC.ToString("s", CultureInfo.InvariantCulture);
            int iD = conn.Device.ID;
            if (picturePackage.PackageIndex == 1)
            {
                long? lTrackDataID = null;
                TrackPoint trackPoint = this.m_ResolvePosition(conn.Device.DeviceOwnerID, picturePackage.RTC);
                if (trackPoint != null)
                {
                    lTrackDataID = new long?(trackPoint.ID);
                }
                savePicture.NewPicture(picturePackage.RTC.ToString("s", CultureInfo.InvariantCulture), iD, picturePackage.RTC, lTrackDataID, (int)picturePackage.TotalPackages, 0, null);
            }
            savePicture.SavePictureData(strPictureName, iD, (int)picturePackage.PackageIndex, picturePackage.PictureData);
        }

        private TrackPoint m_ResolvePosition(int iOwnerID, DateTime dt)
        {
            TrackPoint trackPoint = null;
            TrackInfoReader trackInfoReader = new TrackInfoReader();
            List<TrackInfoBag> list = new List<TrackInfoBag>(trackInfoReader.GetTrackInfoByUser(iOwnerID, dt.AddMinutes(-2.0), dt.AddMinutes(2.0)));
            foreach (TrackInfoBag current in list)
            {
                TrackDataReader trackDataReader = new TrackDataReader();
                foreach (TrackPoint current2 in trackDataReader.GetTrackDataByTrackInfoId(current.ID, dt.AddMinutes(-2.0), dt.AddMinutes(2.0), true))
                {
                    if (trackPoint == null)
                    {
                        trackPoint = current2;
                    }
                    else if (Math.Abs((dt - current2.UTC).TotalSeconds) < Math.Abs((dt - trackPoint.UTC).TotalSeconds))
                    {
                        trackPoint = current2;
                    }
                }
            }
            return trackPoint;
        }
    }
}
