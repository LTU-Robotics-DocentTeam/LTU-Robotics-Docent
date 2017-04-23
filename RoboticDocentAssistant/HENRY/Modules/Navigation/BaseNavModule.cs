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
        public Timer tSlow;
        int hallEffectError = 0, ultrasonicError = 0, lineHoldCounter = 0;
        

        double theta;
        double thetaSmooth;
        double thetaSmoothPrev;
        double thetaDot;
        double thetaDotSmooth;

        int time;


        double Kp;
        double Kd;

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


            time = 0;


            SetPropertyValue("Direction", 0.0);
            SetPropertyValue("DeltaDirection", 0.0);
            SetPropertyValue("Speed", 0); // speed in servo scale (0-180)
            SetPropertyValue("EStop", false); // Send EStop signal (upon Impact)
            SetPropertyValue("AutonomousNavigation", false);

            SetPropertyValue("Kp", 75.0);

            SetPropertyValue("Kd", 25.0);

            t = new Timer();
            t.Interval = 20;
            t.Elapsed += t_Elapsed;


            tSlow = new Timer();
            tSlow.Interval = 100;
            tSlow.Elapsed += tSlow_Elapsed;


        }

        /// <summary>
        /// Calculate appropiate heading and speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void tSlow_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            thetaDot = thetaSmooth - thetaSmoothPrev;
            thetaSmoothPrev = thetaSmooth;

        } 


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
                    //dirLoc = prevLoc;
                    lineHoldCounter++;
                }
            }
            else
            {

                theta = GetPropertyValue("LineAngle").ToDouble();

                lineHoldCounter = 0;

            }

            /*if (ultrasonicError < 0)
            {
                errorState = true;
            }
            else
            {
                // multiply speed to whatever the ultrasonic reccomends (1 if all clear, 0 if only turns - not moving forward)
                // that 0 might cause troubles. test for effectiveness before commiting to it
                speed = Constants.DEFAULT_SPEED;// *GetPropertyValue("ReccomendedUltrasonicSpeed").ToInt32();
                SetPropertyValue("Extra", speed.ToString());
            }*/

            speed = 5;

            thetaSmooth = thetaSmooth + (theta - thetaSmooth) * 0.2;
            thetaDotSmooth = thetaDotSmooth + (thetaDot - thetaDotSmooth) * 0.2;
            


            //Set calculated direction and speed properties
            if (!GetPropertyValue("ManualDriveEnabled").ToBoolean() && GetPropertyValue("AutonomousNavigation").ToBoolean())
            {
                if (!errorState)
                {
                    SetPropertyValue("Direction", Math.Round(thetaSmooth,2));
                    SetPropertyValue("DeltaDirection", Math.Round(thetaDotSmooth, 2));
                    SetPropertyValue("Speed", speed);


                }
                else
                {
                    SetPropertyValue("Direction", 0.0);
                    SetPropertyValue("DeltaDirection", 0.0);
                    SetPropertyValue("Speed", 0);

                    thetaSmooth = 0;
                    thetaDotSmooth = 0;

                    speed = 0;
                }

                
                
            }


            Kp = GetPropertyValue("Kp").ToDouble() / 1000;
            Kd = GetPropertyValue("Kd").ToDouble() / 1000;


            double rmSpeed = 0;
            double lmSpeed = 0;
            double differential = 0;

            if (speed > 0)
            {
                differential = (speed) * (Kp * (thetaSmooth / Constants.MAX_DIR) + Kd * thetaDotSmooth);
                rmSpeed = speed + differential;
                lmSpeed = speed - differential;
                
            }

            error_log.WriteToLog(time++ + "," + thetaSmooth.ToString() + "," + thetaDotSmooth.ToString() + "," + differential.ToString());

            if (rmSpeed < 0) rmSpeed = 0;
            if (lmSpeed < 0) lmSpeed = 0;

            SetPropertyValue("LeftSpeed", lmSpeed);
            SetPropertyValue("RightSpeed", rmSpeed);

        }


        
        public void StopModule()
        {
            t.Stop();
            tSlow.Stop();
            error_log.CloseLog();
            usm.StopRecording();
            SetPropertyValue("Direction", 0.0);
            SetPropertyValue("DeltaDirection", 0.0);
            SetPropertyValue("Speed", 0);
        }
        public void StartModule()
        {
            error_log.OpenLog();
            usm.StartRecording();
            error_log.WriteToLog("Time,position,smoothposition,speed,smoothspeed");
            System.Threading.Thread.Sleep(500);
            t.Start();
            tSlow.Start();

            time  = 0;

            lineHoldCounter = 0;

            theta = 0;
            thetaSmooth = 0;
            thetaSmoothPrev = 0;
            thetaDot = 0;
            thetaDotSmooth = 0;

            
        }
        
        public override string GetModuleName()
        {
            return "BaseNavModule";
        }
    }
}
