using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class HomePage : ContentPage
    {
    
        public HomePage()
        {
            InitializeComponent();
            BindingContext = Locator.Container.Resolve<HomeViewModel>();
        }
    }
}
