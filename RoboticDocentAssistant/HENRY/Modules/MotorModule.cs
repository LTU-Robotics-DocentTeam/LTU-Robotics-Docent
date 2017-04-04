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
            // Get values for direction and speed for whichever module was working on them
            int direction = GetPropertyValue("Direction").ToInt32();
            int speed = GetPropertyValue("Speed").ToInt32();

            int rmSpeed = 0;
            int lmSpeed = 0;

            double rDirection = 0;
            double lDirection = 0;

            if (speed > 0) //if speed is non zero
            {
                // Use robot direction to determine individual motor direction
                if (direction >= 90) // if turning left...
                {
                    rDirection = 90; // right remains at full speed
                    lDirection = 90 - (direction - 90); // left slows down directly proportional to how much to the left robot needs to turn
                }
                else // if turning right...
                {
                    rDirection = direction; // right slows down directly proportional to how much to the left robot needs to turn
                    lDirection = 90; // left remains at full speed
                }

                // using previously calculated multipliers, use this formula to determine differential speeds
                rmSpeed = (int)((rDirection / 90.0) * (speed * 2.0)) - speed;
                lmSpeed = (int)((lDirection / 90.0) * (speed * 2.0)) - speed;
            }
            else // if speed is zero or negative
            {
                // ignore direction and just go backwards (or remain stationary for 0 speed)
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
