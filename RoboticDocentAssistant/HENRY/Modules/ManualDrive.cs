using HENRY.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimersTimer = System.Timers.Timer;

namespace HENRY.Modules
{
    class ManualDrive : LengarioModuleCore
    {
        TimersTimer t;
        
        public ManualDrive()
        {
            SetPropertyValue("ManualDriveEnabled", false);
            SetPropertyValue("Forward", false);
            SetPropertyValue("Backward", false);
            SetPropertyValue("Right", false);
            SetPropertyValue("Left", false);
            SetPropertyValue("RightMSpeed", 0);
            SetPropertyValue("LeftMSpeed", 0);
            t = new TimersTimer();
            t.Interval = 20;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int rmspeed = 0;
            int lmspeed = 0;
            if (GetPropertyValue("Forward").ToBoolean())
            {
                rmspeed += 90;
                lmspeed += 90;
            }
            if (GetPropertyValue("Right").ToBoolean())
            {
                rmspeed -= 90;
                lmspeed += 90;
            }
            if (GetPropertyValue("Left").ToBoolean())
            {
                rmspeed += 90;
                lmspeed -= 90;
            }
            if (rmspeed < 0)
                rmspeed = 0;
            if (rmspeed > 180)
                rmspeed = 180;
            if (lmspeed < 0)
                lmspeed = 0;
            if (lmspeed > 180)
                lmspeed = 180;
            

            SetPropertyValue("RightMSpeed", rmspeed);
            SetPropertyValue("LeftMSpeed", lmspeed);
        }




        public override string GetModuleName()
        {
            return "ManualDrive";
        }
    }
}
