using Franson.Reflection;
using NLog;
using System;
using System.Reflection;

namespace GpsGate.GLP
{
    public class CameraPluginServices
    {
        private static Logger m_nlog;

        public static readonly bool CameraInstalled;

        static CameraPluginServices()
        {
            CameraPluginServices.m_nlog = LogManager.GetCurrentClassLogger();
            try
            {
                Assembly assembly = AssemblyLoader.GetAssemblyLoader().Load("GpsGate.Camera");
                CameraPluginServices.CameraInstalled = true;
            }
            catch (Exception ex)
            {
                CameraPluginServices.CameraInstalled = false;
                CameraPluginServices.m_nlog.Info("GLP camera feature disabled. Could not load the Camera plugin.");
                CameraPluginServices.m_nlog.InfoException(ex.GetType().FullName, ex);
            }
        }

        public static GLPPictureProcessor TryCreatePictureProcessor()
        {
            GLPPictureProcessor result;
            if (CameraPluginServices.CameraInstalled)
            {
                result = new GLPPictureProcessor();
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}
