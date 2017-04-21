using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HENRY.ModuleSystem
{
    class LengarioModuleProperty
    {
        private string owner;
        private Type type;
        private object pvalue;

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public object RawValue
        {
            get { return pvalue; }
        }

        public Type Type
        {
            get { return type; }
        }


        public LengarioModuleProperty(object _value)
        {
            pvalue = _value;

            if (_value != null)
                type = pvalue.GetType();

            owner = string.Empty;
        }

        public void SetValue(object newValue)
        {
            if (type == newValue.GetType())
            {
                pvalue = newValue;
            }
            else
            {
                MessageBox.Show("New value " + newValue + " does not match type " + type + " in property owned by " + owner);
            }
        }


        public string ToString()
        {
            return Convert.ToString(pvalue);
        }
        public bool ToBoolean()
        {
            return Convert.ToBoolean(pvalue);
        }
        public int ToInt32()
        {
            return Convert.ToInt32(pvalue);
        }
        public DateTime ToDateTime()
        {
            return Convert.ToDateTime(pvalue);
        }
        public char ToChar()
        {
            return Convert.ToChar(pvalue);
        }
        public double ToDouble()
        {
            return Convert.ToDouble(pvalue);
        }
    }
}
