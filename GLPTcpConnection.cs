using System;
using System.Collections.Generic;
using System.Text;
using GpsGate.Online.Net.Tcp;
using NLog;
using GpsGate.Online.Net;
using System.Net.Sockets;

namespace GpsGate.GLP
{
    /// <summary>
    /// Handles TCP/IP connection to a GLP device.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public class GLPTcpConnection : TcpConnection
    {
        #region Private variables

        private static Logger m_nlog = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a GLP device connection from a socket.
        /// </summary>
        /// <param name="socket"></param>
        public GLPTcpConnection(Socket socket)
            : base(socket)
        {
        }

        #endregion

        #region CreateProtocol

        /// <summary>
        /// Returns GLP protocol object. <see cref="GLPProtocol"/>
        /// </summary>
        /// <returns></returns>
        protected override Protocol CreateProtocol()
        {
            return new GLPProtocol(this);
        }

        #endregion

        
        #region DoProtocolToDevice

        protected override void DoProtocolToDevice(byte[] data)
        {
            byte[] newData = new byte[data.Length + 2];
            data.CopyTo(newData, 0);
            newData[data.Length] = 0x0D;
            newData[data.Length + 1] = 0x0A;
            base.DoProtocolToDevice(newData);
        }

        #endregion
         

    }
}
