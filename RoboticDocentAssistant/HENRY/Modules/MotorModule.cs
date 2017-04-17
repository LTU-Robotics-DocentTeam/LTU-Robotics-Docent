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
            double direction = GetPropertyValue("Direction").ToDouble();
            int spd = GetPropertyValue("Speed").ToInt32();
            bool estop = GetPropertyValue("EStop").ToBoolean();

            int rmSpeed = 0, lmSpeed = 0;

            if (spd > 0)
            {
                int dSpd = (int)((spd) * (direction / Constants.MAX_DIR));
                rmSpeed = spd + dSpd;
                lmSpeed = spd - dSpd;

                //if (direction > 0)
                //{
                //    rmSpeed = spd;
                //    lmSpeed = spd - (int)Math.Abs((spd) * (direction / Constants.MAX_DIR));
                //}
                //else
                //{
                //    rmSpeed = spd - (int)Math.Abs((spd) * (direction / Constants.MAX_DIR));
                //    lmSpeed = spd;
                //}
            }
            else if (spd == 0)
            {
                rmSpeed = (int)direction;
                lmSpeed = -(int)direction;
            }
            else
            {
                rmSpeed = spd;
                lmSpeed = spd;
            }

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
