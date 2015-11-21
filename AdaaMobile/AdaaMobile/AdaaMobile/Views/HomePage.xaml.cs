using System;

using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _homeViewModel;
        static bool IsPortrait(Page p) { return p.Width < p.Height; }
        public HomePage()
        {
            InitializeComponent();
            
            _homeViewModel = Locator.Container.Resolve<HomeViewModel>();
            BindingContext = _homeViewModel;
            SizeChanged += HomePage_SizeChanged;
        }

        private void HomePage_SizeChanged(object sender, EventArgs e)
        {
            //if (IsPortrait(this))
            //{
            //    PagesPanel.ColumnCount = 2;
            //}
            //else
            //{
            //    PagesPanel.ColumnCount = 3;
            //}

        }

        private void PageItem_Tapped(object sender, EventArgs e)
        {
            var element = sender as View;
            if (element == null) return;
            var model = (element.BindingContext) as AdaaPageItem;
            _homeViewModel.PageClickedCommand.Execute(model);

        }


    }
}

