using System;
using System.Collections.Generic;
using System.Text;
using GpsGate.Online.Net.Sms;

namespace GpsGate.GLP
{
    /// <summary>
    /// GLP SMS listener.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    [Franson.Reflection.Loadable(Installable = true, Description = "GLP Protocol over SMS")]
    public class GLPSmsListener : SmsListener
    {
        /// <summary>
        /// Returns a <see cref="GLPSmsConnection"/> object for incoming data.
        /// </summary>
        /// <param name="msisdn"></param>
        /// <returns></returns>
        public override SmsConnection CreateSmsConnection(Franson.Message.MSISDN msisdn)
        {
            return new GLPSmsConnection(msisdn);
        }

        /// <summary>
        /// Installs GLP protocol and start s up listener.
        /// </summary>
        protected override void OnStart()
        {
            GLPProtocol proto = new GLPProtocol(null);
            proto.InstallProtocol();

            base.OnStart();
        }

        /// <summary>
        /// Adds default settings for GLP. Enable by default.
        /// </summary>
        /// <param name="typeInfo"></param>
        public override void Install(Franson.Reflection.LoadableType typeInfo)
        {
            // Install listener
            Enabled = true;

            base.Install(typeInfo);
        }

        /// <summary>
        /// Returns "GLP"
        /// </summary>
        public override string ProtocolID
        {
            get { return "GLP"; }
        }
    }
}
