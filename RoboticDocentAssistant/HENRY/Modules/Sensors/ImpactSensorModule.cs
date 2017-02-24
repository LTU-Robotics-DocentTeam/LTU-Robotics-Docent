using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Sensors
{
    class ImpactSensorModule : LengarioModuleAuxiliary
    {
        Timer t;

        const int ImpactNum = 10;
        
        public ImpactSensorModule()
        {
            for (int i = 1; i <= ImpactNum; i++)
                SetPropertyValue("Impact" + i.ToString(), false);

            SetPropertyValue("ImpactNum", ImpactNum);

            SetPropertyValue("EStop", false);

            t = new Timer();
            t.Interval = 330;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 1; i <= ImpactNum; i++)
            {
                // if any of them trigger, stop immediately
                if (GetPropertyValue("Impact" + i.ToString()).ToBoolean())
                {
                    SetPropertyValue("EStop", true);
                    break;
                }
            }
        }
        
        public override string GetModuleName()
        {
            return "ImpactSensorModule";
        }
    }
}
