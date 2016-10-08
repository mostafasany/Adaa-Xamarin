using AdaaMobile.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views
{
    public partial class SettingsPage : ContentPage
    {

        private readonly SettingsViewModel _settingsViewModel;


        public SettingsPage()
        {
            InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
            Title = AppResources.Settings;

            _settingsViewModel = Locator.Default.SettingsViewModel;
            BindingContext = _settingsViewModel;
            //LanguageButton.Clicked+= LanguageButton_Clicked;
            SelectedLanguageNameLabel.Clicked += SelectedLanguageNameLabel_Clicked;
            LanguagePicker.SelectedIndex = _settingsViewModel.SelectedLanguageIndex;
			if (Device.OS == TargetPlatform.Android)
				LanguagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;
			else {
				LanguagePicker.Unfocused+= LanguagePicker_Unfocused;
			}
			HandleArabicLanguageFlowDirection();
			VersionLabel.Text = AppResources.AppVersion + "2.0.0.0";//+DependencyService.Get<IPhoneService>().GetVersion();

		}

		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				VersionLabel.HorizontalOptions = LayoutOptions.End;
				LanguageButton.HorizontalOptions = LayoutOptions.End;
				SelectedLanguageNameLabel.HorizontalOptions = LayoutOptions.End;
			}
		}
        void LanguagePicker_Unfocused (object sender, FocusEventArgs e)
        {
			_settingsViewModel.UpdateLanguage(LanguagePicker.SelectedIndex);
        }


        void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settingsViewModel.UpdateLanguage(LanguagePicker.SelectedIndex);

        }

        void SelectedLanguageNameLabel_Clicked(object sender, EventArgs e)
        {
            LanguagePicker.Unfocus();
            LanguagePicker.Focus();
        }


    }
}
