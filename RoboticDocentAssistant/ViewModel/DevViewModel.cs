using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.ViewModel
{
    class DevViewModel
    {
        public String DevString
        {
            get;
            set;
        }

        public int DevNumber
        {
            get;
            set;
        }

        public void InitializeViewmodel(String devstring, int devint)
        {

            DevString = devstring;
            DevNumber = devint;
        }
    }
}
