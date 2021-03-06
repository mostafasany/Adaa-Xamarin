﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Controls;
using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;
using Xamarin.Forms.Xaml;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AttendancePage : ContentPage
	{
		private readonly AttendanceViewModel _attendanceViewModel;

		private Button _lastTappedTab;

		#region List Related fields

		private const string DayLabelName = "DayLabel";
		private const string MonthLabelName = "MonthLabel";
		private const string DayBackName = "DayBack";
		private const string DayBackNormalKey = "NormalBackColor";

		#endregion

		public AttendancePage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");
			_attendanceViewModel = Locator.Default.AttendanceViewModel;
			BindingContext = _attendanceViewModel;

			//TO Avoid that Date is fired on iOS before pressing Done
			MinDatePicker.Date = _attendanceViewModel.StartDate;
			MinDatePicker.Unfocused += MinDatePicker_Unfocused;

			MaxDatePicker.Date = _attendanceViewModel.EndDate;
			MaxDatePicker.Unfocused += MaxDatePicker_Unfocused;

			//Select Tabs
			SelectButton (AttendanceButton, true);
			SelectButton (ExceptionsButton, false);
			_lastTappedTab = AttendanceButton;


			if (LoggedUserInfo.CurrentUserProfile != null) {
				Title = LoggedUserInfo.CurrentUserProfile.DisplayName;
			} else {
				//Title = AppResources.Attendance;
			}
			DayPassBtn.Clicked += DayPassBtn_Clicked;
			DelegationBtn.Clicked += DelegationBtn_Clicked;

            

			_attendanceViewModel.PropertyChanged += _attendanceViewModel_PropertyChanged;

			HandleArabicLanguageFlowDirection ();
		}


		void HandleArabicLanguageFlowDirection ()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				lblFirstLLoc.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblFirstLLoc, 2);

				lblFirstSeenLocResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblFirstSeenLocResult, 2);

				lblLastSeenLoc.HorizontalOptions = LayoutOptions.Start;
				Grid.SetColumn (lblLastSeenLoc, 1);

				lblLastSeenLocResult.HorizontalOptions = LayoutOptions.Start;
				Grid.SetColumn (lblLastSeenLocResult, 1);


				lblDurtation.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblDurtation, 2);

				lblDurtationResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblDurtationResult, 2);


				lblLastSeen.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblLastSeen, 1);

				lblLastSeenResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblLastSeenResult, 1);


				lblFirstSeen.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblFirstSeen, 0);

				lblFirstSeenResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblFirstSeenResult, 0);

				lblExcepFirsSeenLoc.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepFirsSeenLoc, 2);

				lblExcepFirsSeenLocResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepFirsSeenLocResult, 2);

				lblExcepLate.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepLate, 2);

				lblExcepLateResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepLateResult, 2);


				lblExcepRemaining.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepRemaining, 1);

				lblExcepRemainingResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepRemainingResult, 1);


				lblExcepDuration.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepDuration, 0);

				lblExcepDurationResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblExcepDurationResult, 0);

			}
		}

		private void AddSubToolBarItem ()
		{
			if (ToolbarItems != null && ToolbarItems.Count > 0)
				return;
			
			Action action = () => {
				if (_attendanceViewModel.SubordinateList != null) {
					_attendanceViewModel.isSubordinateDataShown = true;
				
					Locator.Default.NavigationService.NavigateToPage (typeof(SubAttendancePage));
				} else {
					Locator.Default.DialogManager.DisplayAlert (AppResources.ApplicationName, AppResources.YouHaveNoSubordinates, AppResources.Ok);

				}


			};
			ToolbarItems.Add (
				new ToolbarItem ("", "subordinate.png", action, ToolbarItemOrder.Primary));
		}


		void _attendanceViewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "EndDate") {
				MaxDatePicker.Date = _attendanceViewModel.EndDate;

			} else if (e.PropertyName == "StartDate") {
				MinDatePicker.Date = _attendanceViewModel.StartDate;
			}
		}

		private void DelegationBtn_Clicked (object sender, EventArgs e)
		{
			this.Navigation.PushAsync (new Delegation.DelegationsPage ());
		}

		private void DayPassBtn_Clicked (object sender, EventArgs e)
		{
			this.Navigation.PushAsync (new DayPass.DayPassPage ());
		}

		void MaxDatePicker_Unfocused (object sender, FocusEventArgs e)
		{
			DatePicker datePicker = (sender as DatePicker);
			if (datePicker != null && _attendanceViewModel != null)
				_attendanceViewModel.EndDate = datePicker.Date;
		}

		void MinDatePicker_Unfocused (object sender, FocusEventArgs e)
		{
			DatePicker datePicker = (sender as DatePicker);
			if (datePicker != null && _attendanceViewModel != null)
				_attendanceViewModel.StartDate = datePicker.Date;
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			NavigationPage.SetBackButtonTitle (this, "");
			try {
				if (_attendanceViewModel.CurrentAttendance == null) {
					await _attendanceViewModel.PopulateAttendanceDaysAsync ();
				}

				if (_attendanceViewModel.SubordinateList == null) {
					bool isHaveSubordinates = await _attendanceViewModel.GetSubordinateList ();

				}
				_attendanceViewModel.isSubordinateDataShown = false;

				AddSubToolBarItem ();
			} catch {
				//ignored
			}
		}

		#region DatePickers

		private void EndDate_Clicked (object sender, EventArgs e)
		{
			MaxDatePicker.Unfocus ();
			MaxDatePicker.Focus ();
			MaxDatePicker.Date = _attendanceViewModel.EndDate;
		}

		private void StartDate_Clicked (object sender, EventArgs e)
		{
			MinDatePicker.Unfocus ();
			MinDatePicker.Focus ();
			MinDatePicker.Date = _attendanceViewModel.StartDate;
		}

		#endregion

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
		private async void OnTabTapped (object sender, EventArgs e)
		{
			if (_attendanceViewModel.IsBusy)
				return;
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
			if (button == AttendanceButton && _attendanceViewModel.AttendanceMode != AttendanceMode.Attendance) {
				await _attendanceViewModel.SwitchMode (AttendanceMode.Attendance);
               
			} else if (button == ExceptionsButton && _attendanceViewModel.AttendanceMode != AttendanceMode.Exceptions) {
				_attendanceViewModel.SwitchMode (AttendanceMode.Exceptions);
			}
		}

		#endregion


		/// <summary>
		/// Changes color state and triggers new load of details.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Day_Tapped (object sender, HorizontaListItemTappedEventArgs e)
		{
			//Change color states of clicked day
			var newDay = (DayWrapper)e.Item;
			if (newDay.IsDummy == false)
				_attendanceViewModel.SelecteDay (newDay);
		}

        

		private void Date_Selected (object sender, DateChangedEventArgs e)
		{
			//_attendanceViewModel.PopulateAttendanceDays();
		}
	}
}
