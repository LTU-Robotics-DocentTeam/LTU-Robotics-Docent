﻿using HENRY.ModuleSystem;
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
        
        string signal = ""; // signal from the arduino. Perhaos pack all data as one long string, and then parse it?
        int prvr = 0, prvl = 0;

        public SerialCommModule()
        {
            // These are all Serial Port initializations. Exception handling allows for running without having to comment out
            //====================================================================================================
            robotConn = Connection.Unknown;
            serPort = new SerialPort();
            serPort.BaudRate = 9600;
            serPort.DataBits = 8;
            serPort.Parity = Parity.None;
            serPort.StopBits = StopBits.One;
            serPort.DataReceived += new SerialDataReceivedEventHandler(serPort_DataReceived);
            //====================================================================================================

            ConnectBot();

            t = new TimersTimer();
            t.Interval = 50;
            t.Elapsed += t_Elapsed;
            t.Start();

            r = new Random();
            SetPropertyValue("ArduinoData", "No Data In");
            SetPropertyValue("DevModeOn", true);


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
            signal = serPort.ReadLine(); // Receiving Arduino data as one string
            int startin = signal.IndexOf('<');
            int endin = signal.IndexOf('>');
            int msglngth = endin - startin;
            string msg = signal.Substring(startin+1, msglngth);
            char key = msg[0];
            string value = msg.Substring(1);

            switch (key)
            {
                case 'H': 
                    for (int i = 0; i < GetPropertyValue("ArrayNum").ToInt32(); i++ )
                    {
                        SetPropertyValue("ArrayNum" + (i + 1).ToString(), value[i]);
                    }
                        break;
                case 'J': SetPropertyValue("UltraS1", value);
                        break;
                default: System.Windows.MessageBox.Show("Key " + key.ToString() + " does not exist!");
                    break;

            }

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
            if (robotConn == Connection.Disconnected)
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
                    SetPropertyValue("IR" + i.ToString(), r.NextDouble() * 100.0);
                    SetPropertyValue("UltraS" + i.ToString(), r.NextDouble() * 100.0);
                }

                if (r.Next(0, 100) < 50)
                {
                    SetPropertyValue("Impact" + r.Next(0, ImpactNum - 1).ToString(), true);
                }
                else
                {
                    for (int i = 1; i <= ImpactNum; i++)
                    {
                        SetPropertyValue("Impact" + i.ToString(), false);
                    }
                }
            }
            if (robotConn == Connection.Connected)
            {
                if (prvr != GetPropertyValue("RightMSpeed").ToInt32() || prvl != GetPropertyValue("LeftMSpeed").ToInt32())
                {
                    serPort.WriteLine("R:" + GetPropertyValue("RightMSpeed").ToString() + "L:" + GetPropertyValue("LeftMSpeed").ToString());
                    prvr = GetPropertyValue("RightMSpeed").ToInt32();
                    prvl = GetPropertyValue("LeftMSpeed").ToInt32();
                }


                    
                

                
                //System.Windows.MessageBox.Show("Sending");
                //SetPropertyValue("ArduinoData", signal);
                //if (GetPropertyValue("Forward").ToBoolean())
                //{
                //    serPort.Write("A\n");
                //}
                //else if (GetPropertyValue("Backward").ToBoolean())
                //{
                //    serPort.Write("B\n");
                //}
                //else if (GetPropertyValue("Right").ToBoolean())
                //{
                //    serPort.Write("C\n");
                //}
                //else if (GetPropertyValue("Left").ToBoolean())
                //{
                //    serPort.Write("D\n");
                //}
                //else
                //{
                //    serPort.Write("S\n");
                //}
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
