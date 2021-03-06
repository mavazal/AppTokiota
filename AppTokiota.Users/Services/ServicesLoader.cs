﻿using System;
using Prism.Ioc;
using AppTokiota.Users.Services;

namespace AppTokiota.Users.Services
{
    public static class ServicesLoader
    {
        public static void Load(IContainerRegistry containerRegistry)
        {
         
            if (AppSettings.UseFakeServices)
            {
                containerRegistry.RegisterSingleton<INetworkConnectionService, FakeNetworkConnectionService>();
                containerRegistry.RegisterSingleton<IAuthenticationService, FakeAuthenticationService>();
                containerRegistry.RegisterSingleton<ITimesheetService, FakeTimesheetService>();
                containerRegistry.RegisterSingleton<IReviewService, FakeReviewService>();
            } else
            {
                containerRegistry.RegisterSingleton<IAuthenticationService, AuthenticationService>();
                containerRegistry.RegisterSingleton<ITimesheetService, TimesheetService>();
                containerRegistry.RegisterSingleton<INetworkConnectionService, NetworkConnectionService>();
                containerRegistry.RegisterSingleton<IReviewService, ReviewService>();
            }

            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
			containerRegistry.RegisterSingleton<IDialogErrorCustomService, DialogErrorCustomService>();
            containerRegistry.RegisterSingleton<ICacheEntity, AkavacheEntity>();
            containerRegistry.RegisterSingleton<ICalendarService, CalendarService>();
			containerRegistry.RegisterSingleton<IChartService, ChartService>();
            containerRegistry.RegisterSingleton<ITimeLineService, TimeLineService>();
            containerRegistry.RegisterSingleton<IAnalyticsService, AnalyticsService>();

            containerRegistry.Register<IRequestService, RequestService>();

        }
    }
}
