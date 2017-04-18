using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Configuration;

namespace HENRY.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    /// TO DO:
    /// - Add regular UserView stuff (Display questions, controller layout, etc.)
    public partial class UserView : UserControl
    {
        public enum UserScreen { Tour, Shutdown, Kiosk, Manual, MainMenu, Estop };

        public UserScreen currentMode = UserScreen.MainMenu, previousMode = UserScreen.MainMenu;

        //public bool showPrompt = false; 

        public UserView()
        {
            InitializeComponent();
        }

        internal void ToggleMode (UserScreen mode, bool p)
        {
            if (p)
            {
                currentMode = mode;
                switch (mode)
                {
                    case UserScreen.Tour:
                        TourMode.Visibility = Visibility.Visible;
                        ShutdownMode.Visibility = Visibility.Hidden;
                        kioskViewControl.Visibility = Visibility.Hidden;
                        ManualDriveMode.Visibility = Visibility.Hidden;
                        break;
                    case UserScreen.Shutdown:
                        ShutdownMode.Visibility = Visibility.Visible;
                        kioskViewControl.Visibility = Visibility.Hidden;
                        ManualDriveMode.Visibility = Visibility.Hidden;
                        TourMode.Visibility = Visibility.Hidden;
                        break;
                    case UserScreen.Kiosk:
                        kioskViewControl.Visibility = Visibility.Visible;
                        ShutdownMode.Visibility = Visibility.Hidden;
                        ManualDriveMode.Visibility = Visibility.Hidden;
                        TourMode.Visibility = Visibility.Hidden;
                        break;
                    case UserScreen.Manual:
                        ManualDriveMode.Visibility = Visibility.Visible;
                        ShutdownMode.Visibility = Visibility.Hidden;
                        TourMode.Visibility = Visibility.Hidden;
                        kioskViewControl.Visibility = Visibility.Hidden;
                        break;
                    case UserScreen.MainMenu:
                        ManualDriveMode.Visibility = Visibility.Hidden;
                        ShutdownMode.Visibility = Visibility.Hidden;
                        TourMode.Visibility = Visibility.Hidden;
                        kioskViewControl.Visibility = Visibility.Hidden;
                        kioskPromptText.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        private void EStopPrompt_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                currentMode = previousMode;
            }
            else
            {
                previousMode = currentMode;
                currentMode = UserScreen.Estop;
            }
        }

        internal void ShowKioskGetOut(bool p)
        {
            
            if (p) kioskPromptText.Visibility = Visibility.Visible;
        }

        
    }

}
