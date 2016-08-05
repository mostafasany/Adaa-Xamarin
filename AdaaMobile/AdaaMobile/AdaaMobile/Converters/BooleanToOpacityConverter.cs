using System;
using System.Globalization;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
	public class BooleanToOpacityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			if (value == null)
				return 1;
			
			var val = (bool)value;

			if(val)
			{
				return 0.5;
			}
			else
			{
				return 1;
			}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
