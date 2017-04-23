using HENRY.Modules;
using HENRY.Modules.Navigation;
using HENRY.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System;
using System.Diagnostics;

namespace HENRY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Buttons { Green, Red, Yellow, Blue, Black };
        bool GreenHold = false, RedHold = false, YellowHold = false, BlueHold = false, BlackHold = false; 
        
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

            

            vm.Green = 0;
            vm.Red = 0;
            vm.Blue = 0;
            vm.Yellow = 0;
            vm.Black = 0;

            vm.GreenPressed = false;
            vm.RedPressed = false;
            vm.BluePressed = false;
            vm.YellowPressed = false;
            vm.BlackPressed = false;

            vm.FinalCountdown = 0;

            InitializeComponent();

            MWindow.DataContext = vm;



            holdDownTimer = new Timer();
            holdDownTimer.Interval = 40;
            holdDownTimer.Elapsed += holdDownTimer_Elapsed;
            holdDownTimer.Start();
        }

        void holdDownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ControllerCount();
            ControllerAct();
            ShutdownCounter();
        }

        private void ShutdownCounter()
        {
            if (vm.CriticalVoltage && vm.UserModeOn)
            {
                vm.FinalCountdown += 0.00083333;
                if (vm.FinalCountdown >= 1)
                {
                    CloseWindow();
                }
            }
            else
                vm.FinalCountdown = 0;
        }


        void UserViewDelegateToggleMode(UserView.UserScreen choice)
        {
            userViewControl.Dispatcher.Invoke(new Action(() => userViewControl.ToggleMode(choice)));
        }

        void UserViewDelegateShowKioskGetOut()
        {
            userViewControl.Dispatcher.Invoke(new Action(() => userViewControl.ShowKioskGetOut()));
        }

        void UserViewDelegateLivestreamOnOff(Visibility v)
        {
            userViewControl.Dispatcher.Invoke(new Action(() => userViewControl.LivestreamOnOff(v)));
        }


        void CloseWindow()
        {
            this.Dispatcher.Invoke(new Action(() => MWindow.Close()));
        }


        void ControllerCount()
        {
            
            switch (userViewControl.currentMode)
            {
                case UserView.UserScreen.Tour:
                    if (vm.GreenPressed)
                        vm.Green += 0.09;
                    if (vm.RedPressed)
                        vm.Red += 0.09;
                    if (vm.BlackPressed)
                        vm.Black += 0.20;
                    break;
                case UserView.UserScreen.Shutdown:
                    if (vm.RedPressed)
                        vm.Red += 0.01;
                    if (vm.BlackPressed)
                        vm.Black += 0.20;
                    break;
                case UserView.UserScreen.Kiosk:
                    if (vm.GreenPressed)
                        vm.Green = 1;
                    if (vm.RedPressed)
                        vm.Red = 1;
                    if (vm.YellowPressed)
                        vm.Yellow = 1;
                    if (vm.BluePressed)
                        vm.Blue = 1;
                    if (vm.BlackPressed)
                        vm.Black += .20;
                    break;
                case UserView.UserScreen.Manual:
                    if (vm.GreenPressed)
                        vm.Green = 1;
                    if (vm.RedPressed)
                        vm.Red = 1;
                    if (vm.YellowPressed)
                        vm.Yellow = 1;
                    if (vm.BluePressed)
                        vm.Blue = 1;
                    if (vm.BlackPressed)
                        vm.Black += 0.20;                    
                    break;
                case UserView.UserScreen.MainMenu:
                    if (vm.GreenPressed)
                        vm.Green += 0.20;
                    if (vm.RedPressed)
                        vm.Red += 0.20;
                    if (vm.YellowPressed)
                        vm.Yellow += 0.20;
                    if (vm.BluePressed)
                        vm.Blue += 0.20;
                    if (vm.BlackPressed)
                        vm.Black += 0.20;
                    break;
                case UserView.UserScreen.Estop:
                    if (vm.BlackPressed)
                        vm.Black += 0.5;
                    break;
                case UserView.UserScreen.Auto:
                    if (vm.GreenPressed)
                        vm.Green += 0.20;
                    if (vm.RedPressed)
                        vm.Red += 0.5;
                    if (vm.BlackPressed)
                        vm.Black += 0.20;
                    break;
            }
            
        }


        void ControllerAct()
        {

            if (vm.Green >= 1)
            {
                switch (userViewControl.currentMode)
                {
                    case UserView.UserScreen.Tour:
                        // Start stream here!
                        UserViewDelegateLivestreamOnOff(Visibility.Visible);
                        scm.SendStartStream();
                        ResetButton(Buttons.Green);
                        break;
                    case UserView.UserScreen.Shutdown:
                        break;
                    case UserView.UserScreen.Kiosk:
                        ResetButton(Buttons.Green);
                        UserViewDelegateShowKioskGetOut();
                        break;
                    case UserView.UserScreen.Manual:
                        vm.Forward = true;
                        break;
                    case UserView.UserScreen.MainMenu:
                        UserViewDelegateToggleMode(UserView.UserScreen.Tour);
                        ResetButton(Buttons.Green);
                        break;
                    case UserView.UserScreen.Auto:
                        if (!vm.AutonomousNavigation)
                            ToggleAutonomousNavigation();
                        ResetButton(Buttons.Green);
                        break;

                }
            }

            if (vm.Red >= 1)
            {       
                switch (userViewControl.currentMode)
                {
                    case UserView.UserScreen.Tour:
                        // End stream here
                        scm.SendStopStream();
                        UserViewDelegateLivestreamOnOff(Visibility.Collapsed);
                        ResetButton(Buttons.Red);
                        break;
                    case UserView.UserScreen.Shutdown:
                        CloseWindow();
                        ResetButton(Buttons.Red);
                        break;
                    case UserView.UserScreen.Kiosk:
                        UserViewDelegateShowKioskGetOut();
                        ResetButton(Buttons.Red);
                        break;
                    case UserView.UserScreen.Manual:
                        vm.Backward = true;
                        break;
                    case UserView.UserScreen.MainMenu:
                        UserViewDelegateToggleMode(UserView.UserScreen.Kiosk);
                        ResetButton(Buttons.Blue);
                        break;
                    case UserView.UserScreen.Auto:
                        if (vm.AutonomousNavigation)
                            ToggleAutonomousNavigation();
                        ResetButton(Buttons.Red);
                        break;
                }
            }

            if (vm.Blue >= 1)
            { 
                switch (userViewControl.currentMode)
                {
                    case UserView.UserScreen.Tour:
                        break;
                    case UserView.UserScreen.Shutdown:
                        break;
                    case UserView.UserScreen.Kiosk:
                        UserViewDelegateShowKioskGetOut();
                        ResetButton(Buttons.Blue);
                        break;
                    case UserView.UserScreen.Manual:
                        vm.Right = true;
                        break;
                    case UserView.UserScreen.MainMenu:
                        UserViewDelegateToggleMode(UserView.UserScreen.Auto);
                        ResetButton(Buttons.Blue);
                        break;
                }
            }

            if (vm.Yellow >= 1)
            {        
                switch (userViewControl.currentMode)
                {
                    case UserView.UserScreen.Tour:
                        break;
                    case UserView.UserScreen.Shutdown:
                        break;
                    case UserView.UserScreen.Kiosk:
                        UserViewDelegateShowKioskGetOut();
                        ResetButton(Buttons.Yellow);
                        break;
                    case UserView.UserScreen.Manual:
                        vm.Left = true;
                        break;
                    case UserView.UserScreen.MainMenu:
                        UserViewDelegateToggleMode(UserView.UserScreen.Manual);
                        ToggleManualDrive();
                        ResetButton(Buttons.Yellow);
                        break;
                }
            }

            if (vm.Black >= 1)
            { 
                switch (userViewControl.currentMode)
                {
                    case UserView.UserScreen.Kiosk:
                        if (userViewControl.kioskPromptText.Visibility == Visibility.Hidden)
                            UserViewDelegateShowKioskGetOut();
                        else
                            UserViewDelegateToggleMode(UserView.UserScreen.MainMenu);
                        ResetButton(Buttons.Black);
                        break;
                    case UserView.UserScreen.Manual:
                        UserViewDelegateToggleMode(UserView.UserScreen.MainMenu);
                        ToggleManualDrive();
                        ResetButton(Buttons.Black);
                        break;
                    case UserView.UserScreen.MainMenu:
                        UserViewDelegateToggleMode(UserView.UserScreen.Shutdown);
                        ResetButton(Buttons.Black);
                        break;
                    case UserView.UserScreen.Estop:
                        vm.EStop = false;
                        ResetButton(Buttons.Black);
                        break;
                    case UserView.UserScreen.Tour:
                        if (vm.AutonomousNavigation)
                            ToggleAutonomousNavigation();
                        UserViewDelegateToggleMode(UserView.UserScreen.MainMenu);
                        ResetButton(Buttons.Black);
                        break;
                    default:
                        UserViewDelegateToggleMode(UserView.UserScreen.MainMenu);
                        ResetButton(Buttons.Black);
                        break;
                }
                
            }
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
            if (vm.DevModeOn)
                DevModeController(e, true);


            if (vm.UserModeOn)
            {
                if (e.Key == Key.W && !GreenHold)
                {
                    vm.GreenPressed = true;
                    GreenHold = true;
                }

                if (e.Key == Key.S && !RedHold)
                {
                    vm.RedPressed = true;
                    RedHold = true;
                }

                if (e.Key == Key.D && !BlueHold)
                {
                    vm.BluePressed = true;
                    BlueHold = true;
                }

                if (e.Key == Key.A && !YellowHold)
                {
                    vm.YellowPressed = true;
                    YellowHold = true;
                }

                if (e.Key == Key.Q && !BlackHold)
                {
                    vm.BlackPressed = true;
                    BlackHold = true;
                }
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
                ToggleAutonomousNavigation();
            }
        }

        /// <summary>
        /// Handles keys released on the keyboard or controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void keyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (vm.DevModeOn)
                DevModeController(e, false);

            else if (vm.UserModeOn)
            {
                
                if (e.Key == Key.W)
                {
                    ResetButton(Buttons.Green);
                    GreenHold = false;
                }
                if (e.Key == Key.S)
                {
                    ResetButton(Buttons.Red);
                    RedHold = false;
                }
                if (e.Key == Key.D)
                {
                    ResetButton(Buttons.Blue);
                    BlueHold = false;
                }
                if (e.Key == Key.A)
                {
                    ResetButton(Buttons.Yellow);
                    YellowHold = false;
                }
                if (e.Key == Key.Q)
                {
                    ResetButton(Buttons.Black);
                    BlackHold = false;
                }
            }
        }

        void ResetButton(Buttons b)
        {
            switch (b)
            {
                case Buttons.Green:
                    vm.GreenPressed = false;
                    vm.Green = 0;
                    if (userViewControl.currentMode == UserView.UserScreen.Manual)
                        vm.Forward = false;
                    break;
                case Buttons.Red:
                    vm.RedPressed = false;
                    vm.Red = 0;
                    if (userViewControl.currentMode == UserView.UserScreen.Manual)
                        vm.Backward = false;
                    break;
                case Buttons.Yellow:
                    vm.YellowPressed = false;
                    vm.Yellow = 0;
                    if (userViewControl.currentMode == UserView.UserScreen.Manual)
                        vm.Left = false;
                    break;
                case Buttons.Blue:
                    vm.BluePressed = false;
                    vm.Blue = 0;
                    if (userViewControl.currentMode == UserView.UserScreen.Manual)
                        vm.Right = false;
                    break;
                case Buttons.Black:
                    vm.BlackPressed = false;
                    vm.Black = 0;
                    break;
            }
        }


        /// <summary>
        /// This function handles all button prompts for dev mode. Allows for manual control only
        /// </summary>
        /// <param name="b"> Which button was pressed</param>
        /// <param name="p"> True if button pressed, false if button released</param>
        private void DevModeController(KeyEventArgs b, bool p)
        {
            switch (b.Key)
            {

                case Key.W: //green
                    vm.Forward = p;
                    break;
                case Key.S: //red
                    vm.Backward = p;
                    break;
                case Key.D: //blue
                    vm.Right = p;
                    break;
                case Key.A: //yellow
                    vm.Left = p;
                    break;
                case Key.Q://black
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

        private void ToggleAutonomousNavigation()
        {
            vm.AutonomousNavigation = !vm.AutonomousNavigation;
            if (vm.AutonomousNavigation) bnm.StartModule();
            else bnm.StopModule();
            if (mmd.recording) mmd.StopRecording();
            else mmd.StartRecording();

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
            holdDownTimer.Stop();

            if (vm.DevModeOn)
            {
                if (userViewControl.stream.IsRunning)
                {
                    //MessageBox.Show("Stream closing...");
                    userViewControl.stream.SignalToStop();
                    userViewControl.stream.WaitForStop();
                    //MessageBox.Show("Stream closed succesfully!");
                }
            }
            else if (vm.UserModeOn)
            {
                userViewControl.stream.SignalToStop();
                userViewControl.stream.WaitForStop();
                foreach (Process p in Process.GetProcesses())
                {

                    if (p.ProcessName == "obs64")
                    {
                        p.Kill();
                    }
                }
                Process.Start("shutdown", "/s /t 0");
            }

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
            userViewControl.ToggleMode(UserView.UserScreen.MainMenu);
            UserView usv = sender as UserView;
            if (usv != null)
            {
                if (!(bool)e.NewValue)
                {
                    if (userViewControl.stream.IsRunning) userViewControl.stream.SignalToStop();
                    Mouse.OverrideCursor = Cursors.Cross;                  
                }
                else
                    Mouse.OverrideCursor = Cursors.None;
            }

        }

        private void MWindow_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
