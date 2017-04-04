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
            int direction = GetPropertyValue("Direction").ToInt32();
            int speed = GetPropertyValue("Speed").ToInt32();

            int rmSpeed = 0;
            int lmSpeed = 0;

            double rDirection = 0;
            double lDirection = 0;

            if (speed > 0)
            {
                if (direction >= 90)
                {
                    rDirection = 90;
                    lDirection = 90 - (direction - 90);
                }
                else
                {
                    rDirection = direction;
                    lDirection = 90;
                }


                rmSpeed = (int)((rDirection / 90.0) * (speed * 2.0)) - speed;
                lmSpeed = (int)((lDirection / 90.0) * (speed * 2.0)) - speed;
            }
            else
            {
                rmSpeed = speed;
                lmSpeed = speed;
            }

            // Ensure total speed does not exceed MAXSPEED
            if (rmSpeed < -MAXSPEED)
                rmSpeed = -MAXSPEED;
            if (rmSpeed > MAXSPEED)
                rmSpeed = MAXSPEED;
            if (lmSpeed < -MAXSPEED)
                lmSpeed = -MAXSPEED;
            if (lmSpeed > MAXSPEED)
                lmSpeed = MAXSPEED;

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
