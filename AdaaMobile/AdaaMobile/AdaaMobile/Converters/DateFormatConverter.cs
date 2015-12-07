using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Helpers;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Converters
{
    public class DateFormatConverter : IValueConverter
    {
        private CultureInfo CurrentCulture;

        public DateFormatConverter()
        {
            CurrentCulture = new CultureInfo(Locator.Default.AppSettings.SelectedCultureName);
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime || value is DateTimeOffset) || parameter == null) return value;

            //TODO:Check why UI thread still the same as os culture
            culture = CurrentCulture;
            string result;
            if (value is DateTime)
            {
                var date = (DateTime)value;
                result = date.ToString(parameter.ToString(), culture);
            }
            else
            {
                var date = (DateTimeOffset)value;
                result = date.ToString(parameter.ToString(), culture);
            }

            //Correct character that breaks direction of arabic RTL like -
            if (culture.Name.StartsWith("ar"))
            {
                //const string ltrMark = "\u200E";
                const string rtlMark = "\u200F";

                try
                {
                    if (result.Contains("-") || result.Contains("/") || result.Contains(" "))
                    {

                        //result = Regex.Replace(result, "[!-/]", "$&" + rtlMark);
                        ////replace matched char with RTLcode after it - R&D to make sure it works with other chars
                        ////link:http://stackoverflow.com/questions/12630566/parsing-through-arabic-rtl-text-from-left-to-right
                        StringBuilder builder = new StringBuilder();
                        builder.Append(rtlMark);
                        for (int i = 0; i < result.Length; i++)
                        {
                            char c = result[i];
                            if (c == '-' || c == '/' || c == ' ')
                            {
                                builder.Append(c);
                                builder.Append(rtlMark);
                            }
                            else
                            {
                                builder.Append(c);
                            }
                        }
                        result = builder.ToString();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
