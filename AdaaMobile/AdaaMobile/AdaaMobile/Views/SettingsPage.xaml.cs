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
            //LanguageButton.Clicked+= LanguageButton_Clicked;
            SelectedLanguageNameLabel.Clicked += SelectedLanguageNameLabel_Clicked;
            LanguagePicker.SelectedIndex = _settingsViewModel.SelectedLanguageIndex;
            LanguagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;

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

        //void LanguageButton_Clicked (object sender, EventArgs e)
        //{
        //    LanguagePicker.Unfocus();
        //    LanguagePicker.Focus ();
        //}


    }
}
