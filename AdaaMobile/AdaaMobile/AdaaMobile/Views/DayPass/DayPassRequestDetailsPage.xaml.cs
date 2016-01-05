using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
    public partial class DayPassRequestDetailsPage : ContentPage
    {
        private DayPassRequest request;


        public DayPassRequestDetailsPage(DayPassRequest request)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            this.request = request;
            this.BindingContext = request;
            Title = AppResources.RequestDetails;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, string.Empty);
        }
    }
}
