using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using AdaaMobile.Models.Response;
using AdaaMobile.Views.EServices;

namespace AdaaMobile.Views
{
    public partial class MyRequestsPage : ContentPage
    {
        MyRequestsViewModel _viewModel;
		private ToolbarItem _addToolBarItem;

        public MyRequestsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyRequestsViewModel;
            BindingContext = _viewModel;

            Title = AppResources.MyRequests;

			string addIcon = Device.OnPlatform("note", "note.png", "note.png");

			Action action = () =>
			{
				this.Navigation.PushAsync(new EServicesPage());
			};
			_addToolBarItem =
				new ToolbarItem("", addIcon, action, ToolbarItemOrder.Primary);
			ToolbarItems.Add(_addToolBarItem);

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
