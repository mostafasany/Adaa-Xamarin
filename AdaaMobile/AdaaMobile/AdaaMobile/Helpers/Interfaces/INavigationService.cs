using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
	public interface INavigationService
	{
		/// <summary>
		/// This will set the App.Current.MainPage
		/// </summary>
		/// <typeparam name="TPageType"></typeparam>
		/// <param name="pageType"></param>
		bool SetAppCurrentPage<TPageType> (TPageType pageType) where TPageType : Type;


		/// <summary>
		/// Navigates to page with back button presened
		/// </summary>
		/// <typeparam name="TPageType"></typeparam>
		/// <param name="pageType"></param>
		/// <returns></returns>
		bool NavigateToPage<TPageType> (TPageType pageTypes) where TPageType : Type;

		/// <summary>
		/// This will try to set details page in master,
		/// It will fail if the current page isn't master.
		/// </summary>
		/// <typeparam name="TPageType"></typeparam>
		/// <param name="pageType"></param>
		/// <param name="wrapInNavigation">Default is true</param>
		/// <returns></returns>
		bool SetMasterDetailsPage<TPageType> (TPageType pageType, bool wrapInNavigation = true) where TPageType : Type;

		/// <summary>
		/// Navigates back
		/// </summary>
		void GoBack ();
        
		//INavigation GetCurrentNavigation();
	}
}
