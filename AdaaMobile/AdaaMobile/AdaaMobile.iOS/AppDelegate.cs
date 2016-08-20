using System.Threading;
using AdaaMobile.Helpers;
using AdaaMobile.iOS.CustomRenderers;
using AdaaMobile.ViewModels;
using Foundation;
using UIKit;
using Xamarin;
namespace AdaaMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {


        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            SvgImageRenderer.Init();
            ExtendedGridRenderer.Init();
            LoadApplication(new App(new Locator()));

            Insights.Initialize("b2655f07a3c842df659dcf2532c519804a88ec7d");


            UINavigationBar.Appearance.BarTintColor = UIColor.White;
            UISwitch.Appearance.OnTintColor = UIColor.FromRGBA(0, 124, 133, 255);

            return base.FinishedLaunching(app, options);

        }



        public AppDelegate()
        {
            //SetLanaguge(new AppSettings().SelectedCultureName);
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);

            UIWindow window = new UIWindow(UIScreen.MainScreen.Bounds);



            IAppSettings settings = new AppSettings();
            if (!string.IsNullOrEmpty(settings.NextSelectedCultureName))
            {
                settings.SelectedCultureName = settings.NextSelectedCultureName;
                settings.NextSelectedCultureName = string.Empty;
            }
            string selectedCultureNAme = settings.SelectedCultureName;
            if (!string.IsNullOrEmpty(selectedCultureNAme))
            {
                var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
                var netLocale = selectedCultureNAme.Replace("_", "-");
                var ci = new System.Globalization.CultureInfo(netLocale);

                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }

        }

        private void SetLanaguge(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            SetValueForKey(NSArray.FromStrings(culture), new NSString("AppleLanguages"));
        }

        public override void FinishedLaunching(UIApplication application)
        {
        }
    }
}
