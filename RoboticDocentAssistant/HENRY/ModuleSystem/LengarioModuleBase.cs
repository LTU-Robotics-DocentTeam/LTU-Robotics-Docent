using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.ModuleSystem
{
    abstract class LengarioModuleBase
    {
        protected static Dictionary<string, LengarioModuleProperty> theValueBin;

        public LengarioModuleBase()
        {
            if (theValueBin == null)
                theValueBin = new Dictionary<string, LengarioModuleProperty>();
        }

        public static event EventHandler<PropertyUpdateArgs> PropertyUpdated;

        protected static void UpdateProperty(string property)
        {
            if(PropertyUpdated != null)
            {
                PropertyUpdated(null, new PropertyUpdateArgs { PropertyUpdated = property });
            }
        }

        public static Dictionary<string, LengarioModuleProperty> GetTheBin()
        {
            return theValueBin;
        }

        public abstract LengarioModuleProperty GetPropertyValue(string name);
        public abstract void SetPropertyValue(string name, object value);

    }

    public class PropertyUpdateArgs : EventArgs
    {
        public string PropertyUpdated { get; set; }
    }
}
