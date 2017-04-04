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
    /// <summary>
    /// Handles all serial communications with robot controllers and manual controller.
    /// </summary>
    /// TO DO:
    /// - Add second micro and manual controller
    /// - Make more responsive (lower/eliminate delay)
    
    class SerialCommModule : LengarioModuleCore
    {
        public enum Connection { Unknown, Disconnected, Connected};

        public Connection serConn1 = Connection.Unknown, serConn2 = Connection.Unknown, userConn = Connection.Unknown;
        
        TimersTimer t;
        Random r;

        SerialPort serPort1; // Motor controller serial communication port
        SerialPort serPort2; // Sensor controller serial communication port (UltraS, Infrared, Hall Effect)
        SerialPort userPort; // User controller serial communication port
        string[] comPorts = new string[3];
        
        string signal = "", msg2motor = "", connectStatus = "";
        int deviceId = 0;
        int counter = 0; // Keeps track of loop. If it goes for too long without a response, show message to retry connection

        int prvr = 0, prvl = 0;
        bool prestop = false;

        public SerialCommModule()
        {
            //robotConn = Connection.Unknown;
            // These are all Serial Port 1 initializations
            //====================================================================================================
            serPort1 = new SerialPort();
            serPort1.BaudRate = 115200;
            serPort1.DataBits = 8;
            serPort1.Parity = Parity.None;
            serPort1.StopBits = StopBits.One;
            serPort1.DataReceived += new SerialDataReceivedEventHandler(serPort1_DataReceived);
            //====================================================================================================
            // These are all Serial Port 2 initializations
            //====================================================================================================
            serPort2 = new SerialPort();
            serPort2.BaudRate = 115200;
            serPort2.DataBits = 8;
            serPort2.Parity = Parity.None;
            serPort2.StopBits = StopBits.One;
            serPort2.DataReceived += new SerialDataReceivedEventHandler(serPort2_DataReceived);
            //====================================================================================================
            // These are all UserController Serial Port initializations
            //====================================================================================================
            userPort = new SerialPort();
            userPort.BaudRate = 115200;
            userPort.DataBits = 8;
            userPort.Parity = Parity.None;
            userPort.StopBits = StopBits.One;
            userPort.DataReceived += new SerialDataReceivedEventHandler(userPort_DataReceived);
            //====================================================================================================

            t = new TimersTimer();
            t.Interval = 10;
            t.Elapsed += t_Elapsed;

            ConnectBot(serPort1, "Motor MicroController", ref serConn1); // Function that handles robot connection initialization
            ConnectBot(serPort2, "Sensor MicroController", ref serConn2);
            ConnectBot(userPort, "User Controller", ref userConn);

            UpdateConnectionStatus();

            SetPropertyValue("ArduinoData", "No Data In");
            SetPropertyValue("DevModeOn", true);
            SetPropertyValue("UserModeOn", false);
            SetPropertyValue("SimulationMode", false);

            t.Start();

            r = new Random();
        }



        

        /// <summary>
        /// Handles the process of connecting to the microcontrollers and manual control
        /// </summary>
        /// TO-DO:
        /// - Make more modular
        void ConnectBot(SerialPort serPort, string thisport, ref Connection robotConn)
        {
            int i = 0;
            bool waiting = false;
            counter = 0; // Keeps track of loop. If it goes for too long without a response, show message to retry connection
            while (robotConn == Connection.Unknown)
            {
                if (!waiting)
                {
                    //counter++;
                    string[] ports = SerialPort.GetPortNames();

                    try
                    {
                        serPort.PortName = ports[i]; // assign port to try
                    }
                    catch (System.IndexOutOfRangeException e) // Ran out of ports to test
                    {
                        // Wanna try again?
                        if (System.Windows.MessageBox.Show(thisport + " not found. Refresh?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        { // If not, set as disconnected
                            robotConn = Connection.Disconnected;
                            return;
                        } // If yes, refresh the list and try again
                        else
                        {
                            i = 0;
                            continue;
                        }
                    }
                    catch (System.InvalidOperationException e) // Port invalid
                    {
                        // try another one
                        i++;
                        continue;
                    }
                    try
                    {
                        serPort.Open(); // try to open port
                    }
                    catch (Exception e)
                    {
                        // if port is already open, access to port is denied, or no port assigned...
                        if (e is IOException || e is System.InvalidOperationException || e is System.UnauthorizedAccessException)
                        {
                            // try the next port
                            i++;
                            continue;
                        }
                        // else throw unhandled exception
                        throw;
                    }
                }
                
                if (serPort.IsOpen) // If an available port was found, attempt to communicate and identify connected device
                {
                    waiting = true;
                    counter++;
                    // Send identification command to device
                    serPort.WriteLine("<C0>"); 
                    // Notify user of connecting procedure
                    AutoClosingMessageBox.Show("Connecting to " + thisport + Environment.NewLine + "Attempt #" + counter.ToString(), "Connecting...", 1000);
                    // Wait for one second
                    System.Threading.Thread.Sleep(1000);
                    // Check deviceid global variable, which the DataReceived functions will update when necessary
                    if (deviceId == 1) // if its 1, then device is valid.
                    { 
                        // Update connection status and reset connection variables to default state
                        robotConn = Connection.Connected;
                        deviceId = 0;
                        signal = "";
                        return;
                    }
                    else if (deviceId == -1) // if its -1, device is invalid
                    {
                        // Reset variables and waiting time, close current port, and try next port
                        i++;
                        waiting = false;
                        deviceId = 0;
                        signal = "";
                        serPort.Close();
                        continue;
                    }
                }
                if (counter >= 3) //connection timed out. Wanna try again?
                {
                    if (System.Windows.MessageBox.Show(thisport + " timed out. Refresh?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    { // If not, set as disconnected
                        robotConn = Connection.Disconnected;
                        return;
                    } // If yes, refresh the list and try again
                    else
                    {
                        waiting = false;
                        counter = 0;
                        i = 0;
                        continue;
                    }
                }
            }
            
        }
        /// <summary>
        /// Builds updated Connection status string to be displayed in DevView
        /// </summary>
        void UpdateConnectionStatus()
        {
            // Build connection status string to be displayed
            connectStatus = "";
            connectStatus += "Motor Micro: ";
            if (serConn1 == Connection.Connected) { connectStatus += serPort1.PortName.ToString(); }
            else { connectStatus += "Disconnected"; }
            connectStatus += " Sensor Micro: ";
            if (serConn2 == Connection.Connected) { connectStatus += serPort2.PortName.ToString(); }
            else { connectStatus += "Disconnected"; }
            connectStatus += " User Controller: ";
            if (userConn == Connection.Connected) { connectStatus += userPort.PortName.ToString(); }
            else { connectStatus += "Disconnected"; }
            SetPropertyValue("Connection", connectStatus);
        }
        /// <summary>
        /// Function that fires everytime Serial Data is received from serPort1 and parses the incoming data. Reads through incoming
        /// messages and assigns values to their corresponding property based on the Key. Keys recognized: R, L, B, E, U(?)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Incoming messages should follow the format <K000>, where K is the key determining what sensor does
            // the data belongs to and 000 is the value for that sensor. The type of the value depends on the sensor
            int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
            int UltraSNum = GetPropertyValue("UltraSNum").ToInt32();
            int IRNum = GetPropertyValue("IRNum").ToInt32();
            
            signal = serPort1.ReadLine(); // Receiving Arduino data as one string
            int startin = signal.IndexOf('<');
            int endin = signal.IndexOf('>');
            if (startin < 0 || endin < 0)
            {
                return;
            }
            int msglngth = endin - startin;
            string msg1 = signal.Substring(startin+1, msglngth-1);
            char key = msg1[0];
            string value = msg1.Substring(1);

            //Each different sensor type has its own key. This code takes in the key and sends the data to the proper module

            switch (key)
            {
                case 'C': // Identification command: Takes device ID. This port only accepts "1", which is the motor microcontroller
                    if (value == "1") deviceId = 1; // if correct id, set deviceid to 1 (meaning correct device is connected)
                    else deviceId = -1; // if incorrect id, set 
                    break;
                case 'B': // Impact Sensors: Binary string
                    for (int i = 0; i < ImpactNum; i++)// Load serial data into impact sensor objects,  each sensor is its own object
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
                // Motor Values =================================
                // Load serial data into motor objects,  each motor is its own object. Data is a double
                case 'L': SetPropertyValue("LeftMSpeed", value);
                        break;
                case 'R': SetPropertyValue("RightMSpeed", value);
                        break;
                // ==============================================
                case 'U': // User View ON?: Determines which View is active at the moment
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
                    break;
                case 'E': // E-Stop signal: sent if E-Stop was triggered.
                    if (value == "1")
                    {
                        SetPropertyValue("EStop", true);
                    }
                    else if (value == "0")
                    {
                        SetPropertyValue("EStop", false);
                    }
                    break;
                default: System.Windows.MessageBox.Show("Key " + key.ToString() + " is not recognized by serPort1"); //Catch statement
                    break;

            }

        }
        /// <summary>
        /// Function that fires everytime Serial Data is received from serPort2 and parses the incoming data. Reads through incoming
        /// messages and assigns values to their corresponding property based on the Key. Keys recognized: H, I, J, K, U(?)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Incoming messages should follow the format <K000>, where K is the key determining what sensor does
            // the data belongs to and 000 is the value for that sensor. The type of the value depends on the sensor
            int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
            int UltraSNum = GetPropertyValue("UltraSNum").ToInt32();
            int IRNum = GetPropertyValue("IRNum").ToInt32();

            signal = serPort2.ReadLine(); // Receiving Arduino data as one string
            int startin = signal.IndexOf('<');
            int endin = signal.IndexOf('>');
            if (startin < 0 || endin < 0)
            {
                return;
            }
            int msglngth = endin - startin;
            string msg2 = signal.Substring(startin + 1, msglngth - 1);
            char key = msg2[0];
            string value = msg2.Substring(1);

            //Each different sensor type has its own key. This code takes in the key and sends the data to the proper module

            switch (key)
            {
                case 'C': // Identification command: Takes device ID. This port only accepts "2", which is the sensor microcontroller
                    if (value == "2") deviceId = 1; // if correct id, set deviceid to 1 (meaning correct device is connected)
                    else deviceId = -1; // if incorrect id, set 
                    break;
                case 'H': // Hall Effect sensors: Data comes as a binary string
                    for (int i = 0; i < ArrayNum; i++) //Load serial data into hall array properties, each sensor is its own object
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
                case 'I': // Infrared Sensors: Binary string
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
                // Ultrasonic Sensors========================
                //Load serial data into ultrasonic objects,  each sensor is its own object and his its own key. Data is a double
                case 'V': SetPropertyValue("UltraS1", value); // saves the incoming value to the corresponding sensor object 
                    break;
                case 'W': SetPropertyValue("UltraS2", value);
                    break;
                case 'X': SetPropertyValue("UltraS3", value);
                    break;
                case 'Y': SetPropertyValue("UltraS4", value);
                    break;
                case 'Z': SetPropertyValue("UltraS5", value);
                    break;
                // ==========================================
                case 'U': // User View ON?: Determines which View is active at the moment
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
                    break;
                default: System.Windows.MessageBox.Show("Key " + key.ToString() + " is not recognized by serPort2"); //Catch statement
                    break;

            }

        }
        private void userPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Incoming messages should follow the format <K000>, where K is the key determining what sensor does
            // the data belongs to and 000 is the value for that sensor. The type of the value depends on the sensor
            int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
            int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
            int UltraSNum = GetPropertyValue("UltraSNum").ToInt32();
            int IRNum = GetPropertyValue("IRNum").ToInt32();

            signal = userPort.ReadLine(); // Receiving Arduino data as one string
            int startin = signal.IndexOf('<');
            int endin = signal.IndexOf('>');
            if (startin < 0 || endin < 0)
            {
                return;
            }
            int msglngth = endin - startin;
            string msg3 = signal.Substring(startin + 1, msglngth - 1);
            char key = msg3[0];
            string value = msg3.Substring(1);

            //Each different sensor type has its own key. This code takes in the key and sends the data to the proper module

            switch (key)
            {
                case 'C': // Identification command: Takes device ID. This port only accepts "2", which is the sensor microcontroller
                    if (value == "3") deviceId = 1; // if correct id, set deviceid to 1 (meaning correct device is connected)
                    else deviceId = -1; // if incorrect id, set 
                    break;
                default: System.Windows.MessageBox.Show("Key " + key.ToString() + " is not recognized by userPort"); //Catch statement
                    break;
            }
        }
        /// <summary>
        /// Handles all data sending on a timer if the robot is connected, else it generates random data
        /// for visualization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// TO DO:
        /// - Figure out how to have simulation mode run with motor communications without slowing down stream. Some sort of counter?
        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateConnectionStatus();
            // If sensor micro is disconnected and simulation mode is on, generate random inputs
            if (serConn2 == Connection.Disconnected && GetPropertyValue("SimulationMode").ToBoolean())
            {
                t.Interval = 800;

                int ArrayNum = GetPropertyValue("ArrayNum").ToInt32();
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
                    SetPropertyValue("UltraS" + i.ToString(), 1000.0 + r.NextDouble() * 1000.0);
                }

                if (r.Next(0, 100) < 50)
                {
                    SetPropertyValue("IR" + r.Next(0, IRNum - 1).ToString(), true);
                }
                else
                {
                    for (int i = 1 ; i <= IRNum; i++)
                    {
                        SetPropertyValue("IR" + i.ToString(), false);
                    }
                }
            }
            // If motor micro is disconnected and simulation mode is on, generate random ass inputs
            if (serConn1 == Connection.Disconnected && GetPropertyValue("SimulationMode").ToBoolean())
            {
                t.Interval = 800;

                int ImpactNum = GetPropertyValue("ImpactNum").ToInt32();
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
            // If motor micro is connected and simulation mode is OFF, send data to arduino
            if (serConn1 == Connection.Connected && !GetPropertyValue("SimulationMode").ToBoolean())
            {
                t.Interval = 1;
                if (prvr != GetPropertyValue("RightMSpeed").ToInt32() || prvl != GetPropertyValue("LeftMSpeed").ToInt32())
                {
                    msg2motor = "<R" + GetPropertyValue("RightMSpeed").ToString() + "><L" + GetPropertyValue("LeftMSpeed").ToString() + ">";
                    prvr = GetPropertyValue("RightMSpeed").ToInt32();
                    prvl = GetPropertyValue("LeftMSpeed").ToInt32();
                }
                if (prestop != GetPropertyValue("EStop").ToBoolean())
                {
                    msg2motor = "<F" + GetPropertyValue("EStop").ToInt32() + ">"; // Attempt to reset E-Stop on board. Does not override physical button
                    prestop = GetPropertyValue("EStop").ToBoolean();
                }
                
                if (msg2motor != "")
                {
                    serPort1.WriteLine(msg2motor);
                    SetPropertyValue("ArduinoData", msg2motor);
                    msg2motor = "";
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
