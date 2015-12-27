using AdaaMobile.Models;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
    public partial class DayPassRequestDetailsPage : ContentPage
    {
        private DayPassRequest request;


        public DayPassRequestDetailsPage(DayPassRequest request)
        {
            InitializeComponent();
            this.request = request;
            DepartureTimeLabel.Text = request.StartTime;
            ExpectedReturnTimeLabel.Text = request.EndTime;
            ReasonTypeLabel.Text = request.ReasonType;

        }
    }
}
