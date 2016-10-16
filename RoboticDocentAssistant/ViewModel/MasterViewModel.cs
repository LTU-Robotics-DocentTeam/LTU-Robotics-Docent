using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HENRY.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace HENRY.ViewModel
{
    class MasterViewModel : INotifyPropertyChanged
    {
        public Visibility UserVisibility;
        public Visibility DevVisibility;
            

        public MasterViewModel()
        {
        }

        private MasterModel reference;

        public void InitializeViewmodel(MasterModel model)
        {
            reference = model;
            RaisePropertyChanged("UserVisibility");
            RaisePropertyChanged("DevVisibility");
            UserVisibility = reference.UserVisibility;
            DevVisibility = reference.DevVisibility;
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
