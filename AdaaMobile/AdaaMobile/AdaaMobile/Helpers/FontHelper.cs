using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class FontHelper
    {
        private static readonly double SmallSize;
        private static readonly double MicroSize;
        private static readonly double MilliSize;//:D becuase there is a gap between small and micro in Android
        private static readonly FontSizeConverter FontsizeConverter;
        static FontHelper()
        {
            FontsizeConverter = new FontSizeConverter();
            SmallSize = (double)FontsizeConverter.ConvertFrom("Small");
            MicroSize = (double)FontsizeConverter.ConvertFrom("Micro");

            MilliSize = (SmallSize + MicroSize) / 2 + Device.OnPlatform(-1, 1, 0);
        }

        public static double MilliFontSize { get { return MilliSize; } }
    }
}
