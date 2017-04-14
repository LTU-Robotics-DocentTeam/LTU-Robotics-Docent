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
        
        
        public MotorModule()
        {
            // Initialize properties to default
            SetPropertyValue("RightMSpeed", 0);
            SetPropertyValue("LeftMSpeed", 0);
            SetPropertyValue("DirectionalSpeed", 0);
            SetPropertyValue("LeftBrake", false);
            SetPropertyValue("RightBrake", false); 
            SetPropertyValue("EStop", false);
            

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 10;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int direction = GetPropertyValue("Direction").ToInt32();
            int spd = GetPropertyValue("Speed").ToInt32();
            bool estop = GetPropertyValue("EStop").ToBoolean();

            //double rDirection = 0;
            //double lDirection = 0;
            int dirSpeed;

            // Calculate directional speed
            if (!(spd < 2 && spd >= 0)) dirSpeed = (int)((direction / 90.0 * (spd)) / Constants.TURN_FACTOR);
            else dirSpeed = (int)((direction / 90.0 * 1));

            SetPropertyValue("DirectionalSpeed", dirSpeed);

            // Calculate total speed
            int rmSpeed = spd + dirSpeed;
            int lmSpeed = spd - dirSpeed;

            // Add deadzone gap to the speed
            if (rmSpeed > 0)
            {
                rmSpeed += Constants.DEAD_ZONE;
            }
            else if (rmSpeed < 0)
            {
                rmSpeed -= Constants.DEAD_ZONE;
            }
            if (lmSpeed > 0)
            {
                lmSpeed += Constants.DEAD_ZONE;
            }
            else if (lmSpeed < 0)
            {
                lmSpeed -= Constants.DEAD_ZONE;
            }

            // Ensure total speed does not exceed MAXSPEED
            if (rmSpeed < -Constants.MAX_MOTOR_SPEED)
                rmSpeed = -Constants.MAX_MOTOR_SPEED;
            if (rmSpeed > Constants.MAX_MOTOR_SPEED)
                rmSpeed = Constants.MAX_MOTOR_SPEED;
            if (lmSpeed < -Constants.MAX_MOTOR_SPEED)
                lmSpeed = -Constants.MAX_MOTOR_SPEED;
            if (lmSpeed > Constants.MAX_MOTOR_SPEED)
                lmSpeed = Constants.MAX_MOTOR_SPEED;

            //Update current property value
            if ((GetPropertyValue("ManualDriveEnabled").ToBoolean() || GetPropertyValue("AutonomousNavigation").ToBoolean()))
            {
                SetPropertyValue("RightMSpeed", rmSpeed);
                SetPropertyValue("LeftMSpeed", lmSpeed);
            }
            else
            {
                SetPropertyValue("RightMSpeed", 0);
                SetPropertyValue("LeftMSpeed", 0);
            }
        }

        public override string GetModuleName()
        {
            return "MotorModule";
        }
    }
}
