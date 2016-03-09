using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class RequestOfficeMaintenancePage : ContentPage
	{
		public RequestOfficeMaintenancePage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle(this, "");

//			_viewModel = Locator.Default.EServicesViewModel;
//			BindingContext = _viewModel;

			Title = AppResources.RequestOfficeMaintenance;
		}
	}
}

