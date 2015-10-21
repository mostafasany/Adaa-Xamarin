using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaaMobile.ViewModels;
using AdaaMobile.Views;
using Xamarin.Forms;

namespace AdaaMobile
{
    public class App : Application
    {
        public App(Locator locator)
        {
            //Build dependencies and set static instance of Container
            Locator.Container = locator.CreateContainer();
            //Set static instance of locator.
            Locator.Default = locator;
   ;

            // The root page of your application
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
