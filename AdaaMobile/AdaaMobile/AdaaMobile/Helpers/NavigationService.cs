using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Views.MasterView;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class NavigationService : INavigationService
    {

        public bool SetAppCurrentPage<TPageType>(TPageType pageType) where TPageType : Type
        {
            try
            {
                //if (Application.Current.MainPage != null && App.Current.MainPage.GetType() == typeof(TPageType)) return false;
                Page displayPage = (Page)Activator.CreateInstance(pageType);
                Application.Current.MainPage = displayPage;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetMasterDetailsPage<TPageType>(TPageType pageType, bool wrapInNavigation = true) where TPageType : Type
        {
            var master = Application.Current.MainPage as MasterDetailPage;
            if (master == null) return false;
            try
            {
                master.Detail = MasterHelper.CreatePage(pageType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
