using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HENRY.Model;
using System.Collections.ObjectModel;

namespace HENRY.ViewModel
{
    class UserViewModel
    {
        public String UserString
        {
            get;
            set;
        }

        public int UserNumber
        {
            get;
            set;
        }

        public void InitializeViewmodel(String userstring, int userint)
        {

            UserString = userstring;
            UserNumber = userint;
        }
    }
}
