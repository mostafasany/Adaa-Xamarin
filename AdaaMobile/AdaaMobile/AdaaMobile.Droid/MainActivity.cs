using System;
using System.Globalization;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using AdaaMobile.Droid.Helpers;
using AdaaMobile.Helpers;
using AdaaMobile.ViewModels;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;
using Java.Util;
using Xamarin;
namespace AdaaMobile.Droid
{
    [Activity(Label = "ADAA", MainLauncher = false,
        Theme = "@style/MyTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static MainActivity Instance;
        private static bool IsFormsInitialized;
        protected override void OnCreate(Bundle bundle)
        {

            Insights.Initialize("b2655f07a3c842df659dcf2532c519804a88ec7d", this, false);
            base.OnCreate(bundle);
            RequestedOrientation = ScreenOrientation.Portrait;
            ActionBar.SetIcon(new ColorDrawable(Color.Transparent));
            Instance = this;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            ExtendedGridRenderer.Init();
            SvgImageRenderer.Init();

            //Create new locator instance.
            var locator = new Locator();

            IAppSettings settings = new AppSettings();
            if (!string.IsNullOrEmpty(settings.NextSelectedCultureName))
            {
                settings.SelectedCultureName = settings.NextSelectedCultureName;
                settings.NextSelectedCultureName = string.Empty;
            }
            //Get user selected culture from last run or default
            string culture = settings.SelectedCultureName;
            //Override system culture
            SetCulture(culture);

            LoadApplication(new App(locator));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Instance = null;
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

