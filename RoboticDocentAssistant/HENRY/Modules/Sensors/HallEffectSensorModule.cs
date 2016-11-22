using HENRY.ModuleSystem;


namespace HENRY.Modules.Sensors
{
    class HallEffectSensorModule : GenericSensorModule
    {
        const int ArrayNum = 10; // Number of sensors in hall effect array
        
        public HallEffectSensorModule()
        {
            for (int i = 1; i <= ArrayNum; i++)
                SetPropertyValue("ArraySensor" + i.ToString(), false);

            SetPropertyValue("ArrayNum", ArrayNum);
        }

        public override string GetModuleName()
        {
            return "HallEffectSensorModule";
        }
    }
}
