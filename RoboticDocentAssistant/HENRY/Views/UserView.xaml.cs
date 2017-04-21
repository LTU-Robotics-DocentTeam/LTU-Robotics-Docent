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
using System.Drawing;
using AForge.Video;
using AForge.Wpf.IpCamera;
using System.Threading;

namespace HENRY.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    /// TO DO:
    public partial class UserView : UserControl
    {
        /// <summary>
        /// Used to easily refer to the different modes the UI has
        /// </summary>
        public enum UserScreen { Tour, Shutdown, Kiosk, Manual, Auto, MainMenu, Estop };

        public UserScreen currentMode = UserScreen.MainMenu, previousMode = UserScreen.MainMenu;

        DispatcherTimer kioskPromptTimer; 
        public MJPEGStream stream;

        //public bool showPrompt = false; 

        public UserView()
        {
            museumLogo = new System.Windows.Controls.Image();
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("Images/Museum Photo.png", UriKind.Relative);
            img.EndInit();
            museumLogo.Stretch = Stretch.Fill;
            museumLogo.Source = img;


            kioskPromptTimer = new DispatcherTimer();
            kioskPromptTimer.Interval = new TimeSpan(0, 0, 10);
            kioskPromptTimer.Tick += new EventHandler(kioskPromptTimer_Tick);

            kioskPromptTimer.IsEnabled = true;

            InitializeComponent();
            stream = new MJPEGStream("http://192.168.9.250:8081/video.mjpg");
            stream.NewFrame += stream_NewFrame;

            stream.Start();
        }

        void kioskPromptTimer_Tick(object sender, EventArgs e)
        {
            kioskPromptText.Visibility = Visibility.Hidden;
            kioskPromptTimer.Stop();
        }

        void stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate {
                    tourStream.Source = bi; manualStream.Source = bi;
                }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                stream.Stop();
            }

        }

        /// <summary>
        /// Method that can be called to toggle modes from an overarching window (such as MainWindow)
        /// </summary>
        /// <param name="mode"> What mode are you switching to</param>
        /// <param name="p"> Boolean to designate button pressed. true for pressed, false for released</param>
        internal void ToggleMode (UserScreen mode)
        {
            currentMode = mode;
            switch (mode)
            {
                case UserScreen.Tour:
                    TourMode.Visibility = Visibility.Visible;
                    ShutdownMode.Visibility = Visibility.Hidden;
                    kioskViewControl.Visibility = Visibility.Hidden;
                    ManualDriveMode.Visibility = Visibility.Hidden;   
                    AutoMode.Visibility = Visibility.Hidden;
                    break;
                case UserScreen.Shutdown:
                    ShutdownMode.Visibility = Visibility.Visible;
                    kioskViewControl.Visibility = Visibility.Hidden;
                    ManualDriveMode.Visibility = Visibility.Hidden;
                    TourMode.Visibility = Visibility.Hidden;
                    AutoMode.Visibility = Visibility.Hidden;
                    break;
                case UserScreen.Kiosk:
                    kioskViewControl.Visibility = Visibility.Visible;
                    ShutdownMode.Visibility = Visibility.Hidden;
                    ManualDriveMode.Visibility = Visibility.Hidden;
                    TourMode.Visibility = Visibility.Hidden;
                    AutoMode.Visibility = Visibility.Hidden;
                    break;
                case UserScreen.Manual:
                    ManualDriveMode.Visibility = Visibility.Visible;
                    ShutdownMode.Visibility = Visibility.Hidden;
                    TourMode.Visibility = Visibility.Hidden;
                    kioskViewControl.Visibility = Visibility.Hidden;
                    AutoMode.Visibility = Visibility.Hidden;
                    break;
                case UserScreen.MainMenu:
                    ManualDriveMode.Visibility = Visibility.Hidden;
                    ShutdownMode.Visibility = Visibility.Hidden;
                    TourMode.Visibility = Visibility.Hidden;
                    kioskViewControl.Visibility = Visibility.Hidden;
                    kioskPromptText.Visibility = Visibility.Hidden;
                    AutoMode.Visibility = Visibility.Hidden;
                    break;
                case UserScreen.Auto:
                    AutoMode.Visibility = Visibility.Visible;
                    ManualDriveMode.Visibility = Visibility.Hidden;
                    ShutdownMode.Visibility = Visibility.Hidden;
                    TourMode.Visibility = Visibility.Hidden;
                    kioskViewControl.Visibility = Visibility.Hidden;
                    break;

                
            }
        }

        /// <summary>
        /// Automatically switch to E-Stop mode upon E-stop trigger. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Shows the estop exit prompt
        /// </summary>
        /// <param name="p"> Boolean to designate button pressed. true for pressed, false for released</param>
        internal void ShowKioskGetOut()
        {
            kioskPromptText.Visibility = Visibility.Visible;
            kioskPromptTimer.Start();
        }

        
    }

}
