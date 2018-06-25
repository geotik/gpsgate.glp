using System;
using System.Collections.Generic;
using System.Text;

using Franson.Reflection;
using Franson.Message.Sms;

using GpsGate.Online.Command.Outgoing;
using GpsGate.Online.Command;
using GpsGate.Online.Net;
using GpsGate.Online;

namespace GpsGate.GLP
{
    /// <summary>
    /// Handles outgoing _GprsSettings command for GLP
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    [Loadable(Installable = true, Description = "GLP implementation of _GprsSettings. Sends GPRS settings to GLP devices.")]
    public class GLPGprsSettingsCmdExe : GprsSettingsCmdExe
    {
        /// <summary>
        /// Throws exception
        /// </summary>
        /// <exception cref="GateCmdException"></exception>
        protected override void Execute()
        {
            throw new GateCmdException("Use Template Commands.");
        }

        /// <summary>
        /// Returns false
        /// </summary>
        public override bool RequiresAck
        {
            get { return false; }
        }

        /// <summary>
        /// Returns "GLP"
        /// </summary>
        public override string Namespace
        {
            get { return "GLP"; }
        }
    }
}
