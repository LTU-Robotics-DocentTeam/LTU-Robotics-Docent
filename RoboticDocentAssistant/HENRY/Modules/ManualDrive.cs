﻿using HENRY.ModuleSystem;
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
    class ManualDrive : LengarioModuleCore
    {
        public TimersTimer t;
        
        public ManualDrive()
        {
            // Initialize properties to default
            SetPropertyValue("ManualDriveEnabled", false);
            SetPropertyValue("Forward", false);
            SetPropertyValue("Backward", false);
            SetPropertyValue("Right", false);
            SetPropertyValue("Left", false);
            SetPropertyValue("Direction", 0.0); // Angle in degrees
            SetPropertyValue("ManualSpeed", Constants.DEFAULT_SPEED);

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 1;
            t.Elapsed += t_Elapsed;
            //t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (GetPropertyValue("ManualSpeed").ToInt32() > Constants.MAX_SPEED) 
                SetPropertyValue("ManualSpeed", Constants.MAX_SPEED);
            if (GetPropertyValue("ManualSpeed").ToInt32() < 0)
                SetPropertyValue("ManualSpeed", 0);
            int manualSpeed = GetPropertyValue("ManualSpeed").ToInt32();

            int rspeed = 0, lspeed = 0;

            if (GetPropertyValue("Backward").ToBoolean()) // Set both motors backwards
            {
                rspeed -= manualSpeed;
                lspeed -= manualSpeed;
            }
            if (GetPropertyValue("Forward").ToBoolean()) // Set both motors forward
            {
                rspeed += manualSpeed;
                lspeed += manualSpeed;
            }
            if (GetPropertyValue("Left").ToBoolean()) // Zero-point turn to the left
            {
                rspeed += manualSpeed;
                lspeed -= manualSpeed;
            }
            if (GetPropertyValue("Right").ToBoolean()) // Zero-point turn to the right
            {
                rspeed -= manualSpeed;
                lspeed += manualSpeed;
            }
            

            if (GetPropertyValue("ManualDriveEnabled").ToBoolean())
            {
                SetPropertyValue("RightSpeed", (double)rspeed);
                SetPropertyValue("LeftSpeed", (double)lspeed);
            }
            
        }


        public override string GetModuleName()
        {
            return "ManualDrive";
        }
    }
}
