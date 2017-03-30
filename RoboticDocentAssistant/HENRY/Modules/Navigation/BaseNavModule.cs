using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Navigation
{
    /// <summary>
    /// Process all sensor data and determines overall speed and direction of robot autonomous motion
    /// </summary>
    class BaseNavModule : LengarioModuleCore
    {
        Timer t;
        
        public BaseNavModule()
        {
            SetPropertyValue("Direction", 0); // Angle in degrees
            SetPropertyValue("Speed", 0); // speed in servo scale (0-180)
            SetPropertyValue("EStop", false); // Send EStop signal (upon Impact)

            t = new Timer();
            t.Interval = 330;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
            int IRNum = GetPropertyValue("IRNum").ToInt32();
            int UltraSNum = GetPropertyValue("UltraSNum").ToInt32();
            
            for (int i = 1; i <= ArrayNum; i++)
            {
                // Do angle calculation for direction here
            }

        }
        
        public override string GetModuleName()
        {
            return "BaseNavModule";
        }
    }
}
