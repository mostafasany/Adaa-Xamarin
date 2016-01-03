using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Views.Authentication;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public class MasterHelper
    {
        private static readonly Color ActionBarBackgroundColor = (Color)App.Current.Resources["AppBackgroundNormal"];
        private static readonly Color ActionBarTextColor = Color.White;

        /// <summary>
        /// This method will create Details page and wraps it inside Navigation page.
        /// It's used for Master details page.
        /// </summary>
        /// <typeparam name="TPageType"></typeparam>
        /// <param name="pageType"></param>
        /// <param name="wrapInNavigation"></param>
        /// <returns></returns>
        public static Page CreatePage<TPageType>(TPageType pageType, bool wrapInNavigation = true) where TPageType : Type
        {
            try
            {
                //Page displayPage = (Page)Activator.CreateInstance(pageType);
                Page displayPage = (Page)CreateInnerPage(pageType);

                if (wrapInNavigation)
                {
                    NavigationPage navPage = new NavigationPage(displayPage)
                    {
                        BarBackgroundColor = ActionBarBackgroundColor,
                        BarTextColor = ActionBarTextColor
                    };



                    ////Hide nav bar in home, to show Welcome message center aligned in all platforms
                    //if (displayPage.GetType() == typeof(HomePage))
                    //{
                    //    NavigationPage.SetHasNavigationBar(displayPage, false);
                    //}
                    return navPage;
                }
                return displayPage;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Call this method each time you set Details of MasterDetailsPage, 
        /// To update side Menu Selection.
        /// </summary>
        /// <param name="pageType"></param>
        public static void UpdateSideMenuSelection(Type pageType)
        {
            try
            {
                var master = Application.Current.MainPage as MasterDetailPage;
                if (master == null) return;
                var menuPage = master.Master as MasterMenuPage;
                if (menuPage == null) return;
                menuPage.UpdateSelectedMenu(pageType);
            }
            catch (Exception)
            {
                //ignored
            }
        }

        /// <summary>
        /// It creates pages manually instead of using reflection, 
        /// as reflection has some issues in linking iniside Xamarin.
        /// </summary>
        /// <param name="pageType"></param>
        /// <returns></returns>
        private static Page CreateInnerPage(Type pageType)
        {
            if (pageType == typeof(HomePage)) return new HomePage();
            if (pageType == typeof(ProfilePage)) return new ProfilePage();
            if (pageType == typeof(AttendancePage)) return new AttendancePage();
            if (pageType == typeof(DirectoryPage)) return new DirectoryPage(Enums.DirectorySourceType.Directory, Enums.DirectoryAccessType.Normal);
            if (pageType == typeof(EServicesPage)) return new EServicesPage();
            if (pageType == typeof(ForgetPasswordPage)) return new ForgetPasswordPage();
            if (pageType == typeof(ITServiesPage)) return new ITServiesPage();
            if (pageType == typeof(LoginPage)) return new LoginPage();
            if (pageType == typeof(ITServiesPage)) return new ITServiesPage();
            if (pageType == typeof(MyRequestsPage)) return new MyRequestsPage();
            if (pageType == typeof(MyTasksPage)) return new MyTasksPage();
            if (pageType == typeof(OracleServicesPage)) return new OracleServicesPage();
            if (pageType == typeof(SettingsPage)) return new SettingsPage();
            if (pageType == typeof(SignUpPage)) return new SignUpPage();
            if (pageType == typeof(TimesheetPage)) return new TimesheetPage();
            if (pageType == typeof(UserServicesPage)) return new UserServicesPage();
            if (pageType == typeof(ChangePasswordPage)) return new ChangePasswordPage();
            return new HomePage();
        }
    }
}
