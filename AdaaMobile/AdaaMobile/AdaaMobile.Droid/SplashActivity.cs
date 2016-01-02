using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AdaaMobile.Droid
{
    [Activity(Label = "ADAA", Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash", NoHistory = true, MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestedOrientation = ScreenOrientation.Portrait;
            // Create your application here
            StartActivity(typeof(MainActivity));
        }
    }
}