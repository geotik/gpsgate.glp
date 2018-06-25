using Franson.Message;
using Franson.Reflection;
using GpsGate.Online;
using GpsGate.Online.Command.Outgoing;
using NLog;
using System;
using System.Text;

namespace GpsGate.GLP
{
    [Loadable(Installable = true, Description = "GLP take picture command.")]
    public class GLPTakePictureCmdExe : ConfigCmd
    {
        private static Logger m_nlog = LogManager.GetCurrentClassLogger();

        public override bool RequiresAck
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return "_TakePicture";
            }
        }

        public override string Namespace
        {
            get
            {
                return "GLP";
            }
        }

        protected override void Execute()
        {
            if (!CameraPluginServices.CameraInstalled)
            {
                throw new QueueFatalError("Camera plugin is not installed");
            }
            base.UpdateProgress(1, 3, "Sending request", null);
            this.ExecuteStep(null);
        }

        public override bool PendingCommit(object oDeviceData, string strCustomState)
        {
            return this.ExecuteStep(oDeviceData);
        }

        protected bool ExecuteStep(object oDeviceData)
        {
            bool result = false;
            if (base.StepCurrent == 1)
            {

                byte[] arrToSend = UTF8Encoding.UTF8.GetBytes(".takepic");

                try
                {
                    SendToDevice(arrToSend, "tcp", true);
                    base.UpdateProgress(2, 3, "Request sent", null);
                }
                catch (GateCmdException ex)
                {
                    if (ex.Message == null || !ex.Message.StartsWith("Could not find transport"))
                    {
                        throw;
                    }
                    base.UpdateProgress(1, 3, "Waiting for the device to connect", null);
                }
            }
            else if (base.StepCurrent == 2)
            {
                GLPBase report = oDeviceData as GLPBase;
                if (report.CameraStatus == 2 || report.CameraStatus == 3)
                {
                    base.UpdateProgress(3, 3, "Device will send the picture soon...", null);
                    result = true;
                }
                else if (report.CameraStatus == 1)
                {
                    base.UpdateProgress(3, 3, "Picture taking failed", null);
                    result = true;
                }
            }
            return result;
        }
    }
}
