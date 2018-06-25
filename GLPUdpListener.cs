using Franson.Reflection;
using GpsGate.Online.Net;
using GpsGate.Online.Net.Udp;

namespace GpsGate.GLP
{
    [Loadable(Installable = true, Description = "GLP Protocol over UDP")]
    public class GLPUdpListener : UdpNmeaListener
    {

        public override string ProtocolID
        {
            get { return "GLP"; }
        }

        public override void Install(LoadableType typeInfo)
        {
            ServerPort = 50091;
            Enabled = true;

            base.Install(typeInfo);
        }

        protected override void OnStart()
        {
            GLPProtocol proto = new GLPProtocol(null);
            proto.InstallProtocol();

            base.OnStart();
        }

        protected override UdpConnection CreateUdpConnection(UdpClientWrapper wrapper)
        {
            return new GLPUdpConnection(wrapper);
        }

    }
}
