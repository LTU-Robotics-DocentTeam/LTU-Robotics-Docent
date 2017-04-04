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

        const int default_Speed = 50; // default speed for both motors when no obstacles are in front or nearby
        
        public BaseNavModule()
        {
            SetPropertyValue("Direction", 0); // Angle in degrees
            SetPropertyValue("Speed", 0); // speed in servo scale (0-180)
            SetPropertyValue("EStop", false); // Send EStop signal (upon Impact)

            t = new Timer();
            t.Interval = 40;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        /// <summary>
        /// Calculate appropiate heading and speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            int UltraNum = GetPropertyValue("UltraSNum").ToInt32();

            int dist2obstacle = 2000;
            int speed;

            // Get line location calculated by hall effects
            int direction = GetPropertyValue("LineAngle").ToInt32();
            
            if (direction >= 0) // if direction is valid (non-negative integer)...
            {
                // Look at Ultrasonic sensors and calculate which one detects the closest object
                for (int i = 0; i < UltraNum; i++)
                {
                    int current_sensor_dist = GetPropertyValue("UltraS" + (i + 1).ToString()).ToInt32();

                    if (current_sensor_dist < dist2obstacle)
                    {
                        dist2obstacle = current_sensor_dist;
                    }
                }

                // Determine current speed based on distance to closest obstacle
                speed = (int)((double)default_Speed * (double)((double)dist2obstacle / 2000));
            }
            else // if its negative (-1 is error state. invalid line location)
            {
                // Set direction and speed to 0
                direction = 90;
                speed = 0;
            }


            // Add infrared stuff here

            //Set calculated direction and speed properties
            if (!GetPropertyValue("ManualDriveEnabled").ToBoolean())
            {
                SetPropertyValue("Direction", direction - 90);
                SetPropertyValue("Speed", speed);
            }

           
        }
        
        public override string GetModuleName()
        {
            return "BaseNavModule";
        }
    }
}
