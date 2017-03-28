using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Sensors
{
    /// <summary>
    /// Secondary impact sensor processing. As of now, useless outside of simulation (randomly generated data processing)
    /// mode. If impact sensor triggers, trigger estop. simple as that
    /// </summary>
    /// TO DO:
    /// - Eliminate? Redundant considering the processing is done by the controller. Might help as safety net, but acts
    ///   much slower than the controller.
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
