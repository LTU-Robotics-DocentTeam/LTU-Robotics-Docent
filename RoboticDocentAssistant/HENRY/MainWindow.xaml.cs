using HENRY.Modules;
using HENRY.Modules.Navigation;
using HENRY.Modules.Sensors;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace HENRY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ErrorLog erlg;
        
        ViewModel vm;
        SerialCommModule scm;
        GenericSensorModule gsm;

        HallEffectSensorModule hem;
        ImpactSensorModule ism;
        InfraredSensorModule irm;
        UltrasonicSensorModule usm;
        BaseNavModule bnm;

        MotorModule mmd;
        ManualDrive mnd;

        public MainWindow()
        {
            erlg = new ErrorLog();
            scm = new SerialCommModule();
            bnm = new BaseNavModule();
            mmd = new MotorModule();
            hem = new HallEffectSensorModule();
            ism = new ImpactSensorModule();
            irm = new InfraredSensorModule();
            usm = new UltrasonicSensorModule();
            vm = new ViewModel();
            mnd = new ManualDrive();
            gsm = new GenericSensorModule();

            

            InitializeComponent();
            
            MWindow.DataContext = vm;

            //if (vm.DevModeOn)
            //{
            //    devViewControl.Visibility = System.Windows.Visibility.Visible;
            //    userViewControl.Visibility = System.Windows.Visibility.Hidden;
            //}
            //else
            //{
            //    devViewControl.Visibility = System.Windows.Visibility.Hidden;
            //    userViewControl.Visibility = System.Windows.Visibility.Visible;
            //}

        }

        /// <summary>
        /// UserView control load event. Sets data context
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            userViewControl.DataContext = vm;
        }

        /// <summary>
        /// DevView control load event. Sets data context
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void devViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            devViewControl.DataContext = vm;
        }

        /// <summary>
        /// Handles pressed keys on the keyboard. Set to allow for WASD control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void keyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (vm.ManualDriveEnabled)
            {
                if (e.Key == Key.W)
                    vm.Forward = true;
                if (e.Key == Key.S)
                    vm.Backward = true;
                if (e.Key == Key.D)
                    vm.Right = true;
                if (e.Key == Key.A)
                    vm.Left = true;

            }
        }

        /// <summary>
        /// Handles keys released on the keyboard. Set to allow for WASD control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void keyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (vm.ManualDriveEnabled)
            {
                if (e.Key == Key.W)
                    vm.Forward = false;
                if (e.Key == Key.S)
                    vm.Backward = false;
                if (e.Key == Key.D)
                    vm.Right = false;
                if (e.Key == Key.A)
                    vm.Left = false;
            }
        }

        // ===================================================================================================
        // Leftover testing code? Not sure if needed

        public class ThingClass : INotifyPropertyChanged
        {
            private string sometext;

            public string SomeText
            {
                get
                {
                    return sometext;
                }
                set
                {
                    sometext = value;
                    RaisePropertyChanged("SomeText");
                }
            }

            private Dictionary<string, string> somedic;

            public Dictionary<string,string> SomeDic
            {
                get
                {
                    return somedic;
                }
                set
                {
                    somedic = value;
                    RaisePropertyChanged("SomeDic");
                }
            }

            // ===================================================================================================

            //Basic INotifyPropertyChanged interface implementation

            public event PropertyChangedEventHandler PropertyChanged;

            private void RaisePropertyChanged(string property)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }

        private void MWindow_Closing(object sender, CancelEventArgs e)
        {
            erlg.CloseLog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MWindow.Close();
        }

        
        
    }
}
