using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AdaaMobile.Helpers;
using AdaaMobile.ViewModels;
using Foundation;
using UIKit;

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
            LoadApplication(new App(new Locator()));


            return base.FinishedLaunching(app, options);
        }

        public AppDelegate()
        {
            //SetLanaguge(new AppSettings().SelectedCultureName);
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, true);
			UIWindow  window = new UIWindow (UIScreen.MainScreen.Bounds);

			new AppSettings ().SelectedCultureName = "ar-EG";
			string selectedCultureNAme = new AppSettings ().SelectedCultureName;
			selectedCultureNAme = "ar-EG";

			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(selectedCultureNAme);
			Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(selectedCultureNAme);


			//			viewController = new StartingLanguageViewcontroller ();
			//			UINavigationBar.Appearance.SetBackgroundImage (UIImage.FromBundle ("app_bar.png"), UIBarMetrics.Default);
			//			navigation = new UINavigationController (viewController);
			//			navigation.NavigationBar.TintColor = UIColor.White;
			//			navigation.NavigationBar.TitleTextAttributes =  new UIStringAttributes{ForegroundColor = UIColor.White}; 
			//			window.RootViewController = navigation;

        }

        private void SetLanaguge(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            SetValueForKey(NSArray.FromStrings(culture), new NSString("AppleLanguages"));
        }

		public override void FinishedLaunching (UIApplication application)
		{



		}

    }
}
