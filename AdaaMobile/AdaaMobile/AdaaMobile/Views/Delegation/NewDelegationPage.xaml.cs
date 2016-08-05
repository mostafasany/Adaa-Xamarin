using System;
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
			string sendIcon = Device.OnPlatform("right.png", "right.png", "right.png");
            ToolbarItems.Add(
                new ToolbarItem("", sendIcon, action, ToolbarItemOrder.Primary));

			HandleArabicLanguageFlowDirection ();

        }

		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				Grid.SetColumn (lblRuleStatus, 1);
				Grid.SetColumn (ReturnTodaySwitch, 0);

			}
		}

    }
}
