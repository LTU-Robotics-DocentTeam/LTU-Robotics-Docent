using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules.Sensors
{
    class ImpactSensorModule : GenericSensorModule
    {
        const int ImpactNum = 4;
        
        public ImpactSensorModule()
        {
            for (int i = 1; i <= ImpactNum; i++)
                SetPropertyValue("Impact" + i.ToString(), false);

            SetPropertyValue("ImpactNum", ImpactNum);
        }
        
        public override string GetModuleName()
        {
            return "ImpactSensorModule";
        }
    }
}
