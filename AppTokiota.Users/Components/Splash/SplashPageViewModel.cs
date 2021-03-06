﻿using AppTokiota.Users.Components.BaseNavigation;
using AppTokiota.Users.Components.Core;
using AppTokiota.Users.Components.Core.Module;
using AppTokiota.Users.Components;
using AppTokiota.Users.Components.Login;
using AppTokiota.Users.Components.Master;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using AppTokiota.Users.Components.DashBoard;
using AppTokiota.Users.Components.Connection;
using System.Threading.Tasks;

namespace AppTokiota.Users.Components.Splash
{
    public class SplashPageViewModel : ViewModelBase
    {
        private ISplashModule _splashModule;

        public SplashPageViewModel(IViewModelBaseModule baseModule, ISplashModule splashModule) : base(baseModule)
        {
            Title = "Splash";
            _splashModule = splashModule;

            ModeLoadingPopUp = false;

            IsBusy = true;
                     
            Device.StartTimer(new TimeSpan(0, 0, 0,1,800), () =>
            {
				IsBusy = false;
                AuthenticationRun();
                return false;
            });
        }
        

        private void AuthenticationRun()
        {
			if (AppSettings.AuthenticatedUserResponse != null)
            {
                BaseModule.NavigationService.NavigateAsync(MasterModule.GetMasterNavigationPage(AppSettings.StartupView));
            } else {
			    NavigateCommand.Execute(PageRoutes.GetKey<LoginPage>());
            }     
        }
    }
}
