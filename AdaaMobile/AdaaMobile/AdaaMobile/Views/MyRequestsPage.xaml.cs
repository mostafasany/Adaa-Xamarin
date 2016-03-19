using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views
{
    public partial class MyRequestsPage : ContentPage
    {
        MyRequestsViewModel _viewModel;

        public MyRequestsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyRequestsViewModel;
            BindingContext = _viewModel;

            Title = AppResources.MyRequests;
			//MyRequestsList.ItemTapped+= MyRequestsList_ItemTapped;
        }

        void MyRequestsList_ItemTapped (object sender, ItemTappedEventArgs e)
        {
			
			_viewModel.RequestItemSelectedCommand.Execute (e.Item as Request);
        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_viewModel.LoadDayPassDataCommand.Execute (null);
		}
    }
}
