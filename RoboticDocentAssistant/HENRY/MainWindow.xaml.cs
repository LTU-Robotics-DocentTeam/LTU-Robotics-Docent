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


using System.ComponentModel;

using HENRY.Modules;
using HENRY.Modules.Sensors;

using HENRY.ModuleSystem;

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

        HallEffectSensorModule hem;
        ImpactSensorModule ism;

        ManualDrive mnd;

        public MainWindow()
        {
            hem = new HallEffectSensorModule();
            ism = new ImpactSensorModule();
            vm = new ViewModel();
            scm = new SerialCommModule();
            gsm = new GenericSensorModule();
            mnd = new ManualDrive();


            

            InitializeComponent();

            //ThingClass t = new ThingClass();
            //t.SomeText = "bitch";
            //t.SomeDic = new Dictionary<string, string>();
            //t.SomeDic.Add("egg", "poop");


            //TextBlock b = new TextBlock();

            //Binding binding = new Binding();
            //binding.Path = new PropertyPath("SomeDic[1]");
            //binding.Source = t;  // view model?

            //BindingOperations.SetBinding(b, TextBlock.TextProperty, binding);


            //thingpanel.Child = b;

            //t.SomeText = "bitch2";



            //Dictionary<string, LengarioModuleProperty> thing = LengarioModuleBase.GetTheBin();

            //String stringthing = "";

            //foreach(KeyValuePair<string, LengarioModuleProperty> i in thing)
            //{
            //    stringthing += i.Key + Environment.NewLine;
            //}
            //MessageBox.Show(stringthing);

        }

        private void userViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            userViewControl.DataContext = vm;
        }

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
        
    }
}
