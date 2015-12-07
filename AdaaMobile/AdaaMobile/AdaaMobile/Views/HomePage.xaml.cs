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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            PagesPanel.BatchBegin();
            if (IsPortrait(this))
            {
                PagesPanel.ColumnCount = 3;
            }
            else
            {
                PagesPanel.ColumnCount = 4;
            }
            //double minSide = Math.Min(Width, Height);
            //double maxSide = Math.Min(Width, Height);
            double itemWidth = (Width - PagesPanel.Padding.Left - PagesPanel.Padding.Right - PagesPanel.ColumnSpacing * (PagesPanel.ColumnCount - 1))
                               / (PagesPanel.ColumnCount);
            PagesPanel.ColumnWidth = itemWidth;
            //PagesPanel.ColumnHeight = itemWidth;

            PagesPanel.BatchCommit();

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

		protected override void OnAppearing()
		{
			base.OnAppearing();

			{
				_homeViewModel.LoadCommand.Execute(null);
			}
		}
    }
}

