using HENRY.ModuleSystem;
using System.Timers;


namespace HENRY.Modules.Sensors
{
    class HallEffectSensorModule : GenericSensorModule
    {
        Timer t;
        
        const int ArrayNum = 20; // Number of sensors in hall effect array
        double prevline = -1;
        
        public HallEffectSensorModule()
        {
            for (int i = 1; i <= ArrayNum; i++)
                SetPropertyValue("ArraySensor" + i.ToString(), false);

            SetPropertyValue("ArrayNum", ArrayNum);

            SetPropertyValue("LineAngle", 0);

            t = new Timer();
            t.Interval = 330;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            //int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            bool[] arr = new bool[ArrayNum];
            double anglestep = 360.0 / ArrayNum;
            double lineloc = -1; // start as negative number, so if no line is found, a negative number is sent to main module
                                 // Negative number means error state in nav module, so no line

            for (int i = 0; i < ArrayNum; i++)
            {
                arr[i] = GetPropertyValue("ArraySensor" + (i+1).ToString()).ToBoolean();

                // if any three adjacent sensors are triggered, that means there's a line there.
                // if on the first sensor or last sensor, check for current sensor and the only adjacent sensor available instead
                if ((arr[i] && arr[i-1] && arr[i+1] && (i != 0) && (i != ArrayNum)) || (arr[i] && arr[i+1] && (i == 0)) || (arr[i] && arr[i-1] && (i == ArrayNum)))
                {
                    lineloc = anglestep * (i + 1); // give angle where the line is located based on middle sensor triggered location
                }

                if (prevline > -1 && (lineloc < prevline + 10 && lineloc > prevline - 10))
                {
                    SetPropertyValue("LineAngle", lineloc);
                    prevline = lineloc;
                }
                else
                {
                    lineloc = -1;
                    SetPropertyValue("LineAngle", lineloc);
                }
            }


        }

        public override string GetModuleName()
        {
            return "HallEffectSensorModule";
        }
    }
}
