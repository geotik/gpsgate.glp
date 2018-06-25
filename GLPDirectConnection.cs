using GpsGate.Online.Net;
using GpsGate.Online.Net.Direct;
using System;

namespace GpsGate.GLP
{
    public class GLPDirectConnection : DirectConnection
    {
        public override string ClientAddress
		{
			get
			{
				return (base.MasterConnection != null && base.MasterConnection != this) ? base.MasterConnection.ClientAddress : base.ClientAddress;
			}
		}
        protected override Protocol CreateProtocol()
        {
            return new GLPProtocol(this);
        }
        protected override void DoProtocolToDevice(byte[] arrData)
        {
            if (base.MasterConnection == null)
            {
                throw new InvalidOperationException("Cannot send data. No master connection tunnel available.");
            }
            base.MasterConnection.Protocol.TunnelToDevice(arrData);
        }
    }
}
