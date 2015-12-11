using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaaMobile.ViewModels;
using AdaaMobile.Views;
using AdaaMobile.Views.MasterView;
using Xamarin.Forms;
using AdaaMobile.Helpers;

namespace AdaaMobile
{
    public partial class App : Application
    {
        public App(Locator locator)
        {
            InitializeComponent();
            //Build dependencies and set static instance of Container
            Locator.Container = locator.CreateContainer();
            //Set static instance of locator.
            Locator.Default = locator;


            //MainPage = new AddaMasterPage();
            IAppSettings settings = Locator.Default.AppSettings;

            if (!settings.IsCultureSet)
            {
                MainPage = new Views.ChooseLanguagePage();
            }
            else if (string.IsNullOrEmpty(settings.UserToken))
            {
                MainPage = new LoginPage();
            }
            else
            {
                MainPage = new AddaMasterPage();
            }



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
