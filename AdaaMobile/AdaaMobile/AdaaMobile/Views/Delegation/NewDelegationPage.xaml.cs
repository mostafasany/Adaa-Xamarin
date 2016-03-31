﻿using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class NewDelegationPage : ContentPage
    {
        private NewDelegationViewModel _newDelegationViewModel;

        public NewDelegationPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _newDelegationViewModel = Locator.Default.NewDelegationViewModel;
            BindingContext = _newDelegationViewModel;
            Title = AppResources.NewDelegationCap;
            Action action = () =>
            {
                _newDelegationViewModel.NewDelegationCommand.Execute(null);
            };
			string sendIcon = Device.OnPlatform("right.png", "sent_ico.png", "sent_ico.png");
            ToolbarItems.Add(
                new ToolbarItem("", sendIcon, action, ToolbarItemOrder.Primary));

        }
    }
}
