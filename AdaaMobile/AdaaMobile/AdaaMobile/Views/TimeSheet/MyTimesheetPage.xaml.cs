﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class MyTimesheetPage : ContentPage
	{
		private readonly TimeSheetViewModel _viewModel;
		public MyTimesheetPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.TimeSheetViewModel;
			BindingContext = _viewModel;

			Title = AppResources.EServices;
		}

		private void ServicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (ServicesList.SelectedItem != null)
				ServicesList.SelectedItem = null;
		}
	}
}
