using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WhenINeedToWork.Models;
using WhenINeedToWork.Services;

namespace WhenINeedToWork
{
    public class UniteModel : PageModel
    {
        public string[] colors = { "#DC143C", "#FFA500", "#F0E68C", "#6495ED", "#00FF00" };
        public IUserRepository _UserRepository;
        public ICalendarRepository _CalendarRepository;
        public List<CalendarToUnite> Calendars;
        public User owner { get; private set; }
        public List<DateTime> workingDays { get; private set; }
        public UniteModel(IUserRepository userRepository, ICalendarRepository calendarRepository)
        {
            _UserRepository = userRepository;
            _CalendarRepository = calendarRepository;
        }
        public void OnGet(List<int> owner_calendars,int id)
        {
            Calendars = new List<CalendarToUnite>();
            int a = 0;
            foreach (int i in owner_calendars) {
                Calendar t_calen = new Calendar();
                t_calen = _CalendarRepository.GetCalendarById(i);
                CalendarToUnite toUnite = new CalendarToUnite();
                string textFromFile;
                using (StreamReader sr = new StreamReader(t_calen.DescriptionPath))
                {
                    textFromFile = sr.ReadToEnd();
                }
                string[] wDays = textFromFile.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                List<DateTime>  wd = new List<DateTime>();
                if (wDays != null)
                {
                    foreach (string s in wDays)
                    {
                        wd.Add(Convert.ToDateTime(s));
                    }
                }
                toUnite.name = t_calen.Name;
                toUnite.WD = wd;
                toUnite.color = colors[a];
                if (a + 1 == colors.ToList().Count) {
                    a = 0;
                }
                Calendars.Add(toUnite);
            }
            int r = 032422;
        }
        public class CalendarToUnite {
            public List<DateTime> WD { get; set; }
            public string color{ get; set; }
            public string name { get; set; }
        }

        
    }
}
