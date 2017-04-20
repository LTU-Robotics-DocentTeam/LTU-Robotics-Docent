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

    public class CountdownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double input = System.Convert.ToDouble(value);
            Double max = System.Convert.ToDouble(parameter);

            Double result = (1 - input) * max;
            
            Int32 roundedResult = System.Convert.ToInt32(Math.Ceiling(result));

            return roundedResult.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToInt32(value);
            }
            catch
            {
                return -1;
            }
            
        }
    }
}
