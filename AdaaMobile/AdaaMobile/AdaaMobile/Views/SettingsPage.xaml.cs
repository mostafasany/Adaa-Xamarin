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
            Title = AppResources.Settings;

			_settingsViewModel = Locator.Default.SettingsViewModel;
			BindingContext = _settingsViewModel;
			LanguageButton.Clicked+= LanguageButton_Clicked;
//			var tapGestureRecognizer = new TapGestureRecognizer();
//			tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
//			SelectedLanguageNameGrid.GestureRecognizers.Add(tapGestureRecognizer);
//			SelectedLanguageNameLabel.GestureRecognizers.Add(tapGestureRecognizer);
			SelectedLanguageNameLabel.Clicked += SelectedLanguageNameLabel_Clicked;

			//LanguagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;
			LanguagePicker.Unfocused += LanguagePicker_Unfocused;
			LanguagePicker.SelectedIndex = _settingsViewModel.SelectedLanguageIndex;

        }

        void LanguagePicker_Unfocused (object sender, FocusEventArgs e)
        {
			if (LanguagePicker.SelectedIndex != _settingsViewModel.SelectedLanguageIndex) {
				_settingsViewModel.SelectedLanguageIndex = LanguagePicker.SelectedIndex;
				_settingsViewModel.UpdateLanguage (LanguagePicker.SelectedIndex);
			}
        }

        void LanguagePicker_SelectedIndexChanged (object sender, EventArgs e)
        {
			_settingsViewModel.UpdateLanguage (LanguagePicker.SelectedIndex);
			LanguagePicker.Unfocus ();

        }

        void SelectedLanguageNameLabel_Clicked (object sender, EventArgs e)
        {
			LanguagePicker.Focus ();
        }

        void TapGestureRecognizer_Tapped (object sender, EventArgs e)
        {
			LanguagePicker.Focus ();
        }

        void LanguageButton_Clicked (object sender, EventArgs e)
        {
			LanguagePicker.Focus ();
        }

        
    }
}
