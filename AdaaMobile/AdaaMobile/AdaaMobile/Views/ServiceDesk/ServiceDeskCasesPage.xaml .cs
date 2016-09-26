using System;
using System.Collections.Generic;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile
{
	public partial class ServiceDeskCasesPage : ContentPage
	{
		ServiceDeskCasesViewModel _viewModel;
		public ServiceDeskCasesPage()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.ServiceDeskCasesViewModel;
			BindingContext = _viewModel;

			Title = AppResources.ServiceDesk_MyCases;
			Action action = () =>
		{
			filter();
		};
			ToolbarItems.Add(
				new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));
			CassesSearchBar.TextChanged += CassesSearchBar_TextChanged;

		}

		private  void CassesSearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			(BindingContext as ServiceDeskCasesViewModel).searchBy(CassesSearchBar.Text);

		}
		void filter()
		{
			FilterPicker.Unfocus();
			FilterPicker.Focus();

			if (FilterPicker.SelectedIndex < 0)
			{
				FilterPicker.Items.Clear();
				List<string> optionList = new List<string> { "All", "Pending", "In Progress", "Completed", "Faild", "Accepted", "Rejected" };

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
				(BindingContext as ServiceDeskCasesViewModel).FilterByStatus(status);
			}

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.PageLoadedCommand.Execute(null);
		}
	}
}
