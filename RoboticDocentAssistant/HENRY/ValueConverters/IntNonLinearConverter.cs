using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Globalization;


namespace HENRY.ValueConverters
{
    //I created this simple converter for converting between string and integer
    //These converters are used in WPF Data Binding

    public class IntNonLinearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double x = System.Convert.ToDouble(value);
            double b = System.Convert.ToDouble(parameter);
            double thing = -1*((b*(b+1))/(x+b))+b+1;

            return thing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return 0;
            }
            catch
            {
                return -1;
            }

        }
    }
}