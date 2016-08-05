using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class SelectedRequestPage : ContentPage
	{
		MyRequestsViewModel _viewModel;

		public SelectedRequestPage ()
		{
			InitializeComponent ();

			NavigationPage.SetBackButtonTitle (this, "");

			_viewModel = Locator.Default.MyRequestsViewModel;
			BindingContext = _viewModel;

			Title = _viewModel.SelectedRequest.service;

			DateTime date = DateTime.Parse (_viewModel.SelectedRequest.created);

			DateData.Value = date.ToString ("d MMM yyyy");

		}


	}
}

