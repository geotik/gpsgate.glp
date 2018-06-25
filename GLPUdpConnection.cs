using GpsGate.Online.Net;
using GpsGate.Online.Net.Udp;
using System;
using System.Globalization;

namespace GpsGate.GLP
{
    public class GLPUdpConnection : UdpConnection
    {
        public GLPUdpConnection(UdpClientWrapper wrapper)
            : base(wrapper)
        {
        }

        protected override Protocol CreateProtocol()
        {
            return new GLPProtocol(this);
        }

        protected override void DoProtocolToDevice(byte[] data)
        {
            byte[] newData = new byte[data.Length + 2];
            data.CopyTo(newData, 0);
            newData[data.Length] = 0x0D;
            newData[data.Length + 1] = 0x0A;
            base.DoProtocolToDevice(newData);
        }

    }
}
