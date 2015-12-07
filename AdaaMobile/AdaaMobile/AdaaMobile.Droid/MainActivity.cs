using System;
using System.Globalization;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using AdaaMobile.Droid.Helpers;
using AdaaMobile.Helpers;
using AdaaMobile.ViewModels;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;
using Java.Util;
using Xamarin;
namespace AdaaMobile.Droid
{
    [Activity(Label = "AdaaMobile", Icon = "@drawable/icon", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

			Insights.Initialize("b2655f07a3c842df659dcf2532c519804a88ec7d", this, false);
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            ExtendedGridRenderer.Init();
            SvgImageRenderer.Init();
            //Create new locator instance.
            var locator = new Locator();
            ////Get user selected culture from last run or default
            //string culture = new AppSettings().SelectedCultureName;
            ////Override system culture
            //SetCulture(culture);

            LoadApplication(new App(locator));
        }

        private void SetCulture(string culture)
        {
            //Set .Net culture
            var netCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = netCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = netCulture;

            //Set Java Culture
            var androidCulture = culture.Replace("-", "_"); // turns pt-R into pt_BR
            var locale = new Locale(androidCulture);
            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
        }
    }
}

