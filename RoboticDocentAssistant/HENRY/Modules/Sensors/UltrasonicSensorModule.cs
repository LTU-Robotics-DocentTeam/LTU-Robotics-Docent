using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules.Sensors
{
    class UltrasonicSensorModule : GenericSensorModule
    {
        const int UltraSNum = 8;

        public UltrasonicSensorModule()
        {
            for (int i = 0; i <= UltraSNum; i++)
                SetPropertyValue("UltraS" + i.ToString(), 0.0);

            SetPropertyValue("UltraSNum", UltraSNum);
        }
        
        public override string GetModuleName()
        {
            return "UltrasonicSensorModule";
        }
    }
}
