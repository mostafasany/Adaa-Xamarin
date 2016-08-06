using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class TaskDetails : ContentPage
	{
		private readonly MyTimeSheetViewModel _viewModel;


		public TaskDetails ()
		{
			InitializeComponent ();

			NavigationPage.SetBackButtonTitle (this, "");

			_viewModel = Locator.Default.MyAssigmentsViewModel;
			BindingContext = _viewModel.SelectedTask;

			Title = _viewModel.SelectedTask.ProcedureName;


		}


	}
}

