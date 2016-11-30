using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Sensors
{
    class InfraredSensorModule : LengarioModuleAuxiliary
    {
        //Timer t;
        
        const int IRNum = 8;

        public InfraredSensorModule()
        {
            for (int i = 1; i <= IRNum; i++)
                SetPropertyValue("IR" + i.ToString(), 0.0);
            
            SetPropertyValue("IRNum", IRNum);

            //t = new Timer();
            //t.Interval = 330;
            //t.Elapsed += t_Elapsed;
            //t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Was gonna do a similar processing than for the hall effects
            // but I don't know how we would handle multiple obstacles (ie
            // I send an array of obstacle locations?) so I'm leaving 
            // empty for now, might need to do some preprocessing and
            // then nav takes care of the obstacle finding?
        }

        public override string GetModuleName()
        {
            return "InfraredSensorModule";
        }
    }
}
