﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules.Sensors
{
    class UltrasonicSensorModule : GenericSensorModule
    {
        public UltrasonicSensorModule()
        {

        }
        
        public override string GetModuleName()
        {
            return "UltrasonicSensorModule";
        }
    }
}
