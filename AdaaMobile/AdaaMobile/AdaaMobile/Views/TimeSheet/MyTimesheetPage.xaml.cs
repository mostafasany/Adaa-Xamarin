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

            RightButton.GestureRecognizers.Add(new TapGestureRecognizer(OnTapRightButton));
            LeftButton.GestureRecognizers.Add(new TapGestureRecognizer(OnTapLefttButton));

            Title = AppResources.TimeSheet_MyTimeSheet;
        }

        private void OnTapRightButton(View arg1, object arg2)
        {
            _viewModel.GetNextDay();
        }

        private void OnTapLefttButton(View arg1, object arg2)
        {
            _viewModel.GetPreviousDay();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
        }

        private void Day_Tapped(object sender, HorizontaListItemTappedEventArgs e)
        {
            var newDay = (DayWrapper)e.Item;
            if (newDay.IsDummy == false)
                _viewModel.SelecteDay(newDay);
        }
    }
}

