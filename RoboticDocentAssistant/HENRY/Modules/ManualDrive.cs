using HENRY.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules
{
    class ManualDrive : LengarioModuleCore
    {
        public ManualDrive()
        {
            SetPropertyValue("ManualDriveEnabled", false);
        }



        public override string GetModuleName()
        {
            return "ManualDrive";
        }
    }
}
