using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Views.MasterView;
using Xamarin.Forms;
using AdaaMobile.Views;

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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool NavigateToPage<TPageType>(TPageType pageType) where TPageType : Type
        {
            try
            {
				Page displayPage ;

				if (pageType == typeof(DirectoryPage)) 
					displayPage = 	new DirectoryPage(Enums.DirectorySourceType.Directory, Enums.DirectoryAccessType.Normal);
				else
					displayPage  = (Page)Activator.CreateInstance(pageType);
                //if (Application.Current.MainPage != null && App.Current.MainPage.GetType() == typeof(TPageType)) return false;
                if (Application.Current.MainPage is MasterDetailPage)
                {
                    (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(displayPage);
                }
                else
                {
                    (Application.Current.MainPage as Page).Navigation.PushAsync(displayPage);
                }
                return true;
            }
            catch (Exception ex)
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
                MasterHelper.UpdateSideMenuSelection(pageType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void GoBack()
        {
            try
            {
                if (Application.Current.MainPage is MasterDetailPage)
                {
                    (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PopAsync();
                }
                else
                {
                    (Application.Current.MainPage as Page).Navigation.PopAsync();
                }

            }
            catch (Exception ex)
            {
                ///TODO Log this error
            }

        }
    }
}
