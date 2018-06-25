using System;
using System.Collections.Generic;
using System.Text;
using Franson.Reflection;
using GpsGate.Online.Net.Tcp;
using NLog;
using System.Net.Sockets;

namespace GpsGate.GLP
{
    [Loadable(Installable = true, Description = "GLP Protocol over TCP/IP")]
    public class GLPTcpListener : TcpNmeaListener
    {
        private Logger m_nlog = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns "GLP"
        /// </summary>
        public override string ProtocolID
        {
            get { return "GLP"; }
        }

        /// <summary>
        /// Called when listener type is installed. Enable by default.
        /// Sets default server port to 50090
        /// </summary>
        /// <param name="typeInfo"></param>
        public override void Install(LoadableType typeInfo)
        {
            ServerPort = 50091;
            Enabled = true;

            base.Install(typeInfo);
        }

        /// <summary>
        /// Called when listener is started.
        /// </summary>
        protected override void OnStart()
        {
            GLPProtocol proto = new GLPProtocol(null);
            proto.InstallProtocol();

            base.OnStart();
        }


        /// <summary>
        /// When the listener is stopped:
        /// * Disconnect and stop handling all connections to slave devices.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <summary>
        /// Returns a <see cref="GLPTcpConnection"/> object.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected override TcpConnection CreateTcpConnection(Socket socket)
        {
            return new GLPTcpConnection(socket);
        }
    }
}
