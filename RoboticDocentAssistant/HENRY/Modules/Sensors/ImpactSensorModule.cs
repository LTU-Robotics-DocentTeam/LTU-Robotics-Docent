using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules.Sensors
{
    class ImpactSensorModule : GenericSensorModule
    {
        public ImpactSensorModule()
        {
            SetPropertyValue("ImpactFront",false);
            SetPropertyValue("ImpactBack", false);
            SetPropertyValue("ImpactRight", false);
            SetPropertyValue("ImpactLeft", false);
        }
        
        public override string GetModuleName()
        {
            return "ImpactSensorModule";
        }
    }
}
