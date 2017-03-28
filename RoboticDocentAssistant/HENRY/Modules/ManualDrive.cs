using HENRY.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimersTimer = System.Timers.Timer;

namespace HENRY.Modules
{
    /// <summary>
    /// Handles conversion from Directional input (WASD) into motor speeds.
    /// </summary>
    /// TO DO:
    /// - Changing manual speed during runtime
    /// - Make it a little more advanced than WASD
    class ManualDrive : LengarioModuleCore
    {
        
        
        TimersTimer t;
        const int spd = 20; // Current speed for robot movement (CANNOT EXCEED MAXSPEED)
        const int MAXSPEED = 180; // DO NOT CHANGE -- Maximum speed the motors can take (based on regular servo code)
                                  // 180 is SANIC fast, so don't set spd to the max
        
        public ManualDrive()
        {
            // Initialize properties to default
            SetPropertyValue("ManualDriveEnabled", false);
            SetPropertyValue("Forward", false);
            SetPropertyValue("Backward", false);
            SetPropertyValue("Right", false);
            SetPropertyValue("Left", false);
            SetPropertyValue("RightMSpeed", 0);
            SetPropertyValue("LeftMSpeed", 0);

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 1;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // If no keys are pressed, set both speeds to 0
            int rmspeed = 0;
            int lmspeed = 0;

            if (GetPropertyValue("Forward").ToBoolean()) // Set both motors forward
            {
                rmspeed += spd;
                lmspeed += spd;
            }
            if (GetPropertyValue("Backward").ToBoolean()) // Set both motors backwards
            {
                rmspeed -= spd;
                lmspeed -= spd;
            }
            if (GetPropertyValue("Right").ToBoolean()) // Zero-point turn to the right
            {
                rmspeed -= spd / 2;
                lmspeed += spd / 2;
            }
            if (GetPropertyValue("Left").ToBoolean()) // Zero-point turn to the left
            {
                rmspeed += spd / 2;
                lmspeed -= spd / 2;
            }
            // Multiple directions allow for more varied movement (i.e. forward + right = gradual right turn)

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
            return "ManualDrive";
        }
    }
}
