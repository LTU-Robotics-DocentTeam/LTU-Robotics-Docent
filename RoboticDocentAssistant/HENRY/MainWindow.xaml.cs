using HENRY.Modules;
using HENRY.Modules.Navigation;
using HENRY.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace HENRY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Buttons { Green, Red, Yellow, Blue, Black };
        
        ViewModel vm;
        SerialCommModule scm;
        GenericSensorModule gsm;
        
        BaseNavModule bnm;
        MotorModule mmd;
        ManualDrive mnd;

        Timer holdDownTimer;

        public MainWindow()
        {
            scm = new SerialCommModule();
            bnm = new BaseNavModule();
            mmd = new MotorModule();
            vm = new ViewModel();
            mnd = new ManualDrive();
            gsm = new GenericSensorModule();

            vm.Green = false;
            vm.Red = false;
            vm.Blue = false;
            vm.Yellow = false;

            InitializeComponent();
            
            MWindow.DataContext = vm;
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
            if (e.Key == Key.T)
            {
                ToggleAutonomousNavigation(true);
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

        /// <summary>
        /// This function handles all button prompts for user mode. Its basically a giant switch statement with a case for each button, with sub switch cases for each sub mode
        /// </summary>
        /// <param name="b"> Which button was pressed</param>
        /// <param name="p"> True if button pressed, false if button released</param>
        private void UserModeController(Buttons b, bool p)
        {
            switch (b)
            {
                case Buttons.Green:
                    switch (userViewControl.currentMode)
                    {
                        case UserView.UserScreen.Tour: if (!vm.AutonomousNavigation) ToggleAutonomousNavigation(p);
                            break;
                        case UserView.UserScreen.Shutdown: 
                            break;
                        case UserView.UserScreen.Kiosk: userViewControl.ShowKioskGetOut(p);
                            break;
                        case UserView.UserScreen.Manual: vm.Forward = p;
                            break;
                        case UserView.UserScreen.MainMenu: userViewControl.ToggleMode(UserView.UserScreen.Tour, p);
                            break;
                    }
                    break;
                case Buttons.Red:
                    switch (userViewControl.currentMode)
                    {
                        case UserView.UserScreen.Tour: if (vm.AutonomousNavigation) ToggleAutonomousNavigation(p);
                            break;
                        case UserView.UserScreen.Shutdown: if (p) MWindow.Close();
                            break;
                        case UserView.UserScreen.Kiosk: userViewControl.ShowKioskGetOut(p);
                            break;
                        case UserView.UserScreen.Manual: vm.Backward = p;
                            break;
                        case UserView.UserScreen.MainMenu: userViewControl.ToggleMode(UserView.UserScreen.Shutdown, p);
                            break;
                    }
                    break;
                case Buttons.Blue:
                    switch (userViewControl.currentMode)
                    {
                        case UserView.UserScreen.Tour:
                            break;
                        case UserView.UserScreen.Shutdown:
                            break;
                        case UserView.UserScreen.Kiosk: userViewControl.ShowKioskGetOut(p);
                            break;
                        case UserView.UserScreen.Manual: vm.Right = p;
                            break;
                        case UserView.UserScreen.MainMenu: userViewControl.ToggleMode(UserView.UserScreen.Kiosk, p);
                            break;
                    }
                    
                    break;
                case Buttons.Yellow:
                    switch (userViewControl.currentMode)
                    {
                        case UserView.UserScreen.Tour:
                            break;
                        case UserView.UserScreen.Shutdown:
                            break;
                        case UserView.UserScreen.Kiosk: userViewControl.ShowKioskGetOut(p);
                            break;
                        case UserView.UserScreen.Manual: vm.Left = p;
                            break;
                        case UserView.UserScreen.MainMenu: userViewControl.ToggleMode(UserView.UserScreen.Manual, p);
                            ToggleManualDrive(p);
                            break;
                    }
                    break;
                case Buttons.Black:
                    switch (userViewControl.currentMode)
                    {
                        case UserView.UserScreen.Kiosk:  
                            if (userViewControl.kioskPromptText.Visibility == Visibility.Hidden)
                            {
                                userViewControl.ShowKioskGetOut(p);
                            }
                            else
                            {
                                userViewControl.ToggleMode(UserView.UserScreen.MainMenu, p);
                            }
                            break;
                        case UserView.UserScreen.Manual:
                            userViewControl.ToggleMode(UserView.UserScreen.MainMenu, p); ;
                            ToggleManualDrive(p);
                            break;
                        case UserView.UserScreen.MainMenu:
                            break;
                        case UserView.UserScreen.Estop: vm.EStop = false;
                            break;
                        default: userViewControl.ToggleMode(UserView.UserScreen.MainMenu, p);
                            break;
                    }
                    break;
            }
        }
        /// <summary>
        /// This function handles all button prompts for dev mode. Allows for manual control only
        /// </summary>
        /// <param name="b"> Which button was pressed</param>
        /// <param name="p"> True if button pressed, false if button released</param>
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
                    if (p) ToggleManualDrive(p);
                    break;
            }
        }

        private void ToggleManualDrive(bool p)
        {
            if (p)
            {
                vm.ManualDriveEnabled = !vm.ManualDriveEnabled;
                if (vm.ManualDriveEnabled) mnd.t.Start();
                else mnd.t.Stop();
            }
        }

        private void ToggleAutonomousNavigation(bool p)
        {
            if (p)
            {
                vm.AutonomousNavigation = !vm.AutonomousNavigation;
                if (vm.AutonomousNavigation) bnm.t.Start();
                else bnm.t.Stop();
            }
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
            bnm.error_log.CloseLog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MWindow.Close();
        }

        private void ViewControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            vm.ManualDriveEnabled = false;
            mnd.t.Stop();
            vm.AutonomousNavigation = false;
            bnm.t.Stop();
            userViewControl.ToggleMode(UserView.UserScreen.MainMenu, true);
        }
    }
}
