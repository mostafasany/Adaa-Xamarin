
using AdaaMobile.Droid.Helpers;
using AdaaMobile.Helpers;
using Xamarin.Forms;
[assembly: Dependency(typeof(Localize))]

namespace AdaaMobile.Droid.Helpers
{
    public class Localize : ILocalize
    {
        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return new System.Globalization.CultureInfo(netLanguage);
        }
    }
}
