using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HENRY.ModuleSystem
{
    abstract class LengarioModuleAuxiliary : LengarioModuleBase
    {

        public override LengarioModuleProperty GetPropertyValue(string name)
        {
            string moduleName = GetModuleName();

            if (theValueBin.ContainsKey(name))
            {
                if ((theValueBin[name].Owner == moduleName) || (theValueBin[name].Owner == null))
                {
                    return theValueBin[name];
                }
                else
                {
                    Console.WriteLine("ERR: Module " + moduleName + " attepted to read from unowned property " + name);
                    return new LengarioModuleProperty(null);
                }
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
            string moduleName = GetModuleName();

            if (theValueBin.ContainsKey(name))
            {
                if (theValueBin[name].Owner == moduleName)
                {
                    theValueBin[name].SetValue(propertyValue);
                }
                else if (theValueBin[name].Owner == null)
                {
                    theValueBin[name].Owner = moduleName;
                    theValueBin[name].SetValue(propertyValue);
                }
                else
                {
                    MessageBox.Show("ERR: Module " + moduleName + " attepted to write to unowned property " + name);
                    //Console.WriteLine("ERR: Module " + moduleName + " attepted to write to unowned property " + name);
                }
            }
            else
            {
                LengarioModuleProperty newProperty = new LengarioModuleProperty(propertyValue);
                newProperty.Owner = moduleName;

                theValueBin.Add(name, newProperty);
            }

        }

        public abstract string GetModuleName();
    }

}
