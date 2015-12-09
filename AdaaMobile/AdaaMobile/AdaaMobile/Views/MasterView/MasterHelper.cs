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
        private static readonly Color ActionBarBackgroundColor = Color.FromRgba(0, 124, 133, 1);
        private static readonly Color ActionBarTextColor = Color.White;

        public static Page CreatePage<TPageType>(TPageType pageType, bool wrapInNavigation = true) where TPageType : Type
        {

            try
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
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
