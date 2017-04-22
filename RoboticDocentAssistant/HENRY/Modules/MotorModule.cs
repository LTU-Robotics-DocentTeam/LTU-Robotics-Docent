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
        ErrorLog plots;
        double alpha = 5.0, beta = 0; //Beta = D, Alpha = P
        int time = 0;
        public bool recording = false;
        
        public MotorModule()
        {
            plots = new ErrorLog(this);
            
            // Initialize properties to default
            SetPropertyValue("RightMValue", 0);
            SetPropertyValue("LeftMValue", 0);
            SetPropertyValue("RightSpeed", 0.0);
            SetPropertyValue("LeftSpeed", 0.0);
            SetPropertyValue("DirectionalSpeed", 0);
            SetPropertyValue("LeftBrake", false);
            SetPropertyValue("RightBrake", false); 
            SetPropertyValue("EStop", false);
            

            // Set processing timer for module
            t = new TimersTimer();
            t.Interval = 20;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool estop = GetPropertyValue("EStop").ToBoolean();

            

            int rmSpeed = (int)Math.Round(GetPropertyValue("RightSpeed").ToDouble());
            int lmSpeed = (int)Math.Round(GetPropertyValue("LeftSpeed").ToDouble());

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

            plots.WriteToLog(time++ + "," + rmSpeed.ToString() + "," + lmSpeed.ToString());

            //Update current property value
            if (!estop && (GetPropertyValue("ManualDriveEnabled").ToBoolean() || GetPropertyValue("AutonomousNavigation").ToBoolean()))
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

        public void StopRecording()
        {
            plots.CloseLog();
            recording = false;
        }
        public void StartRecording()
        {
            plots.OpenLog();
            time = 0;
            plots.WriteToLog("Time,Right Motor Speed,Left Motor Speed");
            recording = true;
        }

        public override string GetModuleName()
        {
            return "MotorModule";
        }
    }
}
