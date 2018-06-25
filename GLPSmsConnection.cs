using System;
using System.Collections.Generic;
using System.Text;
using GpsGate.Online.Net.Sms;
using Franson.Message;

namespace GpsGate.GLP
{
    /// <summary>
    /// Handles incoming and outgoing SMS to GLP trackers.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public class GLPSmsConnection : SmsConnection
    {
        /// <summary>
        /// Create incoming SMS connection for GLP.
        /// </summary>
        /// <param name="numPhone"></param>
        public GLPSmsConnection(MSISDN numPhone)
            : base(numPhone)
        {
        }

        /// <summary>
        /// Returns <see cref="GLPProtocol"/> object.
        /// </summary>
        /// <returns></returns>
        protected override GpsGate.Online.Net.Protocol CreateProtocol()
        {
            return new GLPProtocol(this);
        }
    }
}
