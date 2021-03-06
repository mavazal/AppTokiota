﻿using System;
using System.Collections.Generic;

using AppTokiota.Users.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AppTokiota.Users.Services
{
    public class TimeLineService : ITimeLineService
    {
        public async Task<IList<ItemTimeLine>> GetListTimesheetForDay(Review review)
        {
            var listTimesheetForDay = new List<ItemTimeLine>();

            foreach (var TsDay in review.Days)
            {
                var timesheetForDay = new ItemTimeLine();
                timesheetForDay.Day = new Day {
                    Date = TsDay.Date, 
                    IsClosed = TsDay.IsClosed,
                    Holiday = new Holiday(),
                    name = TsDay.name,
                    IsWeekend = TsDay.IsWeekend
                };
                if (TsDay.Holiday != null)
                {
                    timesheetForDay.Day.Holiday.IsHolyday = TsDay.Holiday.IsHolyday;
                    timesheetForDay.Day.Holiday.Name = TsDay.Holiday.Name;
                } 
                listTimesheetForDay.Add(timesheetForDay);
            }

            foreach (var ts in listTimesheetForDay) {
                ts.Activities = MapActivities(ts.Day.Date, review);
            }

            return await Task.FromResult(listTimesheetForDay);

        }

        private List<ActivityDay> MapActivities(DateTime dateTime, Review review)
        {
            var activities = new List<ActivityDay>();
            try
            {
                foreach (var item in review.Activities.Where(x => x.Value.Date.Equals(dateTime)))
                {
                    var activity = item.Value;
                    var project = review.Projects.FirstOrDefault(y => y.Key.Equals(activity.ProjectId));

                    activities.Add(new ActivityDay()
                    {
                        AssignementId = activity.AssignementId,
                        Project = TimesheetImutationBase.Map(project.Value),
                        Date = activity.Date,
                        Id = activity.Id,
                        Description = activity.Description,
                        Deviation = activity.Deviation,
                        UserId = activity.UserId,
                        Imputed = activity.Imputed,
                        Task = TimesheetImutationBase.Map(project.Value, activity.TaskId)
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return activities;
        }
      
    }
}
