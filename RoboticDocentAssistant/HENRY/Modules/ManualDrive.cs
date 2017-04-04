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
    /// - Make it a little more advanced than WASD
    /// - Scale servo speed values to m/s for manualdrive
    /// - 
    class ManualDrive : LengarioModuleCore
    {   
        TimersTimer t;
        
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
            // Load manual speed from gui input, and prevent from being beyond 180 or below 0
            if (GetPropertyValue("ManualSpeed").ToInt32() > GetPropertyValue("MaximumSpeed").ToInt32()) 
                SetPropertyValue("ManualSpeed", GetPropertyValue("MaximumSpeed").ToInt32());
            if (GetPropertyValue("ManualSpeed").ToInt32() < 0)
                SetPropertyValue("ManualSpeed", 0);
            int manualSpeed = GetPropertyValue("ManualSpeed").ToInt32();

            // Reset local variables
            int direction = 0;
            int speed = 0;
            int btns = 0;

            if (GetPropertyValue("Backward").ToBoolean()) // backwards motion overrides any other motion
            {
                direction = 90; // motor module currently ignores direction for non-positive speeds. 
                                // Sets to 90 for display reasons because it backs up in a straight line
                speed = -manualSpeed; // set overall speed negative
            }
            else
            {
                if (GetPropertyValue("Forward").ToBoolean()) // Go forward
                {
                    direction = 90; // point straight ahead
                    btns = 1; // indicate directional button pressed for future calculations
                }
                if (GetPropertyValue("Right").ToBoolean()) // Change heading to the right
                {
                    btns++; // indicate additional button pressed
                    direction = (direction + 0) / btns; // change direction to the right. average direction if forward was also pressed
                }
                if (GetPropertyValue("Left").ToBoolean()) // Change heading to the left
                {
                    btns++; // indicate additional button pressed
                    direction = (direction + 180) / btns; // change direction to the left. average direction if forward was also pressed
                }
                if (btns > 2) // if more than two buttons pressed...
                {
                    // send nothing to prevent confusion
                    direction = 0;
                    speed = 0;
                }
                else if (btns > 0)
                {
                    // if more than one button pressed, send speed forward
                    speed = manualSpeed;
                }
                
            }

            // Only send direction and speed data if Manual drive is enabled
            if (GetPropertyValue("ManualDriveEnabled").ToBoolean())
            {
                SetPropertyValue("Direction", direction);
                SetPropertyValue("Speed", speed);
            }
            
        }




        public override string GetModuleName()
        {
            return "ManualDrive";
        }
    }
}
