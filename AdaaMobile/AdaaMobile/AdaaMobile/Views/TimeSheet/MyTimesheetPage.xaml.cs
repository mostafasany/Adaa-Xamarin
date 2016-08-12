using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Controls;
using AdaaMobile.Models;

namespace AdaaMobile
{

    public partial class MyTimesheetPage : ContentPage
    {
        private readonly MyTimeSheetViewModel _viewModel;
        public MyTimesheetPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyTimeSheetViewModel;
            BindingContext = _viewModel;

            Title = AppResources.TimeSheet_MyTimeSheet;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
        }

        private void Day_Tapped(object sender, HorizontaListItemTappedEventArgs e)
        {
            //Change color states of clicked day
            var newDay = (DayWrapper)e.Item;
            if (newDay.IsDummy == false)
                _viewModel.SelecteDay(newDay);
        }
    }
}

