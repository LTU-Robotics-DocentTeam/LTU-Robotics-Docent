using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules.Sensors
{
    class InfraredSensorModule : GenericSensorModule
    {
        public InfraredSensorModule()
        {
            SetPropertyValue("IR1", 0);
            SetPropertyValue("IR2", 0);
            SetPropertyValue("IR3", 0);
            SetPropertyValue("IR4", 0);
        }

        public override string GetModuleName()
        {
            return "InfraredSensorModule";
        }
    }
}
