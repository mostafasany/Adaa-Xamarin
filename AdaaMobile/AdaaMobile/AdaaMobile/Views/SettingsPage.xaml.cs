﻿using AdaaMobile.Strings;
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
           // LanguagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;

			LanguagePicker.Unfocused+= LanguagePicker_Unfocused;

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
