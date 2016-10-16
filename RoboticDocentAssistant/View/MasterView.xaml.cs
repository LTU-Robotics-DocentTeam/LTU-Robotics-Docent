using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HENRY.ViewModel;


namespace HENRY.View
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : UserControl
    {
        public MasterView()
        {
            InitializeComponent();
        }

        private void DevViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            DevViewModel ViewModelObject = new DevViewModel();

            ViewModelObject.InitializeViewmodel("developer", 777);
            DevViewControl.DataContext = ViewModelObject;
        }

        private void UserViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserViewModel ViewModelObject = new UserViewModel();

            ViewModelObject.InitializeViewmodel("user", 123);
            UserViewControl.DataContext = ViewModelObject;
        }
    }
}
