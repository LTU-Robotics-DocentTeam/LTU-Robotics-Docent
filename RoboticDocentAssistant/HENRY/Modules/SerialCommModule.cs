using HENRY.ModuleSystem;
using System;
using System.Timers;

namespace HENRY.Modules
{
    class SerialCommModule : LengarioModuleCore
    {
        Timer t;
        Random r;

        public SerialCommModule()
        {
            t = new Timer();
            t.Interval = 100;
            t.Elapsed += t_Elapsed;
            t.Start();

            r = new Random();
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
