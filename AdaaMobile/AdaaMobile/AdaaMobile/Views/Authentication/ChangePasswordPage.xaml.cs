using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Authentication
{
    public partial class ChangePasswordPage : ContentPage
    {
        private readonly ChangePasswordViewModel _changePasswordModel;

        public ChangePasswordPage()
        {
            InitializeComponent();
			_changePasswordModel = Locator.Default.ChangePasswordViewModel;
            BindingContext = _changePasswordModel;

			Title = AppResources.ChangePassword;
            Action action = () =>
            {
                _changePasswordModel.ChangePasswordCommand.Execute(null);
            };
            ToolbarItems.Add(
				new ToolbarItem(AppResources.Save, "", action, ToolbarItemOrder.Primary));
			
            //Work-around for iOS for cut-images
            if (Device.OS == TargetPlatform.iOS)
            {
                iosBackgroundImage.IsVisible = true;
            }
            else
            {
                iosBackgroundImage.IsVisible = false;
            }

        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
//			var master = Application.Current.MainPage as MasterDetailPage;
//			if (master != null) {
//				Action action = () =>
//				{
//					_changePasswordModel.ChangePasswordCommand.Execute(null);
//				};
//				master.ToolbarItems.Add(
//					new ToolbarItem(AppResources.Save, "icon.png", action, ToolbarItemOrder.Primary));
//			}
		}
    }
}
