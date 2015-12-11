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
