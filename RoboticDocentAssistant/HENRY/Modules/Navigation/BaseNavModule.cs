using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HENRY.ModuleSystem;
using HENRY.Modules.Sensors;
using System.Timers;

namespace HENRY.Modules.Navigation
{
    /// <summary>
    /// Process all sensor data and determines overall speed and direction of robot autonomous motion
    /// </summary>
    class BaseNavModule : LengarioModuleCore
    {
        public Timer t;
        int hallEffectError = 0, ultrasonicError = 0, lineHoldCounter = 0, speed = 0, time = 0;
        double prevLoc = 0, dirLoc = 0, currLoc = 0, smoothLoc = 0, dspd = 0, smoothdspd = 0, prevsmoothLoc = 0;

        HallEffectSensorModule hem;
        ImpactSensorModule ism;
        UltrasonicSensorModule usm;
        public ErrorLog error_log;
        private bool errorState = false;

        
        public BaseNavModule()
        {
            hem = new HallEffectSensorModule();
            ism = new ImpactSensorModule();
            usm = new UltrasonicSensorModule();
            error_log = new ErrorLog(this);

            
            SetPropertyValue("Direction", 0.0); // Angle in degrees
            SetPropertyValue("Speed", 0); // speed in servo scale (0-180)
            SetPropertyValue("EStop", false); // Send EStop signal (upon Impact)
            SetPropertyValue("AutonomousNavigation", false);

            t = new Timer();
            t.Interval = 10;
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
            // run sensor calculations
            hallEffectError = hem.Calculate();
            //ism.Calculate(); // Mostly vestigial, triggers estop for simulation mode, but that's about it
            ultrasonicError = usm.Calculate();
            //ultrasonicError = 0;

            errorState = false;
            int speed = 0;

            if (hallEffectError < 0)
            {
                // line has been lost for too long
                if (lineHoldCounter >= Constants.LINE_HOLD_BUFFER)
                {
                    // what do if its lost for too long?
                    // do nothing for now. stay put
                    errorState = true;
                }
                else
                {
                    dirLoc = prevLoc;
                    lineHoldCounter++;
                }
            }
            else
            {
                currLoc = GetPropertyValue("LineAngle").ToDouble();
                lineHoldCounter = 0;
                prevLoc = currLoc;
                dirLoc = currLoc;
            }

            if (ultrasonicError < 0)
            {
                errorState = true;
            }
            else
            {
                // multiply speed to whatever the ultrasonic reccomends (1 if all clear, 0 if only turns - not moving forward)
                // that 0 might cause troubles. test for effectiveness before commiting to it
                speed = Constants.DEFAULT_SPEED * GetPropertyValue("ReccomendedUltrasonicSpeed").ToInt32();
            }
            prevsmoothLoc = smoothLoc;
            smoothLoc += (dirLoc - smoothLoc) * 0.1;
            dspd = (smoothLoc - prevsmoothLoc);
            smoothdspd += (dspd - smoothdspd) * 0.1; 

           error_log.WriteToLog(time++ + "," + dirLoc.ToString() + "," + smoothLoc.ToString() + "," + dspd.ToString() + "," + smoothdspd.ToString());

            //Set calculated direction and speed properties
            if (!GetPropertyValue("ManualDriveEnabled").ToBoolean() && GetPropertyValue("AutonomousNavigation").ToBoolean())
            {
                if (!errorState)
                {
                    SetPropertyValue("Direction", dirLoc);
                    SetPropertyValue("Speed", speed);
                }
                else
                {
                    SetPropertyValue("Direction", 0.0);
                    SetPropertyValue("Speed", 0);
                }
                
            }

        }
        
        public void StopModule()
        {
            t.Stop();
            error_log.CloseLog();
            time = 0;
        }
        public void StartModule()
        {
            error_log.OpenLog();
            error_log.WriteToLog("Time,position,smoothposition,speed,smoothspeed");
            t.Start();
        }
        
        public override string GetModuleName()
        {
            return "BaseNavModule";
        }
    }
}
