using Xamarin.Forms;
using AdaaMobile.ViewModels;
using System;

namespace AdaaMobile
{
	public partial class SelectedServiceRequestPage : ContentPage
	{
		ServiceDeskRequestsViewModel _viewModel;

		public SelectedServiceRequestPage()
		{
			InitializeComponent();

			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.ServiceDeskRequestsViewModel;
			BindingContext = _viewModel.SelectedRequests;

			Title = _viewModel.SelectedRequests.Title;

			Action action = () =>
				{
					EditSelectedTask();
				};
			ToolbarItems.Add(
				new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));
		}
		private async void EditSelectedTask()
		{
		}

	}
}

