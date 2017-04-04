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

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 1;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int direction = GetPropertyValue("Direction").ToInt32();
            int spd = GetPropertyValue("Speed").ToInt32();

            //double rDirection = 0;
            //double lDirection = 0;

            int rmDirSpeed = (int)((0.334) * (direction));
            int lmDirSpeed = (int)((0.334) * (direction));
            
            int rmSpeed = spd + rmDirSpeed;
            int lmSpeed = spd - lmDirSpeed;

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
            SetPropertyValue("RightMSpeed", rmSpeed);
            SetPropertyValue("LeftMSpeed", lmSpeed);
        }

        public override string GetModuleName()
        {
            return "MotorModule";
        }
    }
}
