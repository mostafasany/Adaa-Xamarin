
using System.Globalization;
using AdaaMobile.Droid.Helpers;
using AdaaMobile.Helpers;
using Java.Util;
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

        public void UpdateCultureInfo(string cultureName)
        {
            ////Set .Net culture
            //var netCulture = new CultureInfo(cultureName);
            //System.Threading.Thread.CurrentThread.CurrentCulture = netCulture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = netCulture;

            ////Set Java Culture
            //var androidCulture = cultureName.Replace("-", "_"); // turns pt-R into pt_BR
            //var locale = new Locale(androidCulture);
            //Java.Util.Locale.Default = locale;

            //var config = new Android.Content.Res.Configuration { Locale = locale };
            //Forms.Context.Resources.UpdateConfiguration(config, Forms.Context.Resources.DisplayMetrics);

            var instance = MainActivity.Instance;
            if (instance != null)
            {
                instance.Recreate();
            }
        }
    }
}
