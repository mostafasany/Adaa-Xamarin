using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class NewDayPassRequestPage : ContentPage
    {

        private DayPassViewModel _dayPassViewModel;
        public NewDayPassRequestPage()
        {
            InitializeComponent();
            _dayPassViewModel = ViewModels.Locator.Default.DayPassViewModel;
            BindingContext = _dayPassViewModel;
			LanguageButton.Clicked += LanguageButton_Clicked;

        }

        void LanguageButton_Clicked (object sender, EventArgs e)
        {
			StartTimePicker.Unfocus ();
			StartTimePicker.Focus ();
        }
    }
}
