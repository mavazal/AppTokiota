﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AppTokiota.Users.Components.Activity
{
    public partial class AddActivityTimeDesviationPage : ContentPage
    {
        public AddActivityTimeDesviationPage()
        {
            try
            {

                NavigationPage.SetHasNavigationBar(this, false);
                InitializeComponent();

            } catch(Exception e)
            {

            }
        }

    }
}