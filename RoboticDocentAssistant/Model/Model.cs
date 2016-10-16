using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HENRY.Model
{
    public class Model { }

    public class Student : INotifyPropertyChanged
    {
        private string userThing;
        private string devThing;

        public string UserThing
        {
            get
            {
                return userThing;
            }

            set
            {
                if (userThing != value)
                {
                    userThing = value;
                    RaisePropertyChanged("UserThing");
                    RaisePropertyChanged("BothThings");
                }
            }
        }

        public string DevThing
        {
            get { return devThing; }

            set
            {
                if (devThing != value)
                {
                    devThing = value;
                    RaisePropertyChanged("DevThing");
                    RaisePropertyChanged("BothThings");
                }
            }
        }

        public string BothThings
        {
            get
            {
                return userThing + " " + devThing;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    } 
}
