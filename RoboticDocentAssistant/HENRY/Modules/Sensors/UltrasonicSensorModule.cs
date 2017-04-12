using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Sensors
{
    /// <summary>
    /// Secondary ultrasonic sensor processing.
    /// </summary>
    /// TO DO:
    /// - Find use for it. Whatever isn't processed by the controller, process here.
    class UltrasonicSensorModule : LengarioModuleAuxiliary
    {
        Timer t;

        public UltrasonicSensorModule()
        {
            for (int i = 0; i <= Constants.US_NUM; i++)
                SetPropertyValue("UltraS" + i.ToString(), 0);

            //t = new Timer();
            //t.Interval = 330;
            //t.Elapsed += t_Elapsed;
            //t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            // I think we have the same problem as in infrared
            // with multiple obstacles 
            // (See comment in InfraredSensorModule)
        }
        
        public override string GetModuleName()
        {
            return "UltrasonicSensorModule";
        }
    }
}
