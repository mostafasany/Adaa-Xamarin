﻿using System;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
	public partial class DayPassPage : ContentPage
	{

		private Button _lastTappedTab;
		private DayPassViewModel _dayPassViewModel;
		private ToolbarItem _addToolBarItem;

		public DayPassPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");
			Title = AppResources.DayPass;
			_dayPassViewModel = ViewModels.Locator.Default.DayPassViewModel;
			BindingContext = _dayPassViewModel;
			SelectButton (MyRequestsButton, true);
			SelectButton (MyTasksButton, false);
			_lastTappedTab = MyRequestsButton;

			string addIcon = Device.OnPlatform ("note", "note.png", "note.png");

			Action action = () => {
				this.Navigation.PushAsync (new NewDayPassRequestPage ());
			};
			_addToolBarItem =
                new ToolbarItem ("", addIcon, action, ToolbarItemOrder.Primary);
			ToolbarItems.Add (_addToolBarItem);
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			NavigationPage.SetBackButtonTitle (this, string.Empty);
			try {
				_dayPassViewModel.LoadDayPassDataCommand.Execute (null);
			} catch (Exception ex) {
				//ignored
			}
		}


		#region Tabs Methods

		private void SelectButton (Button button, bool selected)
		{
			var darkColor = (Color)Resources ["TabColor"];

			if (selected) {
				button.BackgroundColor = darkColor;
				button.TextColor = Color.White;
			} else {
				button.BackgroundColor = Color.White;
				button.TextColor = darkColor;
			}
		}

		/// <summary>
		/// This method changes button backgroundcolor and textcolor based on pressed state.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTabTapped (object sender, EventArgs e)
		{
			var button = (Button)sender;
			if (button == _lastTappedTab)
				return;
			//Change color state of tapped button to Active
			SelectButton (button, true);
			//Change color state of tapped button to be not Active
			if (_lastTappedTab != null) {
				SelectButton (_lastTappedTab, false);
			}
			_lastTappedTab = button;

			//Switch to different modes based on tapped button
			if (button == MyRequestsButton) {
				ToolbarItems.Clear ();
				ToolbarItems.Add (_addToolBarItem);
				MyRequestsListGrid.IsVisible = true;
				MyTasksListGrid.IsVisible = false;
			} else if (button == MyTasksButton) {
				ToolbarItems.Clear ();

				MyRequestsListGrid.IsVisible = false;
				MyTasksListGrid.IsVisible = true;
			}
		}

		#endregion
	}
}
