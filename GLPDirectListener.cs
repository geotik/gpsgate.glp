using Franson.Reflection;
using GpsGate.Online.Net.Direct;
using System;

namespace GpsGate.GLP
{
    [Loadable(Installable = true, Description = "GLP Protocol over Direct")]
    public class GLPDirectListener : DirectListener
    {
        public override string ProtocolID
        {
            get
            {
                return "GLP";
            }
        }
        public override void Install(LoadableType typeInfo)
        {
            base.Enabled = true;
            base.Install(typeInfo);
        }
        public override void Upgrade(LoadableType typeInfo)
        {
            base.Enabled = true;
            base.Upgrade(typeInfo);
        }
        protected override void OnStart()
        {
            GLPProtocol glpProtocol = new GLPProtocol(null);
            glpProtocol.InstallProtocol();
            base.OnStart();
        }
        public override DirectConnection CreateDirectConnection()
        {
            return new GLPDirectConnection();
        }
    }
}
