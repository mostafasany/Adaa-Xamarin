using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _profileViewModel;
        public ProfilePage()
        {
            InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
            _profileViewModel = Locator.Default.ProfileViewModel;
            BindingContext = _profileViewModel;
        }


		public ProfilePage (string userId)
		{
			InitializeComponent();
			_profileViewModel = Locator.Default.ProfileViewModel;
			_profileViewModel.SetOtherUserId (userId);
			BindingContext = _profileViewModel;
			var tapRecognizer = new TapGestureRecognizer();
			tapRecognizer.Tapped += MobileNumber_OnTapped;
			MobileNumberField.GestureRecognizers.Add (tapRecognizer);

			var officeNumbertapRecognizer = new TapGestureRecognizer();
			officeNumbertapRecognizer.Tapped += OfficeNumber_OnTapped;
			OfficeNumberField.GestureRecognizers.Add (officeNumbertapRecognizer);


		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			if (_profileViewModel.UserProfile == null)
            {
                _profileViewModel.LoadCommand.Execute(null);
            }
        }

		private void MobileNumber_OnTapped(object sender, EventArgs e)
		{
			if(_profileViewModel.UserProfile
				!= null && !string.IsNullOrEmpty( _profileViewModel.UserProfile.MobileNum ))
			DependencyService.Get<IPhoneService> ().DialNumber (_profileViewModel.UserProfile.MobileNum);
		}

		void OfficeNumber_OnTapped (object sender, EventArgs e)
		{
			f(_profileViewModel.UserProfile
				!= null && !string.IsNullOrEmpty( _profileViewModel.UserProfile.OfficeNum ))
			DependencyService.Get<IPhoneService> ().DialNumber (_profileViewModel.UserProfile.OfficeNum);

		}
    }


}
