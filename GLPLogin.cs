using System;
using System.Collections.Generic;
using System.Text;

namespace GpsGate.GLP
{
    public class GLPLogin : GLPBase
    {
        public GLPLogin(byte[] arrDeviceID, byte[] arrData) :
            base(arrDeviceID, arrData)
        {
        }

        protected override void Parse(byte[] arrData)
        {

        }
    }
}
