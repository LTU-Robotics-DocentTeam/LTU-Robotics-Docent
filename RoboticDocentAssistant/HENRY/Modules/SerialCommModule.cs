using HENRY.ModuleSystem;
using System;
using System.IO;
using System.IO.Ports;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using TimersTimer = System.Timers.Timer;

namespace HENRY.Modules
{
    class SerialCommModule : LengarioModuleCore
    {
        public enum Connection { Unknown, Disconnected, Connected };

        public Connection robotConn;
        
        TimersTimer t;
        Random r;

        SerialPort serPort;
        
        string signal = "", msg = "";
        int prvr = 0, prvl = 0;

        public SerialCommModule()
        {
            // These are all Serial Port initializations
            //====================================================================================================
            robotConn = Connection.Unknown;
            serPort = new SerialPort();
            serPort.BaudRate = 115200;
            serPort.DataBits = 8;
            serPort.Parity = Parity.None;
            serPort.StopBits = StopBits.One;
            serPort.DataReceived += new SerialDataReceivedEventHandler(serPort_DataReceived);
            //====================================================================================================

            ConnectBot(); // Function that handles robot connection initialization
            SetPropertyValue("ArduinoData", "No Data In");
            SetPropertyValue("DevModeOn", true);
            SetPropertyValue("UserModeOn", false);

            t = new TimersTimer();
            t.Interval = 10;
            t.Elapsed += t_Elapsed;
            t.Start();

            r = new Random();
            


        }

        void ConnectBot()
        {
            string ComPort = string.Empty;
            int i = 0;
            while (robotConn == Connection.Unknown)
            {
                string[] ports = SerialPort.GetPortNames();

                try
                {
                    serPort.PortName = ports[i];
                }
                catch (System.IndexOutOfRangeException e)
                {
                    if (System.Windows.MessageBox.Show("No robots found. Refresh?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        robotConn = Connection.Disconnected;
                    }
                }
                try
                {
                    serPort.Open();
                }
                catch (IOException e)
                {
                    i++;
                    continue;
                }
                if (serPort.IsOpen)
                {
                    robotConn = Connection.Connected;
                }
            }
            if (robotConn == Connection.Connected)
                SetPropertyValue("Connection", true);
            else
                SetPropertyValue("Connection", false);
        }

        void serPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
            int UltraSNum = GetPropertyValue("UltraSNum").ToInt32();
            int IRNum = GetPropertyValue("IRNum").ToInt32();
            
            signal = serPort.ReadLine(); // Receiving Arduino data as one string
            int startin = signal.IndexOf('<');
            int endin = signal.IndexOf('>');
            int msglngth = endin - startin;
            msg = signal.Substring(startin+1, msglngth-1);
            char key = msg[0];
            string value = msg.Substring(1);

            //Each different sensor type has its own key. This code takes in the key and sends the data to the proper module

            switch (key)
            {
                case 'H': // Hall Effect sensors
                    for (int i = 0; i < ArrayNum; i++ ) //Load serial data into hall array properties, each sensor is its own object
                    {
                        if (value[i] == '1')
                        {
                            SetPropertyValue("ArraySensor" + (i + 1).ToString(), true);
                        }
                        else if (value[i] == '0')
                        {
                            SetPropertyValue("ArraySensor" + (i + 1).ToString(), false);
                        }
                    }
                        break;
                case 'I': // Infrared Sensors
                        for (int i = 0; i < IRNum; i++)//Load serial data into infrared objects,  each sensor is its own object
                    {
                        if (value[i] == '1')
                        {
                            SetPropertyValue("IR" + (i + 1).ToString(), true);
                        }
                        else if (value[i] == '0')
                        {
                            SetPropertyValue("IR" + (i + 1).ToString(), false);
                        }
                    }
                    break;
                case 'B': // Impact Sensors
                    for (int i = 0; i < ImpactNum; i++)//Load serial data into impact sensor objects,  each sensor is its own object
                    {
                        if (value[i] == '1')
                        {
                            SetPropertyValue("Impact" + (i + 1).ToString(), true);
                        }
                        else if (value[i] == '0')
                        {
                            SetPropertyValue("Impact" + (i + 1).ToString(), false);
                        }
                    }
                    break;
                // Ultrasonic Sensors========================
                //Load serial data into ultrasonic objects,  each sensor is its own object and his its own key
                case 'J': SetPropertyValue("UltraS1", value); // saves the incoming value to the corresponding sensor object 
                        break;
                case 'K': SetPropertyValue("UltraS2", value);
                        break;
                case 'P': SetPropertyValue("UltraS3", value);
                        break;
                case 'V': SetPropertyValue("UltraS4", value);
                        break;
                case 'N': SetPropertyValue("UltraS5", value);
                        break;
                case 'M': SetPropertyValue("UltraS6", value);
                        break;
                // ==========================================
                // Motor Values =================================
                //Load serial data into motor objects,  each motor is its own object
                case 'L': SetPropertyValue("LeftMSpeed", value);
                        break;
                case 'R': SetPropertyValue("RightMSpeed", value);
                        break;
                // ==============================================
                case 'U': 
                    if (value == "1")
                    {
                        SetPropertyValue("DevModeOn", false);
                        SetPropertyValue("UserModeOn", true);
                    }
                    else if (value == "0")
                    {
                        SetPropertyValue("DevModeOn", true);
                        SetPropertyValue("UserModeOn", false);
                    }
                default: System.Windows.MessageBox.Show("Key " + key.ToString() + " does not exist!"); //Catch statement
                    break;

            }

        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (robotConn == Connection.Disconnected)
            {
                t.Interval = 500;
                SetPropertyValue("Generic_Sensor1", r.Next(0, 100));
                SetPropertyValue("Generic_Sensor2", r.Next(0, 100));
                SetPropertyValue("Generic_Sensor3", r.Next(0, 100));

                int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
                int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
                int UltraSNum = GetPropertyValue("UltraSNum").ToInt32();
                int IRNum = GetPropertyValue("IRNum").ToInt32();
                int line = r.Next(0, ArrayNum); // pick a random sensor to place line



                for (int i = 1; i <= ArrayNum; i++)
                {
                    if (i > line - 2 && i < line + 2)
                        SetPropertyValue("ArraySensor" + i.ToString(), true);
                    else
                        SetPropertyValue("ArraySensor" + i.ToString(), false);
                }
                for (int i = 1; i <= UltraSNum; i++)
                {
                    //SetPropertyValue("IR" + i.ToString(), r.NextDouble() * 100.0);
                    SetPropertyValue("UltraS" + i.ToString(), r.NextDouble() * 100.0);
                }

                if (r.Next(0, 100) < 50)
                {
                    SetPropertyValue("Impact" + r.Next(0, ImpactNum - 1).ToString(), true);
                    SetPropertyValue("IR" + r.Next(0, IRNum - 1).ToString(), true);
                }
                else
                {
                    for (int i = 1; i <= ImpactNum; i++)
                    {
                        SetPropertyValue("Impact" + i.ToString(), false);
                    }
                    for (int i = 1 ; i <= IRNum; i++)
                    {
                        SetPropertyValue("IR" + i.ToString(), false);
                    }
                }
            }
            if (robotConn == Connection.Connected)
            {
                if (prvr != GetPropertyValue("RightMSpeed").ToInt32() || prvl != GetPropertyValue("LeftMSpeed").ToInt32())
                {
                    serPort.WriteLine("<R" + GetPropertyValue("RightMSpeed").ToString() + "><L" + GetPropertyValue("LeftMSpeed").ToString() + ">");
                    prvr = GetPropertyValue("RightMSpeed").ToInt32();
                    prvl = GetPropertyValue("LeftMSpeed").ToInt32();
                }
                SetPropertyValue("ArduinoData", msg);
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
