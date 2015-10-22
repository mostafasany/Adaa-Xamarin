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

        public MasterPage()
        {
            menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as AdaaMenuItem);
            BackgroundColor= Color.Blue;
            Master = menuPage;
            Detail = new NavigationPage(new HomePage());
        }

        void NavigateTo(AdaaMenuItem menu)
        {
            if (menu == null)
                return;

            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            Detail = new NavigationPage(displayPage);
            
            menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}