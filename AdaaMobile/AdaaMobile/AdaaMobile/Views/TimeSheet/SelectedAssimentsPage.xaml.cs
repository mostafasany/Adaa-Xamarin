using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class SelectedAssimentsPage : ContentPage
	{
		MyAssigmentsViewModel _viewModel;

		public SelectedAssimentsPage ()
		{
			InitializeComponent ();

			NavigationPage.SetBackButtonTitle (this, "");

			_viewModel = Locator.Default.MyAssigmentsViewModel;
			BindingContext = _viewModel.SelectedAssignment;

			Title = _viewModel.SelectedAssignment.Title;


		}


	}
}

