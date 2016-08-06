using Xamarin.Forms;
using AdaaMobile.ViewModels;
using System;

namespace AdaaMobile
{
	public partial class SelectedPendingTaskPage : ContentPage
	{
		MyPendingTasksViewModel _viewModel;

		public SelectedPendingTaskPage()
		{
			InitializeComponent ();

			NavigationPage.SetBackButtonTitle (this, "");

			_viewModel = Locator.Default.MyPendingTasksViewModel;
			BindingContext = _viewModel.SelectedPendingTask;

			Title = _viewModel.SelectedPendingTask.ProcedureName;

		}

		public void OpenTask_Clicked(object sender,EventArgs args)
		{
			_viewModel.OpenTaskCommand.Execute (null);
		}

	}
}

