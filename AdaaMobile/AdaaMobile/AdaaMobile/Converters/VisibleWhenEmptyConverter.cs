using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
    public class VisibleWhenEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;

            //special handling for Ilist
            var list = value as IList;
            if (list != null) return list.Count == 0;

            return value.ToString().Length == 0;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("");
        }
    }
}
