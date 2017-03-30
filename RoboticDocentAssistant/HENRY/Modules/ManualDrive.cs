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
    /// - Scale servo speed values to m/s for manualdrive
    class ManualDrive : LengarioModuleCore
    {
        
        
        TimersTimer t;
        const int spd = 20; // Current speed for robot movement (CANNOT EXCEED MAXSPEED)
        
        public ManualDrive()
        {
            // Initialize properties to default
            SetPropertyValue("ManualDriveEnabled", false);
            SetPropertyValue("Forward", false);
            SetPropertyValue("Backward", false);
            SetPropertyValue("Right", false);
            SetPropertyValue("Left", false);
            SetPropertyValue("Direction", 0); // Angle in degrees
            SetPropertyValue("ManualSpeed", 20);

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 1;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (GetPropertyValue("ManualSpeed").ToInt32() > GetPropertyValue("MaximumSpeed").ToInt32()) 
                SetPropertyValue("ManualSpeed", GetPropertyValue("MaximumSpeed"));
            if (GetPropertyValue("ManualSpeed").ToInt32() < 0)
                SetPropertyValue("ManualSpeed", 0);
            int spd = GetPropertyValue("ManualSpeed").ToInt32();


            int direction = 0;
            int speed = 0;
            int btns = 0;

            if (GetPropertyValue("Backward").ToBoolean()) // Set both motors backwards
            {
                direction = 90;
                speed = spd;
            }
            else
            {
                if (GetPropertyValue("Forward").ToBoolean()) // Set both motors forward
                {
                    direction = 90;
                    btns = 1;
                }
                if (GetPropertyValue("Right").ToBoolean()) // Zero-point turn to the right
                {
                    btns++;
                    direction = (direction + 0) / btns;
                }
                if (GetPropertyValue("Left").ToBoolean()) // Zero-point turn to the left
                {
                    btns++;
                    direction = (direction + 180) / btns;
                }
                if (btns > 0)
                {
                    speed = spd;
                }
            }
            

            SetPropertyValue("Direction", direction);
            SetPropertyValue("Speed", speed);
        }




        public override string GetModuleName()
        {
            return "ManualDrive";
        }
    }
}
