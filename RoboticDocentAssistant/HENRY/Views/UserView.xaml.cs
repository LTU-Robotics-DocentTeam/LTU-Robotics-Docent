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
        public enum UserScreen { Tour, Shutdown, Kiosk, Manual, MainMenu };

        public UserScreen currentMode = UserScreen.MainMenu;

        //public bool showPrompt = false; 

        public UserView()
        {
            InitializeComponent();
        }


        internal void ToggleKiosk()
        {
            if (kioskViewControl.Visibility == Visibility.Hidden)
            {
                kioskViewControl.Visibility = Visibility.Visible;
                currentMode = UserScreen.Kiosk;
            }

            else if (kioskViewControl.Visibility == Visibility.Visible)
            {
                kioskViewControl.Visibility = Visibility.Hidden;
                currentMode = UserScreen.MainMenu;
                kioskPromptText.Visibility = Visibility.Hidden;
            }
        }

        internal void ShowKioskGetOut()
        {
            kioskPromptText.Visibility = Visibility.Visible;
        }
    }
}
