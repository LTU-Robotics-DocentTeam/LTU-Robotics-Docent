using HENRY.ModuleSystem;
using System;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using TimersTimer = System.Timers.Timer;

namespace HENRY.Modules
{
    class SerialCommModule : LengarioModuleCore
    {
        TimersTimer t;
        Random r;

        SerialPort serPort;
        string ComPort = "COM4";
        string signal = ""; // signal from the arduino. Perhaos pack all data as one long string, and then parse it?

        public SerialCommModule()
        {
            // These are all Serial Port initializations. Comment out to be able to run program without serial comms
            //====================================================================================================
            //serPort = new SerialPort(ComPort);
            //serPort.BaudRate = 9600;
            //serPort.DataBits = 8;
            //serPort.Parity = Parity.None;
            //serPort.StopBits = StopBits.One;
            //serPort.Open();
            //serPort.DataReceived += new SerialDataReceivedEventHandler(serPort_DataReceived);
            //====================================================================================================

            t = new TimersTimer();
            t.Interval = 1000;
            t.Elapsed += t_Elapsed;
            t.Start();

            r = new Random();

        }

        void serPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            signal = serPort.ReadLine(); // Receiving Arduino data as one string
        }

        // Ok this is goofy I know, but this sort of emulates what the SerialCommModule
        // would do, take in data and assign it to various properties. This example uses
        // a timer event to trigger random property writes. When implementing the real
        // thing we can swap the timer for the real serial port read event.

        // Note: It is legal for this module to write to Properties it doesn't own, because
        // it inherits the LengarioModuleCore. The only Core Modules we will need are probably this
        // one, the ViewModel, and maybe a main navigation/control one that uses the finished
        // and refined data from all of the Auxiliary Modules.

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetPropertyValue("Generic_Sensor1", r.Next(0, 100));
            SetPropertyValue("Generic_Sensor2", r.Next(0, 100));
            SetPropertyValue("Generic_Sensor3", r.Next(0, 100));
            



            int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
            int line = r.Next(0, ArrayNum); // pick a random sensor to place line

            
            
            for (int i = 1; i <= ArrayNum; i++)
            {
                if (i > line - 2 && i < line + 2)
                    SetPropertyValue("ArraySensor" + i.ToString(), true);
                else
                    SetPropertyValue("ArraySensor" + i.ToString(), false);
            }
            for (int i = 1; i <= 8; i++)
            {
                SetPropertyValue("IR" + i.ToString(), r.NextDouble()*100.0);
                SetPropertyValue("UltraS" + i.ToString(), r.NextDouble()*100.0);
            }

            if (r.Next(0, 100) < 50)
            {
                SetPropertyValue("Impact" + r.Next(0, ImpactNum-1).ToString(), true);
            }
            else
            {
                for (int i = 1; i <= ImpactNum; i++)
                {
                    SetPropertyValue("Impact" + i.ToString(), false);
                }
            }
        }


        //=================================== Inheritance Implementation =======================================

        //Method required by ModuleSystem to identify module in respect to system
        //Required for every module

        public override string GetModuleName()
        {
            return "SerialCommModule";
        }
    }
}
