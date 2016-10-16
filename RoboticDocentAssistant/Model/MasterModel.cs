using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.ComponentModel;

namespace HENRY.Model
{
    class MasterModel : INotifyPropertyChanged
    {
        private Visibility userVis;
        private Visibility devVis;

        public Visibility UserVisibility
        {
            get
            {
                return userVis;
            }
        }
        public Visibility DevVisibility
        {
            get
            {
                return devVis;
            }

        }

        /*
        private Boolean devVisible;


        public Boolean DevVisible
        {
            get
            {
                return devVisible;
            }
            set
            {
                devVisible = value;
                RaisePropertyChanged("DevVisible");
            }
        }*/

        public MasterModel()
        {
            userVis = Visibility.Visible;
            devVis = Visibility.Collapsed;
        }

        public void SetUser()
        {
            userVis = Visibility.Collapsed;
            devVis = Visibility.Visible;
            RaisePropertyChanged("UserVisibility");
            RaisePropertyChanged("DevVisibility");
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
