using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class DelegationsPage : ContentPage
    {
        private DelegationViewModel _delegationViewModel;
        public DelegationsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _delegationViewModel = Locator.Default.DelegationViewModel;
            BindingContext = _delegationViewModel;
            Title = AppResources.MyDelegationsCap;

            Action action = () =>
            {
                this.Navigation.PushAsync(new NewDelegationPage());
            };
            string addIcon = Device.OnPlatform("note", "note.png", "note.png");
            ToolbarItems.Add(
                new ToolbarItem("", addIcon, action, ToolbarItemOrder.Primary));

            DelegationsList.ItemTapped += DelegationsList_ItemTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _delegationViewModel.LoadDayPassDataCommand.Execute(null);
        }
        private void DelegationsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Clear color selection
            DelegationsList.SelectedItem = null;

            Models.Response.Delegation delegation = (e.Item as Models.Response.Delegation);
            this.Navigation.PushAsync(new DelegationDetailsPage(delegation));
        }
    }
}
