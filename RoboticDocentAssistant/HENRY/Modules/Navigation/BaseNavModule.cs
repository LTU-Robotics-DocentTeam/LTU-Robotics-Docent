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
        public Timer t;

        
        
        public BaseNavModule()
        {
            SetPropertyValue("Direction", 0); // Angle in degrees
            SetPropertyValue("Speed", 0); // speed in servo scale (0-180)
            SetPropertyValue("EStop", false); // Send EStop signal (upon Impact)
            SetPropertyValue("AutonomousNavigation", false);

            t = new Timer();
            t.Interval = 40;
            t.Elapsed += t_Elapsed;
            //t.Start();
        }

        /// <summary>
        /// Calculate appropiate heading and speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Elapsed(object sender, ElapsedEventArgs e)
        {

            int dist2obstacle = 2000;
            int speed = 0;

            // Get line location calculated by hall effects
            int direction = GetPropertyValue("LineAngle").ToInt32();
            
            if (direction >= 0) // if direction is valid (non-negative integer)...
            {
                // Look at Ultrasonic sensors and calculate which one detects the closest object
                for (int i = 0; i < Constants.US_NUM; i++)
                {
                    int current_sensor_dist = GetPropertyValue("UltraS" + (i + 1).ToString()).ToInt32();
                    
                    if (i == 0) // Takes the mast sensor and subtracts 40mm from the sensed distance to even it out with the rest
                    {
                        current_sensor_dist = current_sensor_dist - Constants.MAST_TO_FRONT;
                    }

                    if (current_sensor_dist < dist2obstacle) // sets smallest distance
                    {
                        dist2obstacle = current_sensor_dist;
                    }
                }

                if (300 < dist2obstacle)
                {
                    speed = Constants.DEFAULT_SPEED;
                }

                else if (100 < dist2obstacle && dist2obstacle < 300)
                {
                    speed = 0;
                }

                else if (dist2obstacle < 100 )
                {
                    speed = 0;
                    direction = 0;
                }
                // Determine current speed based on distance to closest obstacle
                //speed = (int)((double)Constants.DEFAULT_SPEED * (double)((double)dist2obstacle / 2000));
                // speed = 3;
            }
            else // if its negative (-1 is error state. invalid line location)
            {
                // Set direction and speed to 0
                direction = 90;
                speed = 0;
            }


            // Add infrared stuff here

            //Set calculated direction and speed properties
            if (!GetPropertyValue("ManualDriveEnabled").ToBoolean() && GetPropertyValue("AutonomousNavigation").ToBoolean())
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
