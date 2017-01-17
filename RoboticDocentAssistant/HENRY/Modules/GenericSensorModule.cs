using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules
{
    class GenericSensorModule : LengarioModuleAuxiliary
    {
        Timer t;
        /*
        ============================================ Constructor =============================================
        
         * This module represents a sort of template to
         * build basic processing modules off of. For
         * example, a Hall Effect Module that takes the
         * many raw boolean inputs in the array and
         * processes them into a single angle of the line
         * under the robot. A basic timer is used to do 
         * periodic work. (like a basic thread, but nice
         * and slow)
         * 
         * Notice all of the property names start with an
         * identifier named after the module. Not necessary,
         * but also not a bad idea to do.
         * 
         * Also notice the constructor initializes all of those
         * properties. This makes sure that they ready for
         * reading, and more importantly it allows this module
         * to claim the properties.
         * 
         * The SerialCommModule also writes to the sensor values,
         * but because it is a core module, it does not assign to
         * or care about ownership. Only this AuxiliaryModule can
         * now write or read to those properties. Any other module
         * accidentally interfering will be blocked.
         */

        public GenericSensorModule()
        {
            /* 
             * SetPropertyValue method is inherited from the ModuleSystem
             * It can set any property from the ModuleSystem, provided that
             * the module owns the data. (or is a CoreModule) This means
             * the data is both private to the module, and available
             * globally to powerful CoreModules such as a Serial Port or a
             * ViewModel, which uses the property to display on a View.
             */

            SetPropertyValue("Generic_Sensor1", 0);
            SetPropertyValue("Generic_Sensor2", 0);
            SetPropertyValue("Generic_Sensor3", 0);

            SetPropertyValue("Generic_Output1", 0);
            SetPropertyValue("Generic_Multiply", true);

            t = new Timer();
            t.Interval = 330;
            t.Elapsed += t_Elapsed;
            t.Start();
        }


        //============================================== Do Work ===============================================

        /* This method is an example of a module doing work that should be kept seperate
         * from other modules. This particular example has the module adding the three raw
         * sensor values to create an output value.
         * 
         * This example also implements one property acting as a sort of 'setting',
         * multiplying the output by 1000. This property is set by the ModuleCore ViewModel.
         * The ModuleCore ViewModel has this property bound to a CheckBox. When the CheckBox
         * is changed, the property is updated and used in this module. (This module
         * technically owns the property)
         */

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            /* 
            * GetPropertyValue method is inherited from the ModuleSystem
            * It can get any property from the ModuleSystem, provided that
            * the module owns the data. (or is a CoreModule) The method
            * returns a LengarioModuleProperty object, which can be cast
            * to different types. Don't mix and match types! Bad times.
            * String, Char, DateTime, Int32, and Boolean are available now.
            */

            int s1 = GetPropertyValue("Generic_Sensor1").ToInt32();
            int s2 = GetPropertyValue("Generic_Sensor2").ToInt32();
            int s3 = GetPropertyValue("Generic_Sensor3").ToInt32();

            int o1 = s1 + s2 + s3;

            if(GetPropertyValue("Generic_Multiply").ToBoolean())
            {
                o1 = o1 * 1000;
            }

            SetPropertyValue("Generic_Output1", o1);

            // It seems clunky to have to read and right to the magical ModuleProperties
            // any time you want to use data, but the benefits of global data management
            // are pretty neat.
            
        }


        //=================================== Inheritance Implementation =======================================

        //Method required by ModuleSystem to identify module in respect to system (ownership)
        //Required for every module


        public override string GetModuleName()
        {
            return "GenericSensorModule";
        }
    

    }
}
