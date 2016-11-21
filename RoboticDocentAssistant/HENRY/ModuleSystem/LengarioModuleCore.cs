using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.ModuleSystem
{
    abstract class LengarioModuleCore : LengarioModuleBase
    {
        public override LengarioModuleProperty GetPropertyValue(string name)
        {
            string moduleName = GetModuleName();

            if (theValueBin.ContainsKey(name))
            {
                return theValueBin[name];
            }
            else
            {
                Console.WriteLine("ERR: Module " + moduleName + " attepted to read from nonexistant property " + name);
                return new LengarioModuleProperty(null);
            }
        }

        public override void SetPropertyValue(string name, object propertyValue)
        {
            UpdateProperty(name);

            if (theValueBin.ContainsKey(name))
            {
                theValueBin[name].SetValue(propertyValue);
            }
            else
            {
                LengarioModuleProperty newProperty = new LengarioModuleProperty(propertyValue);
                newProperty.Owner = null;

                theValueBin.Add(name, newProperty);
            }
        }

        public abstract string GetModuleName();
    }

}
