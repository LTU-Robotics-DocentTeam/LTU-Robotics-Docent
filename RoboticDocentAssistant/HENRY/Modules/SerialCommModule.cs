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

        public Connection serConn1 = Connection.Unknown, serConn2 = Connection.Unknown;
        
        TimersTimer t;
        Random r;

        SerialPort serPort1; // Motor controller serial communication port
        SerialPort serPort2; // Sensor controller serial communication port (UltraS, Infrared, Hall Effect)
        string[] comPorts = new string[3];
        
        string signal = String.Empty, msg2motor = String.Empty, connectStatus = String.Empty;
        int deviceId = 0;
        int counter = 0; // Keeps track of loop. If it goes for too long without a response, show message to retry connection
        int simTimer = 0;

        int prvr = 0, prvl = 0;
        bool prestop = false;
        private string bBuff1 = String.Empty, bBuff2 = String.Empty;

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
            serPort1.ReadTimeout = 50;
            serPort1.DataReceived += new SerialDataReceivedEventHandler(serPort1_DataReceived);
            //====================================================================================================
            // These are all Serial Port 2 initializations
            //====================================================================================================
            serPort2 = new SerialPort();
            serPort2.BaudRate = 115200;
            serPort2.DataBits = 8;
            serPort2.Parity = Parity.None;
            serPort2.StopBits = StopBits.One;
            serPort2.ReadTimeout = 50;
            serPort2.DataReceived += new SerialDataReceivedEventHandler(serPort2_DataReceived);
            //====================================================================================================

            t = new TimersTimer();
            t.Interval = 10;
            t.Elapsed += t_Elapsed;

            ConnectBot(serPort1, "Motor MicroController", ref serConn1); // Function that handles robot connection initialization
            ConnectBot(serPort2, "Sensor MicroController", ref serConn2);
            //ConnectBot(userPort, "User Controller", ref userConn);

            UpdateConnectionStatus();

            SetPropertyValue("ArduinoData", "No Data In");
            SetPropertyValue("DevModeOn", true);
            SetPropertyValue("UserModeOn", false);
            SetPropertyValue("SimulationMode", false);
            SetPropertyValue("BatteryVoltage", 0.ToString());

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
            int attempts = 0;
            while (robotConn == Connection.Unknown)
            {
                if (attempts >= 4)
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
                        attempts++;
                        i = 0;
                        continue;
                        // Wanna try again?
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
                    if (counter % 10 == 0) serPort.Write("<C0>"); 
                    // Notify user of connecting procedure
                    //AutoClosingMessageBox.Show("Connecting to " + thisport + Environment.NewLine + "Attempt #" + counter.ToString(), "Connecting...", 2000);
                    // Wait for one second
                    //System.Threading.Thread.Sleep(2000);
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
                if (counter >= 5000) //connection timed out
                {
                    // refresh the list and try again
                    waiting = false;
                    counter = 0;
                    i++;
                    serPort.Close();
                    continue;
                    
                    // This code asks the user to refresh when serialPort times out
                    //if (System.Windows.MessageBox.Show(thisport + " timed out. Refresh?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    //{ // If not, set as disconnected
                    //    robotConn = Connection.Disconnected;
                    //    return;
                    //} // If yes, refresh the list and try again
                    //else
                    //{
                    //    waiting = false;
                    //    counter = 0;
                    //    i++;
                    //    serPort.Close();
                    //    continue;
                    //}
                }
                
            }
            
        }
        /// <summary>
        /// Builds updated Connection status string to be displayed in DevView
        /// </summary>
        void UpdateConnectionStatus()
        {
            // Check if serial ports are still open
            if (!serPort1.IsOpen && serConn1 != Connection.Disconnected)
            {
                serConn1 = Connection.Unknown;
                if (System.Windows.MessageBox.Show("serPort1 lost connection. Attempt reconnect?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ConnectBot(serPort1, "Motor MicroController", ref serConn1);
                }
                else
                {
                    serConn1 = Connection.Disconnected;
                }
            }
            if (!serPort2.IsOpen && serConn2 != Connection.Disconnected)
            {
                serConn2 = Connection.Unknown;
                if (System.Windows.MessageBox.Show("serPort2 lost connection. Attempt reconnect?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ConnectBot(serPort2, "Sensor MicroController", ref serConn2);
                }
                else
                {
                    serConn1 = Connection.Disconnected;
                }
            }
            
            // Build connection status string to be displayed
            connectStatus = "";
            connectStatus += "Motor Micro: ";
            if (serConn1 == Connection.Connected) { connectStatus += serPort1.PortName.ToString(); }
            else { connectStatus += "Disconnected"; }
            connectStatus += " Sensor Micro: ";
            if (serConn2 == Connection.Connected) { connectStatus += serPort2.PortName.ToString(); }
            else { connectStatus += "Disconnected"; }
            SetPropertyValue("Connection", connectStatus);
        }
        /// <summary>
        /// Function that fires everytime Serial Data is received from serPort1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Incoming messages should follow the format <K000>, where K is the key determining what sensor does
            // the data belongs to and 000 is the value for that sensor. The type of the value depends on the sensor
            
            // Read from seriaport up to next valid character

            string indata = serPort1.ReadExisting();

            foreach (char c in indata)
            {
                bBuff1 += c;

                if (c == '>')
                {
                    Console.WriteLine(bBuff1);
                    bBuff1 = String.Empty;
                }
            }

            //int msglngth = endin - startin;
            //string msg = signal.Substring(startin+1);
            

        }
        /// <summary>
        /// Parses incoming motor data. Reads through incoming
        /// messages and assigns values to their corresponding property based on the Key. Keys recognized: R, L, B, E, U(?)
        /// </summary>
        /// <param name="msg"> full incoming message in <K###> format</param>
        void serPort1_DataProcess(string msg)
        {
            char key = msg[0];
            string value = msg.Substring(1);

            //Each different sensor type has its own key. This code takes in the key and sends the data to the proper module

            switch (key)
            {
                case 'C': // Identification command: Takes device ID. This port only accepts "1", which is the motor microcontroller
                    if (value == "1" && serConn1 != Connection.Connected) deviceId = 1; // if correct id, set deviceid to 1 (meaning correct device is connected)
                    else deviceId = -1; // if incorrect id, set 
                    break;
                case 'B': // Impact Sensors: Binary string
                    for (int i = 0; i < Constants.IMPACT_NUM; i++)// Load serial data into impact sensor objects,  each sensor is its own object
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
                        prestop = true;
                        SetPropertyValue("EStop", true);
                    }
                    else if (value == "0")
                    {
                        prestop = false;
                        SetPropertyValue("EStop", false);
                    }
                    break;
                case 'P': // Left Brake signal: sent if the left brake has been triggered
                    if (value == "1")
                    {
                        SetPropertyValue("LeftBrake", true);
                    }
                    else if (value == "0")
                    {
                        SetPropertyValue("LeftBrake", false);
                    }
                    break;
                case 'Q': // Right Brake signal: sent if the right brake has been triggered
                    if (value == "1")
                    {
                        SetPropertyValue("RightBrake", true);
                    }
                    else if (value == "0")
                    {
                        SetPropertyValue("RightBrake", false);
                    }
                    break;
                default:
                    if (serConn1 != Connection.Unknown)
                        System.Windows.MessageBox.Show("Key " + key.ToString() + " is not recognized by serPort1"); //Catch statement
                    break;

            }
        }
        /// <summary>
        /// Function that fires everytime Serial Data is received from serPort2 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Incoming messages should follow the format <K000>, where K is the key determining what sensor does
            // the data belongs to and 000 is the value for that sensor. The type of the value depends on the sensor
            
            // Read from seriaport up to next valid character

            string indata = serPort2.ReadExisting();

            foreach (char c in indata)
            {
                bBuff2 += c;

                if (c == '>')
                {
                    Console.WriteLine(bBuff1);
                    bBuff2 = String.Empty;
                }
            }
            
        }
        /// <summary>
        /// Parses incoming sensor data. Reads through incoming
        /// messages and assigns values to their corresponding property based on the Key. Keys recognized: H, I, J, K, U(?)
        /// </summary>
        /// <param name="msg"> full incoming message in <K###> format</param>
        void serPort2_DataProcess(string msg)
        {
            char key = msg[0];
            string value = msg.Substring(1);

            //Each different sensor type has its own key. This code takes in the key and sends the data to the proper module

            switch (key)
            {
                case 'C': // Identification command: Takes device ID. This port only accepts "2", which is the sensor microcontroller
                    if (value == "2" && serConn2 != Connection.Connected) deviceId = 1; // if correct id, set deviceid to 1 (meaning correct device is connected)
                    else deviceId = -1; // if incorrect id, set 
                    break;
                case 'H': // Hall Effect sensors: Data comes as a binary string
                    for (int i = 0; i < Constants.ARRAY_NUM; i++) //Load serial data into hall array properties, each sensor is its own object
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
                    for (int i = 0; i < Constants.IR_NUM; i++) //Load serial data into infrared objects,  each sensor is its own object
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
                case 'V': SetPropertyValue("UltraS1", Int32.Parse(value)); // saves the incoming value to the corresponding sensor object 
                    break;
                case 'W': SetPropertyValue("UltraS2", Int32.Parse(value));
                    break;
                case 'X': SetPropertyValue("UltraS3", Int32.Parse(value));
                    break;
                case 'Y': SetPropertyValue("UltraS4", Int32.Parse(value));
                    break;
                case 'Z': SetPropertyValue("UltraS5", Int32.Parse(value));
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
                case 'S': // Battery voltage: Returns analog reading of current battery voltage
                    SetPropertyValue("BatteryVoltage", value);
                    break;
                default: 
                    if (serConn2 != Connection.Unknown) 
                        System.Windows.MessageBox.Show("Key " + key.ToString() + " is not recognized by serPort2"); //Catch statement
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
            // Calls function to update the connection status on the GUI
            UpdateConnectionStatus();

            // decrease simulation timer counter back to 0
            if (simTimer > 0) simTimer--;
            
            // If simulation mode is on, access random generation cases. These reset the simulation timers and generate random inputs
            // only access these cases if simTimer is down to 0 again.
            // This timer allows for slow random generation of inputs while maintaining a faster timer for sending serial messages
            if (simTimer <= 0)
            {
                // If sensor micro is disconnected and simulation mode is on, generate random inputs
                if (serConn2 == Connection.Disconnected && GetPropertyValue("SimulationMode").ToBoolean())
                {
                    // set timer for when sensor micro is disconnected
                    simTimer = 80;

                    int line = r.Next(0, Constants.ARRAY_NUM); // pick a random location in hall effect array to place line

                    // Set hall effect array states to true or false accordingly 
                    for (int i = 1; i <= Constants.ARRAY_NUM; i++)
                    {
                        if (i > line - 2 && i < line + 2)
                            SetPropertyValue("ArraySensor" + i.ToString(), true);
                        else
                            SetPropertyValue("ArraySensor" + i.ToString(), false);
                    }

                    // Generate random valid inputs for the ultrasonic simulation
                    for (int i = 1; i <= Constants.US_NUM; i++)
                    {
                        SetPropertyValue("UltraS" + i.ToString(), 1000 + r.Next() * 1000);
                    }

                    // trigger random infrared sensor in array
                    if (r.Next(0, 100) < 50)
                    {
                        SetPropertyValue("IR" + r.Next(0, Constants.IR_NUM - 1).ToString(), true);
                    }
                    else
                    {
                        for (int i = 1; i <= Constants.IR_NUM; i++)
                        {
                            SetPropertyValue("IR" + i.ToString(), false);
                        }
                    }
                }
                // If motor micro is disconnected and simulation mode is on, generate random inputs
                if (serConn1 == Connection.Disconnected && GetPropertyValue("SimulationMode").ToBoolean())
                {
                    // set timer for when motor micro is disconnected
                    simTimer = 80;

                    // trigger random impact sensor in array
                    if (r.Next(0, 100) < 50)
                    {
                        SetPropertyValue("Impact" + r.Next(0, Constants.IMPACT_NUM - 1).ToString(), true);
                    }
                    else
                    {
                        for (int i = 1; i <= Constants.IMPACT_NUM; i++)
                        {
                            SetPropertyValue("Impact" + i.ToString(), false);
                        }
                    }
                }
            }
            
            // If motor micro is connected and simulation mode is OFF, send data to arduino
            if (serConn1 == Connection.Connected && !GetPropertyValue("SimulationMode").ToBoolean())
            {
                // Check for data to send to the arduino
                msg2motor = "";
                // Only send motor data if it has changed from last message sent
                if (prvr != GetPropertyValue("RightMSpeed").ToInt32() || prvl != GetPropertyValue("LeftMSpeed").ToInt32())
                {
                    msg2motor = "<R" + GetPropertyValue("RightMSpeed").ToString() + "><L" + GetPropertyValue("LeftMSpeed").ToString() + ">";
                    prvr = GetPropertyValue("RightMSpeed").ToInt32();
                    prvl = GetPropertyValue("LeftMSpeed").ToInt32();
                }
                // if estop triggered before, send attempt reset signal when user addresses in the program (i.e. checkbox in DevView, 
                // alert message from UserView)
                if (prestop != GetPropertyValue("EStop").ToBoolean() && prestop)
                {
                    msg2motor = "<F" + GetPropertyValue("EStop").ToInt32() + ">";
                    prestop = GetPropertyValue("EStop").ToBoolean();
                }
                
                // if message is not empty, send message through serial port
                if (msg2motor != "")
                {
                    serPort1.Write(msg2motor);
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
