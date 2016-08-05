using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Constants;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.TimeSheet
{
	public partial class SelectTask : ContentPage
    {
		private readonly SelectTaskViewModel _viewModel;
		public SelectTask()
        {
            InitializeComponent();
            Title = "Tasks";
			this._viewModel = Locator.Default.SelectTaskViewModel;
            BindingContext = _viewModel;
        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_viewModel.PageLoadedCommand.Execute (null);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		private void ItemTapped(object sender, object e)
		{
			
		}

    }
}
