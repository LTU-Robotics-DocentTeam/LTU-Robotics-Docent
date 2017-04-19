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
        string moduleName;

        public ErrorLog(LengarioModuleCore m)
        {
            moduleName = m.GetModuleName();
        }

        public void WriteToLog (string line)
        {
            file.WriteLineAsync(line);
        }

        public void OpenLog()
        {
            string filename = moduleName + "test" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".csv";
            file = new StreamWriter("..\\..\\ErrorLogs\\" + filename);
            //file.WriteLine("Error log for " + moduleName + " on " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }

        public void CloseLog()
        {
            try
            {
                file.Close();
            }
            catch (Exception)
            {
                
            }
            
        }

        public override string GetModuleName()
        {
            return "ErrorLog";
        }
    }
}
