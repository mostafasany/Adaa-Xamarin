using AdaaMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class MasterPage : MasterDetailPage
    {
        MenuPage menuPage;

        private Color actionBarBackgroundColor = Color.FromRgb(42,82,107);
        private Color actionBarTextColor = Color.White;

        public MasterPage()
        {
            menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as AdaaMenuItem);
            BackgroundColor = Color.Blue;
            Master = menuPage;
            NavigationPage navPage = new NavigationPage(new AttendancePage());
            navPage.BarBackgroundColor = actionBarBackgroundColor;
            navPage.BarTextColor = actionBarTextColor;

            Detail = navPage;
        }

        void NavigateTo(AdaaMenuItem menu)
        {
            if (menu == null)
                return;

            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            NavigationPage navPage = new NavigationPage(displayPage);

            navPage.BarBackgroundColor = actionBarBackgroundColor;
            navPage.BarTextColor = actionBarTextColor;

            Detail = navPage;
            menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}