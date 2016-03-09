using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class RequestDriverPage : ContentPage
	{
		public RequestDriverPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle(this, "");

//			_viewModel = Locator.Default.EServicesViewModel;
//			BindingContext = _viewModel;

			Title = AppResources.RequestDriver;
		}
	}
}

