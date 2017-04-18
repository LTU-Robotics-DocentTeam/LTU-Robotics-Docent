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
        StreamWriter file;

        public ErrorLog(LengarioModuleCore m)
        {
            string filename = m.GetModuleName() + "test" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".txt";
            file = new StreamWriter("..\\..\\ErrorLogs\\" + filename);
            file.WriteLine("Error log for " + m.GetModuleName() + " on " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }

        public void WriteToLog (string line)
        {
            file.WriteLineAsync(line);
        }

        public void CloseLog()
        {
            file.Close();
        }

        public override string GetModuleName()
        {
            return "ErrorLog";
        }
    }
}
