using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhenINeedToWork.Models;
using WhenINeedToWork.Services;

namespace WhenINeedToWork.Pages
{
    public class CalendarEditModel : PageModel
    {
        public ICalendarRepository calendarRepository;
        public string Today = DateTime.Now.ToString("yyyy-MM-dd");
        public List<DateTime> workingDays;
        public List<DateTime> events;
        public IActionResult OnGet(User calendar_owner) {
            workingDays = new List<DateTime>();
            events = new List<DateTime>();
            return Page();
        }
        public void OnPost(DateTime work_start_day,int working, int flexing) {
            int a = working + flexing;
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++) {
                daysOfyear.Add(DateTime.DaysInMonth(work_start_day.Year,i+1));
            }
            CalculateWorkingPeriods(work_start_day,daysOfyear, events,working,flexing);
            int e = 0;
        }
        public void CalculateWorkingPeriods(DateTime work_start_day,List<int> daysOfyear,List<DateTime> events, int working, int flexing) {
            workingDays = new List<DateTime>();
            events = new List<DateTime>();
            if (events.Count == 0) {
                int tempDay = work_start_day.Day, tempWorkDays = working, tempFlexDays = 0;
                for(int i = work_start_day.Month;i <= daysOfyear.Count;i++) {
                    for (int j = tempDay; j <= daysOfyear[i-1]; j++) {
                        if (tempWorkDays != 0) {
                            workingDays.Add(new DateTime(work_start_day.Year,i,j));
                            tempWorkDays -= 1;
                            if (tempWorkDays == 0) {
                                tempFlexDays = flexing;
                            }
                        }
                        else
                        {
                            tempFlexDays -= 1;
                            if (tempFlexDays == 0) {
                                tempWorkDays = working;
                            }
                        } 
                    }
                    tempDay = 1;
                }
                tempDay = work_start_day.Day-1;
                tempWorkDays = 0;
                tempFlexDays = flexing;
                for (int i = work_start_day.Month; i >= 1; i--)
                {
                    for (int j = tempDay; j >= 1; j--)
                    {
                        if (tempWorkDays != 0)
                        {
                            workingDays.Add(new DateTime(work_start_day.Year, i, j));
                            tempWorkDays -= 1;
                            if (tempWorkDays == 0)
                            {
                                tempFlexDays = flexing;
                            }
                        }
                        else
                        {
                            tempFlexDays -= 1;
                            if (tempFlexDays == 0)
                            {
                                tempWorkDays = working;
                            }
                        }
                    }
                    if(i!=1)tempDay = daysOfyear[i-2];
                }
            }
            else {
                int tempDay = work_start_day.Day, tempWorkDays = working, tempFlexDays = 0;
                for (int i = work_start_day.Month; i <= daysOfyear.Count; i++)
                {
                    for (int j = tempDay; j <= daysOfyear[i - 1]; j++)
                    {
                        if (tempWorkDays != 0)
                        {
                            DateTime tempDate = new DateTime(work_start_day.Year, i, j);
                            if (!events.Contains(tempDate))
                            {
                                workingDays.Add(new DateTime(work_start_day.Year, i, j));
                                tempWorkDays -= 1;
                                if (tempWorkDays == 0)
                                {
                                    tempFlexDays = flexing;
                                }
                            }
                        }
                        else
                        {
                            tempFlexDays -= 1;
                            if (tempFlexDays == 0)
                            {
                                tempWorkDays = working;
                            }
                        }
                    }
                    tempDay = 1;
                }
                tempDay = work_start_day.Day - 1;
                tempWorkDays = 0;
                tempFlexDays = flexing;
                for (int i = work_start_day.Month; i >= 1; i--)
                {
                    for (int j = tempDay; j >= 1; j--)
                    {
                        if (tempWorkDays != 0)
                        {
                            workingDays.Add(new DateTime(work_start_day.Year, i, j));
                            tempWorkDays -= 1;
                            if (tempWorkDays == 0)
                            {
                                tempFlexDays = flexing;
                            }
                        }
                        else
                        {
                            tempFlexDays -= 1;
                            if (tempFlexDays == 0)
                            {
                                tempWorkDays = working;
                            }
                        }
                    }
                    if (i != 1) tempDay = daysOfyear[i - 2];
                }
            }
        }
    }
}
