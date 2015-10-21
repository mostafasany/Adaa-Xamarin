using System;
using System.Globalization;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
    public class TruncateDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = (double)value;
            return decimal.Truncate(new decimal(d));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value.ToString();
        }
    }
}
