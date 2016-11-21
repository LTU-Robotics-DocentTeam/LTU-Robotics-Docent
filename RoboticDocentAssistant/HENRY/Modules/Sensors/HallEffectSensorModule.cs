using HENRY.ModuleSystem;
using System.Timers;


namespace HENRY.Modules.Sensors
{
    class HallEffectSensorModule : GenericSensorModule
    {
        //Timer t;
        
        public HallEffectSensorModule()
        {
            SetPropertyValue("ArraySensor1", false);
            SetPropertyValue("ArraySensor2", false);
            SetPropertyValue("ArraySensor3", false);
            SetPropertyValue("ArraySensor4", false);
            SetPropertyValue("ArraySensor5", false);
        }

        public override string GetModuleName()
        {
            return "HallEffectSensorModule";
        }
    }
}
