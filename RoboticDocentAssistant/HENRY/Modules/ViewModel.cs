using HENRY.ModuleSystem;
using System.ComponentModel;

namespace HENRY.Modules
{
    /// <summary>
    /// Handles all communications between the Model (data processing modules) and View (User and Developer)
    /// </summary>
    class ViewModel : LengarioModuleCore, INotifyPropertyChanged
    {
        
        //==================================== Constructor and Support =========================================

        public ViewModel()
        {
            /*
             * PropertyUpdated is a static event owned
             * by the ModuleBase. It is called when any
             * property in the ModuleSystem is changed.
             * Yes, any. Please handle with care. Only
             * subscribe to this event once please.
             * Powers outside of my control, and whatnot
             */

            PropertyUpdated += ViewModel_PropertyUpdated;
        }

        void ViewModel_PropertyUpdated(object sender, PropertyUpdateArgs e)
        {
            //PropertyUpdated basically just triggers the PropertyChanged
            //event and updates the appropriate binding. 'e' contains the
            //name of the property that gets updated. (sent from ModuleBase)

            RaisePropertyChanged(e.PropertyUpdated);
        }




        //============================= Inheritance and Interface Implementation ===============================


        //Method required by ModuleSystem to identify module in respect to system
        //Required for every module

        public override string GetModuleName()
        {
            return "ViewModel";
        }



        //Basic INotifyPropertyChanged interface implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }



        //================================= Properties for Use by the Views ====================================
        
        //There will be a ton here, but they are easy to copy and paste

        public bool DevModeOn
        {
            get { return GetPropertyValue("DevModeOn").ToBoolean(); }
            set { SetPropertyValue("DevModeOn", value); RaisePropertyChanged("DevModeOn"); }
        }

        public bool UserModeOn
        {
            get { return GetPropertyValue("UserModeOn").ToBoolean(); }
            set { SetPropertyValue("UserModeOn", value); RaisePropertyChanged("UserModeOn"); }
        }

        public bool SimulationMode
        {
            get { return GetPropertyValue("SimulationMode").ToBoolean(); }
            set { SetPropertyValue("SimulationMode", value); RaisePropertyChanged("SimulationMode"); }
        }
        
        public string Connection
        {
            get { return GetPropertyValue("Connection").ToString(); }
            set { SetPropertyValue("Connection", value); RaisePropertyChanged("Connection"); }
        }

        public string ArduinoData
        {
            get { return GetPropertyValue("ArduinoData").ToString(); }
            set { SetPropertyValue("ArduinoData", value); RaisePropertyChanged("ArduinoData"); }
        }

        public int Generic_Sensor1
        {
            get
            {
                /* 
                 * GetPropertyValue method is inherited from the ModuleSystem
                 * It can get any property from the ModuleSystem, provided that
                 * the module owns the data. (or is a CoreModule) The method
                 * returns a LengarioModuleProperty object, which can be cast
                 * to different types. Don't mix and match types! Bad times.
                 * String, Char, DateTime, Int32, and Boolean are available now.
                 */

                return GetPropertyValue("Generic_Sensor1").ToInt32();
            }
            set
            {
                /*
                 * SetPropertyValue method is inherited from the ModuleSystem
                 * as well. Same logic here. We also RaisePropertyChanged in
                 * the 'set' as well. This 'set' is usually only called if you
                 * have bound an input to the property.
                 */

                SetPropertyValue("Generic_Sensor1", value);
                RaisePropertyChanged("Generic_Sensor1");
            }
        }

        public int Generic_Sensor2
        {
            get
            {
                return GetPropertyValue("Generic_Sensor2").ToInt32();
            }
            set
            {
                SetPropertyValue("Generic_Sensor2", value);
                RaisePropertyChanged("Generic_Sensor2");
            }
        }

        public int Generic_Sensor3
        {
            get
            {
                return GetPropertyValue("Generic_Sensor3").ToInt32();
            }
            set
            {
                SetPropertyValue("Generic_Sensor3", value);
                RaisePropertyChanged("Generic_Sensor3");
            }
        }

        
        public int Generic_Output1
        {
            get
            {
                return GetPropertyValue("Generic_Output1").ToInt32();
            }
            set
            {
                SetPropertyValue("Generic_Output1", value);
                RaisePropertyChanged("Generic_Output1");
            }
        }

        public bool Generic_Multiply
        {
            get
            {
                return GetPropertyValue("Generic_Multiply").ToBoolean();
            }
            set
            {
                SetPropertyValue("Generic_Multiply", value);
                RaisePropertyChanged("Generic_Multiply");
            }
        }

        public bool ManualDriveEnabled
        {
            get
            {
                return GetPropertyValue("ManualDriveEnabled").ToBoolean();
            }
            set
            {
                SetPropertyValue("ManualDriveEnabled", value);
                RaisePropertyChanged("ManualDriveEnabled");
            }
        }

        public int ManualSpeed
        {
            get { return GetPropertyValue("ManualSpeed").ToInt32(); }
            set { SetPropertyValue("ManualSpeed", value); RaisePropertyChanged("ManualSpeed"); }
        }

        public bool Forward
        {
            get { return GetPropertyValue("Forward").ToBoolean(); }
            set { SetPropertyValue("Forward", value); RaisePropertyChanged("Forward"); }
        }

        public bool Backward
        {
            get { return GetPropertyValue("Backward").ToBoolean(); }
            set { SetPropertyValue("Backward", value); RaisePropertyChanged("Backward"); }
        }

        public bool Right
        {
            get { return GetPropertyValue("Right").ToBoolean(); }
            set { SetPropertyValue("Right", value); RaisePropertyChanged("Right"); }
        }

        public bool Left
        {
            get { return GetPropertyValue("Left").ToBoolean(); }
            set { SetPropertyValue("Left", value); RaisePropertyChanged("Left"); }
        }

        public bool RightBrake
        {
            get { return GetPropertyValue("RightBrake").ToBoolean(); }
            set { SetPropertyValue("RightBrake", value); RaisePropertyChanged("RightBrake"); }
        }

        public bool LeftBrake
        {
            get { return GetPropertyValue("LeftBrake").ToBoolean(); }
            set { SetPropertyValue("LeftBrake", value); RaisePropertyChanged("LeftBrake"); }
        }
        public string RightMSpeed
        {
            get { return GetPropertyValue("RightMSpeed").ToString(); }
            set { SetPropertyValue("RightMSpeed", value); RaisePropertyChanged("RightMSpeed"); }
        }
        public string LeftMSpeed
        {
            get { return GetPropertyValue("LeftMSpeed").ToString(); }
            set { SetPropertyValue("LeftMSpeed", value); RaisePropertyChanged("LeftMSpeed"); }
        }
        public string Direction
        {
            get { return GetPropertyValue("Direction").ToString(); }
            set { SetPropertyValue("Direction", value); RaisePropertyChanged("Direction"); }
        }
        public string Speed
        {
            get { return GetPropertyValue("Speed").ToString(); }
            set { SetPropertyValue("Speed", value); RaisePropertyChanged("Speed"); }
        }
        public bool AutonomousNavigation
        {
            get { return GetPropertyValue("AutonomousNavigation").ToBoolean(); }
            set { SetPropertyValue("AutonomousNavigation", value); RaisePropertyChanged("AutonomousNavigation"); }
        }
        public string DirectionalSpeed
        {
            get { return GetPropertyValue("DirectionalSpeed").ToString(); }
            set { SetPropertyValue("DirectionalSpeed", value); RaisePropertyChanged("DirectionalSpeed"); }
        }
        public string BatteryVoltage
        {
            get { return GetPropertyValue("BatteryVoltage").ToString(); }
            set { SetPropertyValue("BatteryVoltage", value); RaisePropertyChanged("BatteryVoltage"); }
        }
        public bool ArraySensor1
        {
            get { return GetPropertyValue("ArraySensor1").ToBoolean(); }
            set { SetPropertyValue("ArraySensor1", value); RaisePropertyChanged("ArraySensor1"); }
        }
        public bool ArraySensor2
        {
            get { return GetPropertyValue("ArraySensor2").ToBoolean(); }
            set { SetPropertyValue("ArraySensor2", value); RaisePropertyChanged("ArraySensor2"); }
        }
        public bool ArraySensor3
        {
            get { return GetPropertyValue("ArraySensor3").ToBoolean(); }
            set { SetPropertyValue("ArraySensor3", value); RaisePropertyChanged("ArraySensor3"); }
        }
        public bool ArraySensor4
        {
            get { return GetPropertyValue("ArraySensor4").ToBoolean(); }
            set { SetPropertyValue("ArraySensor4", value); RaisePropertyChanged("ArraySensor4"); }
        }
        public bool ArraySensor5
        {
            get { return GetPropertyValue("ArraySensor5").ToBoolean(); }
            set { SetPropertyValue("ArraySensor5", value); RaisePropertyChanged("ArraySensor5"); }
        }
        public bool ArraySensor6
        {
            get { return GetPropertyValue("ArraySensor6").ToBoolean(); }
            set { SetPropertyValue("ArraySensor6", value); RaisePropertyChanged("ArraySensor6"); }
        }
        public bool ArraySensor7
        {
            get { return GetPropertyValue("ArraySensor7").ToBoolean(); }
            set { SetPropertyValue("ArraySensor7", value); RaisePropertyChanged("ArraySensor7"); }
        }
        public bool ArraySensor8
        {
            get { return GetPropertyValue("ArraySensor8").ToBoolean(); }
            set { SetPropertyValue("ArraySensor8", value); RaisePropertyChanged("ArraySensor8"); }
        }
        public bool ArraySensor9
        {
            get { return GetPropertyValue("ArraySensor9").ToBoolean(); }
            set { SetPropertyValue("ArraySensor9", value); RaisePropertyChanged("ArraySensor9"); }
        }
        public bool ArraySensor10
        {
            get { return GetPropertyValue("ArraySensor10").ToBoolean(); }
            set { SetPropertyValue("ArraySensor10", value); RaisePropertyChanged("ArraySensor10"); }
        }
        public bool ArraySensor11
        {
            get { return GetPropertyValue("ArraySensor11").ToBoolean(); }
            set { SetPropertyValue("ArraySensor11", value); RaisePropertyChanged("ArraySensor11"); }
        }
        public bool ArraySensor12
        {
            get { return GetPropertyValue("ArraySensor12").ToBoolean(); }
            set { SetPropertyValue("ArraySensor12", value); RaisePropertyChanged("ArraySensor12"); }
        }
        public bool ArraySensor13
        {
            get { return GetPropertyValue("ArraySensor13").ToBoolean(); }
            set { SetPropertyValue("ArraySensor13", value); RaisePropertyChanged("ArraySensor13"); }
        }
        public bool ArraySensor14
        {
            get { return GetPropertyValue("ArraySensor14").ToBoolean(); }
            set { SetPropertyValue("ArraySensor14", value); RaisePropertyChanged("ArraySensor14"); }
        }
        public bool ArraySensor15
        {
            get { return GetPropertyValue("ArraySensor15").ToBoolean(); }
            set { SetPropertyValue("ArraySensor15", value); RaisePropertyChanged("ArraySensor15"); }
        }
        public bool ArraySensor16
        {
            get { return GetPropertyValue("ArraySensor16").ToBoolean(); }
            set { SetPropertyValue("ArraySensor16", value); RaisePropertyChanged("ArraySensor16"); }
        }
        public bool ArraySensor17
        {
            get { return GetPropertyValue("ArraySensor17").ToBoolean(); }
            set { SetPropertyValue("ArraySensor17", value); RaisePropertyChanged("ArraySensor17"); }
        }
        public bool ArraySensor18
        {
            get { return GetPropertyValue("ArraySensor18").ToBoolean(); }
            set { SetPropertyValue("ArraySensor18", value); RaisePropertyChanged("ArraySensor18"); }
        }
        public bool ArraySensor19
        {
            get { return GetPropertyValue("ArraySensor19").ToBoolean(); }
            set { SetPropertyValue("ArraySensor19", value); RaisePropertyChanged("ArraySensor19"); }
        }
        public bool ArraySensor20
        {
            get { return GetPropertyValue("ArraySensor20").ToBoolean(); }
            set { SetPropertyValue("ArraySensor20", value); RaisePropertyChanged("ArraySensor20"); }
        }
        public double LineAngle
        {
            get { return System.Math.Round(GetPropertyValue("LineAngle").ToDouble(), 1); }
            set { SetPropertyValue("LineAngle", value); RaisePropertyChanged("LineAngle"); }
        }

        public double UltraS1
        {
            get { return System.Math.Round(GetPropertyValue("UltraS1").ToDouble(), 1); }
            set { SetPropertyValue("UltraS1", value); RaisePropertyChanged("UltraS1"); }
        }
        public double UltraS2
        {
            get { return System.Math.Round(GetPropertyValue("UltraS2").ToDouble(), 1); }
            set { SetPropertyValue("UltraS2", value); RaisePropertyChanged("UltraS2"); }
        }
        public double UltraS3
        {
            get { return System.Math.Round(GetPropertyValue("UltraS3").ToDouble(), 1); }
            set { SetPropertyValue("UltraS3", value); RaisePropertyChanged("UltraS3"); }
        }
        public double UltraS4
        {
            get { return System.Math.Round(GetPropertyValue("UltraS4").ToDouble(), 1); }
            set { SetPropertyValue("UltraS4", value); RaisePropertyChanged("UltraS4"); }
        }
        public double UltraS5
        {
            get { return System.Math.Round(GetPropertyValue("UltraS5").ToDouble(), 1); }
            set { SetPropertyValue("UltraS5", value); RaisePropertyChanged("UltraS5"); }
        }
        public double UltraS6
        {
            get { return System.Math.Round(GetPropertyValue("UltraS6").ToDouble(), 1); }
            set { SetPropertyValue("UltraS6", value); RaisePropertyChanged("UltraS6"); }
        }

        public bool IR1
        {
            get { return GetPropertyValue("IR1").ToBoolean(); }
            set { SetPropertyValue("IR1", value); RaisePropertyChanged("IR1"); }
        }
        public bool IR2
        {
            get { return GetPropertyValue("IR2").ToBoolean(); }
            set { SetPropertyValue("IR2", value); RaisePropertyChanged("IR2"); }
        }
        public bool IR3
        {
            get { return GetPropertyValue("IR3").ToBoolean(); }
            set { SetPropertyValue("IR3", value); RaisePropertyChanged("IR3"); }
        }
        public bool IR4
        {
            get { return GetPropertyValue("IR4").ToBoolean(); }
            set { SetPropertyValue("IR4", value); RaisePropertyChanged("IR4"); }
        }
        public bool IR5
        {
            get { return GetPropertyValue("IR5").ToBoolean(); }
            set { SetPropertyValue("IR5", value); RaisePropertyChanged("IR5"); }
        }
        public bool IR6
        {
            get { return GetPropertyValue("IR6").ToBoolean(); }
            set { SetPropertyValue("IR6", value); RaisePropertyChanged("IR6"); }
        }
        public bool Impact1
        {
            get { return GetPropertyValue("Impact1").ToBoolean(); }
            set { SetPropertyValue("Impact1", value); RaisePropertyChanged("Impact1"); }
        }
        public bool Impact2
        {
            get { return GetPropertyValue("Impact2").ToBoolean(); }
            set { SetPropertyValue("Impact2", value); RaisePropertyChanged("Impact2"); }
        }
        public bool Impact3
        {
            get { return GetPropertyValue("Impact3").ToBoolean(); }
            set { SetPropertyValue("Impact3", value); RaisePropertyChanged("Impact3"); }
        }
        public bool Impact4
        {
            get { return GetPropertyValue("Impact4").ToBoolean(); }
            set { SetPropertyValue("Impact4", value); RaisePropertyChanged("Impact4"); }
        }
        public bool Impact5
        {
            get { return GetPropertyValue("Impact5").ToBoolean(); }
            set { SetPropertyValue("Impact5", value); RaisePropertyChanged("Impact5"); }
        }
        public bool Impact6
        {
            get { return GetPropertyValue("Impact6").ToBoolean(); }
            set { SetPropertyValue("Impact6", value); RaisePropertyChanged("Impact6"); }
        }
        public bool Impact7
        {
            get { return GetPropertyValue("Impact7").ToBoolean(); }
            set { SetPropertyValue("Impact7", value); RaisePropertyChanged("Impact7"); }
        }
        public bool Impact8
        {
            get { return GetPropertyValue("Impact8").ToBoolean(); }
            set { SetPropertyValue("Impact8", value); RaisePropertyChanged("Impact8"); }
        }
        public bool Impact9
        {
            get { return GetPropertyValue("Impact9").ToBoolean(); }
            set { SetPropertyValue("Impact9", value); RaisePropertyChanged("Impact9"); }
        }
        public bool Impact10
        {
            get { return GetPropertyValue("Impact10").ToBoolean(); }
            set { SetPropertyValue("Impact10", value); RaisePropertyChanged("Impact10"); }
        }
        public bool EStop
        {
            get { return GetPropertyValue("EStop").ToBoolean(); }
            set { SetPropertyValue("EStop", value); RaisePropertyChanged("EStop"); }
        }
        public bool Yellow 
        { 
            get { return GetPropertyValue("Yellow").ToBoolean(); } 
            set { SetPropertyValue("Yellow", value); RaisePropertyChanged("Yellow"); } 
        }

        public bool Blue  
        { 
            get { return GetPropertyValue("Blue").ToBoolean(); } 
            set { SetPropertyValue("Blue", value); RaisePropertyChanged("Blue"); } 
        }

        public bool Red 
        { 
            get { return GetPropertyValue("Red").ToBoolean(); } 
            set { SetPropertyValue("Red", value); RaisePropertyChanged("Red"); } 
        }

        public bool Green 
        { 
            get { return GetPropertyValue("Green").ToBoolean(); } 
            set { SetPropertyValue("Green", value); RaisePropertyChanged("Green"); } 
        }

        public bool Black 
        { 
            get { return GetPropertyValue("Black").ToBoolean(); } 
            set { SetPropertyValue("Black", value); RaisePropertyChanged("Black"); } 
        }
    }
}
