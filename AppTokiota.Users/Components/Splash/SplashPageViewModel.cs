﻿using AppTokiota.Users.Components.BaseNavigation;
using AppTokiota.Users.Components.Core;
using AppTokiota.Users.Components.Core.Module;
using AppTokiota.Users.Components.Dashboard;
using AppTokiota.Users.Components.Login;
using AppTokiota.Users.Components.Master;
using System;
using Xamarin.Forms;

namespace AppTokiota.Users.Components.Splash
{
    public class SplashPageViewModel : ViewModelBase
    {
        private ISplashModule _splashModule;

        public SplashPageViewModel(IViewModelBaseModule baseModule, ISplashModule splashModule) : base(baseModule)
        {
            Title = "Splash";
            _splashModule = splashModule;
            
            Device.StartTimer(new TimeSpan(0, 0, 3), () =>
            {
                BaseModule.AuthenticationService.InitializeAsync();
                AuthenticationRun();
                return false;
            });
        }


        private void AuthenticationRun()
        {
            if (BaseModule.AuthenticationService.IsAuthenticated)
            {
                NavigateCommand.Execute(MasterModule.Tag + BaseNavigationModule.Tag + DashBoardModule.Tag);
            }
            else
            {
                NavigateCommand.Execute(LoginModule.Tag);
            }           
        }
    }
}