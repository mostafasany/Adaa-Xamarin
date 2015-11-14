using System;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public partial class MasterPage : MasterDetailPage
    {
        private MenuPage menuPage;
        private readonly INavigationService _navigationService;
        private AdaaPageItem _lastSelectedPage;
        public MasterPage()
        {
            _navigationService = Locator.Default.NavigationService;
            BackgroundColor = Color.Blue;
            SetMenuPage();
            Detail = MasterHelper.CreatePage(typeof(HomePage));

			AppSettings settings = new AppSettings ();
			if (string.IsNullOrEmpty (settings.UserToken)) {
				Navigation.PushModalAsync  ( new NavigationPage( new LoginPage ()));
			}
        }

        private void SetMenuPage()
        {
            menuPage = new MenuPage();
            menuPage.Menu.ItemSelected += OnMenuOnItemSelected;
            Master = menuPage;
        }

        private void OnMenuOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AdaaPageItem;
            if (item == null)
                return;
            //Update selected item color

            //Update lastSelected item color

            //set last selected to item
            _lastSelectedPage = item;
            NavigateTo(item);
        }

        private void NavigateTo(AdaaPageItem page)
        {
            if (page == null) return;
            var type = page.TargetType;
            Detail = MasterHelper.CreatePage(page.TargetType);
            menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}