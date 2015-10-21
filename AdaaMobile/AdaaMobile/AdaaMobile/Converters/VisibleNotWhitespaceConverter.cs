﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
    public class VisibleNotWhitespaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null && !string.IsNullOrWhiteSpace(value.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("");
        }
    }
}
