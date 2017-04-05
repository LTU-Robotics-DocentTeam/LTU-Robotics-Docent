using HENRY.ModuleSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HENRY.Modules
{
    class ErrorLog : LengarioModuleCore
    {
        Timer t;

        StreamWriter file;
        string lines;

        public ErrorLog()
        {
            SetPropertyValue("ErrorLines", "Log for " + DateTime.Now);

            file = new StreamWriter("test.txt");

            t = new Timer();
            t.Interval = 100;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {

            WriteToLog(GetPropertyValue("ErrorLines").ToString());
            
            SetPropertyValue("ErrorLines", "");
        }

        public void WriteToLog (string line)
        {
            file.WriteLineAsync(line);
        }

        public void CloseLog()
        {
            t.Stop();
            file.Close();
        }

        public override string GetModuleName()
        {
            return "ErrorLog";
        }
    }
}
