using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules.Sensors
{
    class InfraredSensorModule : GenericSensorModule
    {
        const int IRNum = 8;

        public InfraredSensorModule()
        {
            for (int i = 1; i <= IRNum; i++)
                SetPropertyValue("IR1", 0.0);
            
            SetPropertyValue("IRNum", IRNum);
        }

        public override string GetModuleName()
        {
            return "InfraredSensorModule";
        }
    }
}
