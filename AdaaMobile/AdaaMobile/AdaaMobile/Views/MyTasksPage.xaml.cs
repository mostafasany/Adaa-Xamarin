﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class MyTasksPage : ContentPage
    {
        public MyTasksPage()
        {
            InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
        }
    }
}
