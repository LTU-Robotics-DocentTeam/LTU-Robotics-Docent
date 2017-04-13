using HENRY.Modules;
using HENRY.Modules.Navigation;
using HENRY.Modules.Sensors;
using HENRY.Views;
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

        public enum Buttons { Green, Red, Yellow, Blue, Black };
        
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
            scm = new SerialCommModule();
            erlg = new ErrorLog();
            bnm = new BaseNavModule();
            mmd = new MotorModule();
            hem = new HallEffectSensorModule();
            ism = new ImpactSensorModule();
            irm = new InfraredSensorModule();
            usm = new UltrasonicSensorModule();
            vm = new ViewModel();
            mnd = new ManualDrive();
            gsm = new GenericSensorModule();

            vm.Green = false;
            vm.Red = false;
            vm.Blue = false;
            vm.Yellow = false;

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
        /// Handles pressed keys on the keyboard or controller. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void keyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Green, true);
                if (vm.UserModeOn) UserModeController(Buttons.Green, true);
            }

            if (e.Key == Key.S)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Red, true);
                if (vm.UserModeOn) UserModeController(Buttons.Red, true);
            }
            if (e.Key == Key.D)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Blue, true);
                if (vm.UserModeOn) UserModeController(Buttons.Blue, true);
            }
            if (e.Key == Key.A)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Yellow, true);
                if (vm.UserModeOn) UserModeController(Buttons.Yellow, true);
            }
            if (e.Key == Key.Q)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Black, true);
                if (vm.UserModeOn) UserModeController(Buttons.Black, true);
            }

            // Not gonna affect the controller
            if (e.Key == Key.U)
            {
                vm.DevModeOn = !vm.DevModeOn;
                vm.UserModeOn = !vm.UserModeOn;
            }
            if (e.Key == Key.E)
            {
                vm.EStop = !vm.EStop;
            }
        }

        /// <summary>
        /// Handles keys released on the keyboard or controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void keyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Green, false);
                if (vm.UserModeOn) UserModeController(Buttons.Green, false);
            }
                
            if (e.Key == Key.S)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Red, false);
                if (vm.UserModeOn) UserModeController(Buttons.Red, false);
            }
            if (e.Key == Key.D)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Blue, false);
                if (vm.UserModeOn) UserModeController(Buttons.Blue, false);
            }
            if (e.Key == Key.A)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Yellow, false);
                if (vm.UserModeOn) UserModeController(Buttons.Yellow, false);
            }
            if (e.Key == Key.Q)
            {
                if (vm.DevModeOn) DevModeController(Buttons.Black, false);
                if (vm.UserModeOn) UserModeController(Buttons.Black, false);
            }
        }

        private void UserModeController(Buttons b, bool p)
        {
            switch (b)
            {
                case Buttons.Green:
                    if (p)
                    {
                        if (userViewControl.currentMode == UserView.UserScreen.Kiosk) userViewControl.ShowKioskGetOut();
                    }
                    break;
                case Buttons.Red:
                    if (p)
                    {
                        if (userViewControl.currentMode == UserView.UserScreen.Kiosk) userViewControl.ShowKioskGetOut();
                    }
                    break;
                case Buttons.Blue:
                    if (p)
                    {
                        if (userViewControl.currentMode == UserView.UserScreen.MainMenu) userViewControl.ToggleKiosk();
                    }
                    break;
                case Buttons.Yellow:
                    if (p)
                    {
                        if (userViewControl.currentMode == UserView.UserScreen.Kiosk) userViewControl.ShowKioskGetOut();
                    }
                    break;
                case Buttons.Black:
                    if (p)
                    {
                        if (userViewControl.currentMode == UserView.UserScreen.Kiosk && userViewControl.kioskPromptText.Visibility == Visibility.Hidden)
                        {
                            userViewControl.ShowKioskGetOut();
                        }
                        else if (userViewControl.currentMode == UserView.UserScreen.Kiosk && userViewControl.kioskPromptText.Visibility == Visibility.Visible)
                        {
                            userViewControl.ToggleKiosk();
                        }
                    }
                    break;
            }
        }

        private void DevModeController(Buttons b, bool p)
        {
            switch (b)
            {
                case Buttons.Green:
                    vm.Forward = p;
                    break;
                case Buttons.Red:
                    vm.Backward = p;
                    break;
                case Buttons.Blue:
                    vm.Right = p;
                    break;
                case Buttons.Yellow:
                    vm.Left = p;
                    break;
                case Buttons.Black:
                    if (p) ToggleManualDrive();
                    break;
            }
        }

        private void ToggleManualDrive()
        {
            vm.ManualDriveEnabled = !vm.ManualDriveEnabled;
            if (vm.ManualDriveEnabled) mnd.t.Start();
            else mnd.t.Stop();
            
        }



        // ===================================================================================================
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
        // ===================================================================================================
        // ===================================================================================================

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
