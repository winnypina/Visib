﻿using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace Visib.Mobile.Views
{
    [MvxModalPresentation]
    public partial class VideoDisplayView 
    {
        public VideoDisplayView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
