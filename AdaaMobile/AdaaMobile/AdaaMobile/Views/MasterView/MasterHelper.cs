using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public class MasterHelper
    {
        private static readonly Color ActionBarBackgroundColor = Color.FromRgb(42, 82, 107);
        private static readonly Color ActionBarTextColor = Color.White;

        public static Page CreatePage<TPageType>(TPageType pageType, bool wrapInNavigation = true) where TPageType : Type
        {
            Page displayPage = (Page)Activator.CreateInstance(pageType);

            if (wrapInNavigation)
            {
                NavigationPage navPage = new NavigationPage(displayPage)
                {
                    BarBackgroundColor = ActionBarBackgroundColor,
                    BarTextColor = ActionBarTextColor
                };
                return navPage;
            }
            return displayPage;
        }
    }
}
