﻿using AppTokiota.Components.Core;
using AppTokiota.Components.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;

namespace AppTokiota.Components.Dashboard
{
    public class DashBoardPageViewModel : ViewModelBase
    {
        private readonly IDashBoardModule _dashBoardModule;

        public DashBoardPageViewModel(INavigationService navigationService, IDashBoardModule dashBoardModule) : base(navigationService)
        {
            Title = "Dashboard";
            _dashBoardModule = dashBoardModule;
            //_dashBoardModule.AuthenticationService.Logout();
        }
    }
}
