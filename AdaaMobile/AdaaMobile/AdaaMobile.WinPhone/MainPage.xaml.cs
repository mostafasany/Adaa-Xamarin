﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AdaaMobile.ViewModels;
using AdaaMobile.WinPhone.CustomRenderers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AdaaMobile.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();

            LoadApplication(new AdaaMobile.App(new Locator()));
        }
    }
}
