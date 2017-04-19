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

namespace HENRY.Views
{
    /// <summary>
    /// Interaction logic for DevView.xaml
    /// </summary>
    public partial class DevView : UserControl
    {
        public enum DevScreen { Manual, Testing };

        public DevScreen currentmode = DevScreen.Testing;
        
        public DevView()
        {
            InitializeComponent();
        }

        public void SwitchMode(DevScreen newmode)
        {
            currentmode = newmode;
            switch (newmode)
            {
                case DevScreen.Manual: modeName.Text = "Manual Drive mode";
                    break;
                case DevScreen.Testing: modeName.Text = "Testing Mode";
                    break;
                default:
                    break;
            }
        }
    }
}
