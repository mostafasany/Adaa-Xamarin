using System;
using System.Collections.Generic;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile
{
	public partial class ServiceDeskRequestsPage : ContentPage
	{
		ServiceDeskRequestsViewModel _viewModel;
		public ServiceDeskRequestsPage()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.ServiceDeskRequestsViewModel;
			BindingContext = _viewModel;

			Title = AppResources.ServiceDesk_MyRequests;

			Action action = () =>
		{
			filter();
		};
			ToolbarItems.Add(
				new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));
			RequestSearchBar.TextChanged += RequestSearchBar_TextChanged;

		}
		private void RequestSearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			(BindingContext as ServiceDeskRequestsViewModel).searchBy(RequestSearchBar.Text);

		}
		void filter()
		{
			FilterPicker.Unfocus();
			FilterPicker.Focus();

			if (FilterPicker.SelectedIndex < 0)
			{
				FilterPicker.Items.Clear();
				List<string> optionList = new List<string> { "All", "Active", "Canceled", "Closed", "Resolved"};

				for (int i = 0; i < optionList.Count; i++)
				{
					FilterPicker.Items.Add(optionList[i]);
				}

				FilterPicker.SelectedIndexChanged += FilterPicker_SelectionIndexChanged;
			}

		}

		void FilterPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			string status = "";
			try
			{
				var picker = sender as Picker;
				if (picker != null && picker.SelectedIndex >= 0)
				{
					status = FilterPicker.Items[picker.SelectedIndex];

				}
			}
			catch (Exception)
			{
			}
			finally
			{
				(BindingContext as ServiceDeskRequestsViewModel).FilterByStatus(status);
			}

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.PageLoadedCommand.Execute(null);
		}
	}
}
