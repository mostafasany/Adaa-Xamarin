using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class RequestDriverPage : ContentPage
    {
        RequestDriverViewModel _viewModel;

        public RequestDriverPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.RequestDriverViewModel;
            BindingContext = _viewModel;

            Title = AppResources.RequestDriver;

            //Initialize picker and wire events
            ReasonTypePicker.Items.Add(AppResources.Work);
            ReasonTypePicker.Items.Add(AppResources.Personal);
            ReasonTypePicker.SelectedIndexChanged += ReasonTypePicker_SelectedIndexChanged;

            PriorityPicker.Items.Add(AppResources.Normal);
            PriorityPicker.Items.Add(AppResources.Meduim);
            PriorityPicker.Items.Add(AppResources.Urgent);
            PriorityPicker.SelectedIndexChanged += PriorityPicker_SelectedIndexChanged;

			SourcePicker.SelectedIndexChanged+= SourcePicker_SelectedIndexChanged;
			DestinationPicker.SelectedIndexChanged += DestinationPicker_SelectedIndexChanged;
            //Add submit action
            Action action = () =>
            {
                _viewModel.NewDriverRequestCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem("", Device.OnPlatform("sentico.png", "sent_ico.png", "sent_ico.png"), action, ToolbarItemOrder.Primary));

        }

        void DestinationPicker_SelectedIndexChanged (object sender, EventArgs e)
        {
			_viewModel.SelectedDestinationName = _viewModel.ClientsList [DestinationPicker.SelectedIndex].title;
        }

        void SourcePicker_SelectedIndexChanged (object sender, EventArgs e)
        {
			_viewModel.SelectedSourceName = _viewModel.ClientsList [SourcePicker.SelectedIndex].title;
        }

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();
			await _viewModel.GetClients();
			if(_viewModel.ClientsList != null && _viewModel.ClientsList.Count >0){
			foreach (var item in _viewModel.ClientsList) {
					SourcePicker.Items.Add(item.title);
					DestinationPicker.Items.Add(item.title);
			}
			}
		}

        private void Priority_Tapped(object sender, EventArgs e)
        {
            PriorityPicker.Unfocus();
            PriorityPicker.Focus();
        }

        private void ReasonType_Tapped(object sender, EventArgs e)
        {
            ReasonTypePicker.Unfocus();
            ReasonTypePicker.Focus();
        }


        private void SourceText_Tapped(object sender, EventArgs e)
        {
            SourcePicker.Unfocus();
            SourcePicker.Focus();
        }


        private void Destination_Tapped(object sender, EventArgs e)
        {
            DestinationPicker.Unfocus();
            DestinationPicker.Focus();
        }

        private void RequestDate_Tapped(object sender, EventArgs e)
        {
            RequestDatePicker.Unfocus();
            RequestDatePicker.Focus();
        }

        private void RequestTimeSpan_Tapped(object sender, EventArgs e)
        {
            RequestTimePicker.Unfocus();
            RequestTimePicker.Focus();
        }

        private void ReasonTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReasonTypePicker.SelectedIndex == 0)
            {
                _viewModel.ReasonType = "Work";
                _viewModel.LocalizedReasonType = AppResources.Work;
            }
            else
            {
                _viewModel.ReasonType = "Personal";
                _viewModel.LocalizedReasonType = AppResources.Personal;
            }
        }

        private void PriorityPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PriorityPicker.SelectedIndex == 0)
            {
                _viewModel.SelectedPiorityText = AppResources.Normal;
            }
            if (PriorityPicker.SelectedIndex == 1)
            {
                _viewModel.SelectedPiorityText = AppResources.Meduim;
            }
            if (PriorityPicker.SelectedIndex == 2)
            {
                _viewModel.SelectedPiorityText = AppResources.Urgent;
            }

        }

        private void Date_Selected(object sender, DateChangedEventArgs e)
        {
            _viewModel.RequestDate = (sender as DatePicker).Date;
        }
    }
}

