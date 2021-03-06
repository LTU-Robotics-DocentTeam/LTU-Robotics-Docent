﻿using System;
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

    public class ShiftedScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter) + 1;
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