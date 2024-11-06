﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShoppingPlatform.WebApi.Filters
{
    // Custom action filter to control access based on the time of day
    public class TimeControlFilter : ActionFilterAttribute
    {
        // Properties to set the start and end times for allowed access
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        // Override method to execute logic before the action executes
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var now = DateTime.Now.TimeOfDay; // Get the current time of day

            StartTime = "00:00";
            EndTime = "04.59";

            if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime)) 
            {
                base.OnActionExecuting(context);
            }
            else
            {
                // If outside the allowed range, return a 403 Forbidden response
                context.Result = new ContentResult
                {
                    Content = "An end-point request cannot be made between these hours.",
                    StatusCode = 403
                };
            }

            
        }
    }
}