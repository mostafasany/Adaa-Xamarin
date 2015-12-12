using System;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public partial class AddaMasterPage : MasterDetailPage
    {

        private readonly INavigationService _navigationService;
        public AddaMasterPage()
        {
            _navigationService = Locator.Default.NavigationService;
            BackgroundColor = (Color)Application.Current.Resources["AppBackgroundNormal"];
            //Icon = Device.OnPlatform("menu", "menu.png", "menu");

            SetMenuPage();
            Detail = MasterHelper.CreatePage(typeof(HomePage));


        }

        private void SetMenuPage()
        {
            var master = new MasterMenuPage();
            master.ItemTapped += (sender, args) => IsPresented = false;

            master.Title = "Menu";
            Master = master;
        }


    }
}