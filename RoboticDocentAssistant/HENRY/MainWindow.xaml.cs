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

using HENRY.Modules;

namespace HENRY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        SerialCommModule scm;
        GenericSensorModule gsm;

        public MainWindow()
        {
            vm = new ViewModel();
            scm = new SerialCommModule();
            gsm = new GenericSensorModule();

            InitializeComponent();
        }

        private void userViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            userViewControl.DataContext = vm;
        }
    }
}
