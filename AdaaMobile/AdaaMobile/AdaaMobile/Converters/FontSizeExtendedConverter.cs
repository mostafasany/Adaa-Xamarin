using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
    public class FontSizeExtendedConverter : IValueConverter
    {
        private double smallSize;
        private double microSize;
        private double milliSize;//:D becuase there is a gap between small and micro in Android
        FontSizeConverter fontsizeConverter;
        public FontSizeExtendedConverter()
        {

            fontsizeConverter = new FontSizeConverter();
            smallSize = (double)fontsizeConverter.ConvertFrom("Small");
            microSize = (double)fontsizeConverter.ConvertFrom("Micro");
            milliSize = (smallSize + microSize) / 2 + Device.OnPlatform(0, 1, 0);
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.Equals((string)value, "milli"))
            {
                return milliSize;
            }
            return fontsizeConverter.ConvertFrom(culture, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
