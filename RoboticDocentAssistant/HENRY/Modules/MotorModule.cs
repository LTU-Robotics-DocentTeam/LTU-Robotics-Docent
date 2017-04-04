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
        const int MAXSPEED = 120; // DO NOT CHANGE -- Maximum speed the motors can take (based on regular servo code)
                                  // 180 is SANIC fast, so don't set spd to the max
        const int DEAD_ZONE = 55;
        
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
            int direction = GetPropertyValue("Direction").ToInt32();
            int spd = GetPropertyValue("Speed").ToInt32();

            //double rDirection = 0;
            //double lDirection = 0;

            int rmDirSpeed = (int)((0.3) * (direction));
            int lmDirSpeed = (int)((0.3) * (direction));
            
            int rmSpeed = spd + rmDirSpeed;
            int lmSpeed = spd - lmDirSpeed;

            //if (spd > 0)
            //{
            //    if (direction >= 90)
            //    {
            //        rDirection = 90;
            //        lDirection = 90 - (direction - 90);
            //    }
            //    else
            //    {
            //        rDirection = direction;
            //        lDirection = 90;
            //    }


            //    rmSpeed = (int)((rDirection / 90.0) * (spd * 2.0)) - spd;
            //    lmSpeed = (int)((lDirection / 90.0) * (spd * 2.0)) - spd;
            //}
            //else
            //{
            //    rmSpeed = spd;
            //    lmSpeed = spd;
            //}

            // Ensure total speed does not exceed MAXSPEED
            if (rmSpeed < -MAXSPEED)
                rmSpeed = -MAXSPEED;
            if (rmSpeed > MAXSPEED)
                rmSpeed = MAXSPEED;
            if (rmSpeed < DEAD_ZONE && rmSpeed != 0)
                rmSpeed = DEAD_ZONE;
            else if (rmSpeed > -DEAD_ZONE && rmSpeed != 0)
                rmSpeed = -DEAD_ZONE;
            if (lmSpeed < -MAXSPEED)
                lmSpeed = -MAXSPEED;
            if (lmSpeed > MAXSPEED)
                lmSpeed = MAXSPEED;
            if (lmSpeed < DEAD_ZONE && rmSpeed != 0)
                lmSpeed = DEAD_ZONE;
            else if (lmSpeed > DEAD_ZONE && rmSpeed != 0)
                lmSpeed = -DEAD_ZONE;

            //Update current property value
            SetPropertyValue("RightMSpeed", rmSpeed);
            SetPropertyValue("LeftMSpeed", lmSpeed);
        }

        public override string GetModuleName()
        {
            return "MotorModule";
        }
    }
}
