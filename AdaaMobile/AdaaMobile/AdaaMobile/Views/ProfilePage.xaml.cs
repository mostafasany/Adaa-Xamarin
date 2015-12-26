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
            _profileViewModel = Locator.Default.ProfileViewModel;
            BindingContext = _profileViewModel;
        }


		public ProfilePage (uint userID)
		{
			InitializeComponent();
			_profileViewModel = Locator.Default.ProfileViewModel;
			_profileViewModel.SetOtherUserId (userID.ToString ());
			BindingContext = _profileViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			if (_profileViewModel.UserProfile == null)
            {
                _profileViewModel.LoadCommand.Execute(null);
            }
        }
    }


}
