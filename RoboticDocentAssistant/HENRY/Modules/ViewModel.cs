using HENRY.ModuleSystem;
using System.ComponentModel;

namespace HENRY.Modules
{
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
    }
}
