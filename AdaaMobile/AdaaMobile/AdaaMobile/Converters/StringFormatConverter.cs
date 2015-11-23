using System;
using System.Globalization;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter,  CultureInfo culture)
        {
            if (value == null) return null;
            // Retrieve the format string and use it to format the value.
            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(formatString, value);
            }

            // If the format string is null or empty, simply
            // call ToString() on the value.
            return value.ToString();
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            return value.ToString();
        }
    }
}
