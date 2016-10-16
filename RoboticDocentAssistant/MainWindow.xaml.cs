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
using System.Threading;
using HENRY.Model;
using System.Collections.ObjectModel;

using HENRY.ViewModel;

namespace HENRY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MasterModel mastermodel;
        MasterViewModel masterviewmodel;

        public MainWindow()
        {
            mastermodel = new MasterModel();
            masterviewmodel = new MasterViewModel();

            masterviewmodel.InitializeViewmodel(mastermodel);

            DataContext = masterviewmodel;


            InitializeComponent();
        }

        private void UserViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserViewModel ViewModelObject1 = new UserViewModel();

            ViewModelObject1.InitializeViewmodel("user", 123);
            UserViewControl.DataContext = ViewModelObject1;
        }

        private void DevViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            DevViewModel ViewModelObject2 = new DevViewModel();

            ViewModelObject2.InitializeViewmodel("developer", 777);
            DevViewControl.DataContext = ViewModelObject2;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mastermodel.SetUser();
        }
    }
}
