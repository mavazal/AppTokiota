﻿using AppTokiota.Controls;
using AppTokiota.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTokiota.Services
{
    public interface ICalendarService
    {
        Task<IList<SpecialDate>> GetSpecialDatesBeetweenDatesAsync(Timesheet timesheet);
    }
}
