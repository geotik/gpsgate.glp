using System;
using System.Collections.Generic;
using System.Text;

using Franson.Message;
using Franson.Serialization;
using Franson.Reflection;

using GpsGate.Online.Directory;
using GpsGate.Online.Command;

namespace GpsGate.GLP
{
    /// <summary>
    /// GLP device definitions.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    [Serialize]
    [Loadable(Description = "GLP", Installable = true)]
    public class GLPDeviceDefinition : DeviceDefinition
    {
        /// <summary>
        /// Device name
        /// </summary>
        [Serialize]
        public override string Name
        {
            get { return "GLP"; }
        }

        /// <summary>
        /// Description.
        /// </summary>
        public override string Description
        {
            get { return "GLP"; }
        }

        /// <summary>
        /// DefaultGuid
        /// </summary>
        public override Guid DefaultGuid
        {
            get { return new System.Guid("3ACE2729-40A1-4755-9C36-CBFF9EBDE5E9"); }
        }

        /// <summary>
        /// Add common GLP message fields.
        /// </summary>
        /// <param name="deviceNamespace"></param>
        /// <param name="listFields"></param>
        /// <returns></returns>
        protected override void InstallMessageFields(MessageNamespace deviceNamespace, List<DeviceFieldProperty> listFields)
        {
            MessageField mf;

            mf = new MessageField("Ignition", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In1", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In2", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In3", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In4", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In5", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In6", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("In7", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("Out1", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out2", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out3", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out4", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out5", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out6", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out7", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Out8", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("Alarm0", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Alarm1", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Alarm2", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Alarm3", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("HarshAcceleration", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("HarshBraking", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("HarshCornering", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Accident", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));


            mf = new MessageField("SatelliteCount", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Fix", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("HDOP", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            //parametery 32 bitowe
            mf = new MessageField("Parameter2", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter3", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter4", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter5", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter6", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter7", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter8", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter9", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter10", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter11", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter12", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter13", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter14", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter15", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter16", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter17", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter18", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter19", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter20", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter21", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter22", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter23", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter24", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter25", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter26", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter27", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter28", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter29", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter30", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter31", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter32", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            //parametry 8 bitowe
            mf = new MessageField("Parameter64", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter65", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter66", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter67", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter68", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter69", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter70", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter71", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            //parametry 16 bitowe
            mf = new MessageField("Parameter128", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter129", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter130", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter131", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter132", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter133", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter134", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter135", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter136", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter137", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter138", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter139", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter140", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter141", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter142", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter143", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter144", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter145", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter146", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter147", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter148", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter149", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter150", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter151", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter152", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter153", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter154", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter155", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter156", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter157", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter158", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter159", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            //parametry 64 bitowe
            mf = new MessageField("Parameter194", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter195", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter196", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter197", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("Parameter240", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter241", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter242", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter243", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter244", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Parameter245", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("OdometerAcc", "", deviceNamespace, typeof(double), Franson.Unit.METER);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("Csq", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("FirmwareVersion", "", deviceNamespace, typeof(String), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));

            mf = new MessageField("ChatMessage", "Chat text", deviceNamespace, typeof(string), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, false));
            mf = new MessageField("JobAssignmentState", "Job assignment state", deviceNamespace, typeof(string), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, false));


            mf = new MessageField("SavedUnix", "", deviceNamespace, typeof(double), Franson.Unit.NONE);
            listFields.Add(new DeviceFieldProperty(mf, true));
            mf = new MessageField("Historic", "", deviceNamespace, typeof(bool), Franson.Unit.BOOLEAN);
            listFields.Add(new DeviceFieldProperty(mf, true));
        }

        /// <summary>
        /// Add default mapping to GpsGate fields.
        /// </summary>
        /// <param name="inField"></param>
        /// <param name="defaultMapper"></param>
        /// <returns></returns>
        protected override MessageFieldDictionaryEntry? InstallDefaultMapperEntryFor(MessageField inField, MessageFieldDictionary defaultMapper)
        {
            MessageFieldDictionaryEntry? result = null;
            MessageFieldReader reader = MessageFieldReader.GetReader();
            MessageNamespace defaultNamespace = MessageNamespace.GetDefaultNamespace();
            string name = inField.Name;
            if (name != null)
            {
                if (!(name == "ChatMessage"))
                {
                    if (name == "JobAssignmentState")
                    {
                        MessageField? messageField = reader.FindMessageField("JobAssignmentState", defaultNamespace);
                        if (messageField.HasValue)
                        {
                            result = new MessageFieldDictionaryEntry?(defaultMapper.NewFieldEntry(inField, messageField.Value, false));
                        }
                    }
                }
                else
                {
                    MessageField? messageField = reader.FindMessageField("ChatMessage", defaultNamespace);
                    if (messageField.HasValue)
                    {
                        result = new MessageFieldDictionaryEntry?(defaultMapper.NewFieldEntry(inField, messageField.Value, false));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Register outgoing commands for device type.
        /// </summary>
        /// <param name="gateCommand"></param>
        /// <returns></returns>
        protected override DeviceDefinitionCommandProperty? RegisterCommad(GateCmd gateCommand)
        {
            DeviceDefinitionCommandProperty? retCmdProp = null;

            switch (gateCommand.Name)
            {
                case "_GprsSettings":
                    retCmdProp = new DeviceDefinitionCommandProperty(gateCommand.Name, new string[] { "sms" });
                    break;
                case "_SendChatText":
                    retCmdProp = new DeviceDefinitionCommandProperty(gateCommand.Name, new string[] { "tcp" });
                    break;
                case "_SendStop":
                    retCmdProp = new DeviceDefinitionCommandProperty(gateCommand.Name, new string[] { "tcp" });
                    break;
                case "_TakePicture":
                    retCmdProp = new DeviceDefinitionCommandProperty(gateCommand.Name, new string[] { "tcp" });
                    break;
            }

            return retCmdProp;
        }

        //public override void Install(LoadableType typeInfo)
        //{
        //    GLPDeviceDefinition.m_InstallMessageField();
        //    base.Install(typeInfo);
        //}
        //public override void Upgrade(LoadableType typeInfo)
        //{
        //    GLPDeviceDefinition.m_InstallMessageField();
        //    base.Upgrade(typeInfo);
        //}
        //private static void m_InstallMessageField()
        //{
        //    MessageNamespace defaultNamespace = MessageNamespace.GetDefaultNamespace();
        //    new MessageFieldWriter
        //    {
        //        new MessageField(0, defaultNamespace.ID, "ChatMessage", "Chat text", typeof(string), Franson.Unit.NONE, "ChatMessage")
        //    }.Update();
        //    new MessageFieldWriter
        //    {
        //        new MessageField(0, defaultNamespace.ID, "JobAssignmentState", "Job assignment state", typeof(string), Franson.Unit.NONE, "JobAssignmentState")
        //    }.Update();
        //}

        #region Configuration

        /// <summary>
        /// Returns "GLP"
        /// </summary>
        protected override string NamespaceName
        {
            get { return "GLP"; }
        }

        /// <summary>
        /// Returns "GLP"
        /// </summary>
        [Franson.Serialization.Serialize]
        public override string DeviceIdentifierLabel
        {
            get { return "GLP"; }
        }

        #endregion
    }
}
