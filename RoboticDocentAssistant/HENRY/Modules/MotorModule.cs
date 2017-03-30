using HENRY.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimersTimer = System.Timers.Timer;

namespace HENRY.Modules
{
    class MotorModule : LengarioModuleAuxiliary
    {
        TimersTimer t;
        const int MAXSPEED = 180; // DO NOT CHANGE -- Maximum speed the motors can take (based on regular servo code)
                                  // 180 is SANIC fast, so don't set spd to the max
        
        public MotorModule()
        {
            // Initialize properties to default
            SetPropertyValue("RightMSpeed", 0);
            SetPropertyValue("LeftMSpeed", 0);
            SetPropertyValue("MaximumSpeed", MAXSPEED);

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 1;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int dir = GetPropertyValue("Direction").ToInt32();
            int spd = GetPropertyValue("Speed").ToInt32();

            int rmspeed = 0;
            int lmspeed = 0;

            double rdir = 0;
            double ldir = 0;

            if (spd > 0)
            {
                if (dir >= 90)
                {
                    rdir = 90;
                    ldir = 90 - (dir - 90);
                }
                else
                {
                    rdir = dir;
                    ldir = 90;
                }


                rmspeed = (int)((rdir / 90.0) * (spd * 2.0)) - spd;
                lmspeed = (int)((ldir / 90.0) * (spd * 2.0)) - spd;
            }
            else
            {
                rmspeed = spd;
                lmspeed = spd;
            }

            // Ensure total speed does not exceed MAXSPEED
            if (rmspeed < -MAXSPEED)
                rmspeed = -MAXSPEED;
            if (rmspeed > MAXSPEED)
                rmspeed = MAXSPEED;
            if (lmspeed < -MAXSPEED)
                lmspeed = -MAXSPEED;
            if (lmspeed > MAXSPEED)
                lmspeed = MAXSPEED;

            //Update current property value
            SetPropertyValue("RightMSpeed", rmspeed);
            SetPropertyValue("LeftMSpeed", lmspeed);
        }

        public override string GetModuleName()
        {
            return "MotorModule";
        }
    }
}
