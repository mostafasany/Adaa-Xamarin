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
using Xamarin.Forms.Platform.Android;

namespace AdaaMobile.Droid
{
    [Activity(Label = "ADAA", MainLauncher = false,
        Theme = "@style/MyTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {

        public static MainActivity Instance;
        private static bool IsFormsInitialized;

        protected override void OnCreate(Bundle bundle)
        {

            //For Tracking Bugs
            Insights.Initialize("b2655f07a3c842df659dcf2532c519804a88ec7d", this, false);
            base.OnCreate(bundle);

            Toolbar mToolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            mToolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_white_24dp);
            //Limit to only Portrait orientation
            RequestedOrientation = ScreenOrientation.Portrait;
            //ActionBar.SetIcon(new ColorDrawable(Color.Transparent));

            //Work around used to recreate Activity when user change language in Forms at first time
            Instance = this;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            //For Design Material
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.mytoolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            ImageCircleRenderer.Init();
            ExtendedGridRenderer.Init();
            SvgImageRenderer.Init();

            //Create new locator instance.
            var locator = new Locator();
            UpdateLanguage();
            LoadApplication(new App(locator));

        }

        protected override void OnResume()
        {
            base.OnResume();
            Toolbar mToolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            if (mToolbar != null)
                mToolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_white_24dp);
        }

        private void UpdateLanguage()
        {
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

