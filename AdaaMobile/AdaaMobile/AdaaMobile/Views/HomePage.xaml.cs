using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _homeViewModel;
        public HomePage()
        {
            InitializeComponent();
            _homeViewModel = Locator.Container.Resolve<HomeViewModel>();
            BindingContext = _homeViewModel;
        }

        private void PageItem_Tapped(object sender, EventArgs e)
        {
            var element = sender as View;
            if (element == null) return;
            var model = (element.BindingContext) as AdaaPageItem;
            _homeViewModel.PageClickedCommand.Execute(model);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            
        }
    }
}
