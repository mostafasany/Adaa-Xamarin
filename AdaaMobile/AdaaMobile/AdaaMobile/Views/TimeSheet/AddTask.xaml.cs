using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Views.TimeSheet;

namespace AdaaMobile
{

	public partial class AddTask : ContentPage
	{
		private readonly MyTimeSheetViewModel _viewModel;
		public AddTask ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.MyTimeSheetViewModel;
			BindingContext = _viewModel;

			Title = AppResources.EServices;

			LoadDuration ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_viewModel.PageLoadedCommand.Execute (null);
		}

		private void LoadDuration()
		{
			DurationPicker.Items.Clear ();
			for (int i = 0; i < 24; i++) {
				string value = i.ToString () + ":00";
				DurationPicker.Items.Add(value);
				value = i.ToString () + ":30";
				DurationPicker.Items.Add(value);
			}
			DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
		}

		private void Duration_OnClicked (object sender, EventArgs e)
		{
			DurationPicker.Unfocus ();
			DurationPicker.Focus ();
		}

		private void SelectTask_OnClicked (object sender, EventArgs e)
		{
			Locator.Default.NavigationService.NavigateToPage (typeof(SelectTask));
		}

		private void DurationPicker_SelectionIndexChanged (object sender, EventArgs e)
		{
			if (sender != null && sender is Picker) {
				var picker = sender as Picker;
				_viewModel.SelectedDuration = DurationPicker.Items[picker.SelectedIndex];
			}
		}
	}
}

