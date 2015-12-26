using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;
using Xamarin.Forms;

namespace AdaaMobile.Views
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
