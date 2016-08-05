using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;
using AdaaMobile.Models;
using AdaaMobile.Enums;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views
{
	public partial class DirectoryPage : ContentPage, IUserSelection
	{
		private readonly DirectoryViewModel _directoryViewModel;
		private DirectorySourceType _directorySourceType;
		private DirectoryAccessType _accessType;


		public DirectoryPage (DirectorySourceType directorySourceType, DirectoryAccessType accessType)
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");
			Title = AppResources.Directory;
			this._directorySourceType = directorySourceType;
			this._accessType = accessType;

			_directoryViewModel = Locator.Default.DirectoryViewModel;
			_directoryViewModel.UserSelectionChanged += (sender, emp) => {
				if (emp != null) {
					if (_accessType == DirectoryAccessType.Normal) {
						this.Navigation.PushAsync (new ProfilePage ());
					} else {
						try {
							this.Navigation.PopAsync ();
						} catch (Exception ex) {
						} finally {
							OnUserSelected (emp);
						}

					}
				}
			};
			_directoryViewModel.DirectorySourceType = directorySourceType;
			BindingContext = _directoryViewModel;
			EmployeeSearchBar.TextChanged += EmployeeSearchBar_TextChanged;
			HandleArabicLanguageFlowDirection ();

		}

		void HandleArabicLanguageFlowDirection ()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				EmployeeSearchBar.HorizontalTextAlignment = TextAlignment.End;
			}
		}

		private async void EmployeeSearchBar_TextChanged (object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty (EmployeeSearchBar.Text)) {
				_directoryViewModel.GroupEmployees ();
				EmployeesListView.ItemsSource = _directoryViewModel.GroupedEmployees;
				EmployeesListView.IsGroupingEnabled = true;
        
			} else {

				bool isValidSearch = _directoryViewModel.Search (EmployeeSearchBar.Text);
				if (isValidSearch) {
					EmployeesListView.IsGroupingEnabled = false;
                
					EmployeesListView.ItemsSource = _directoryViewModel.NotGroupedEmployees;
            
   
				} else {

				}

			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			_directoryViewModel.SelectedEmployee = null;
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			await Task.Delay (300);

			//Check if there data are already loaded to prevent multiple loading.
			if (_directoryViewModel.GroupedEmployees == null || _directoryViewModel.GroupedEmployees.Count == 0)
				_directoryViewModel.LoadEmployeesCommand.Execute (null);
		}

		#region User Selection Event

		public event EventHandler<Employee> UserSelected;

		protected virtual void OnUserSelected (Employee e)
		{
			var handler = UserSelected;
			if (handler != null)
				handler (this, e);
		}

		#endregion

		protected override bool OnBackButtonPressed ()
		{
			OnUserSelected (null);
			return base.OnBackButtonPressed ();          
		}
	}
}
