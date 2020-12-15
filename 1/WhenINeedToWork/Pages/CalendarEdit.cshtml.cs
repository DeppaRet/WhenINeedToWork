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

namespace WhenINeedToWork.Pages
{
    public class CalendarEditModel : PageModel
    {
        public ICalendarRepository _calendarRepository;
        public string Today = DateTime.Now.ToString("yyyy-MM-dd");
        public List<DateTime> workingDays { get; private set; }
        public List<Event> events;
        public int currYear = 2020;
        public int work, flex;
        public Event tempEvent;
        public DateTime workstartday { get; set; }
        public User owner { get; private set; }
        public Calendar owner_calendar { get; private set; }
        public IUserRepository _UserRepository;
        public ICalendarRepository _CalendarRepository;
        public IWebHostEnvironment _WebHostEnviroment;
        public IEventRepository _EventRepository;
        public CalendarEditModel(IUserRepository userRepository, ICalendarRepository calendarRepository, IWebHostEnvironment webHostEnviroment, IEventRepository eventRepository)
        {
            _UserRepository = userRepository;
            _CalendarRepository = calendarRepository;
            _WebHostEnviroment = webHostEnviroment;
            _EventRepository = eventRepository;
        }
        public void OnGet(int id, int calendar_id, List<DateTime> wD) {
            owner = new User();
            owner = _UserRepository.GetUserById(id);
            owner_calendar = new Calendar();
            events = new List<Event>();
            events = _EventRepository.GetEvents(owner_calendar).ToList();
            if (calendar_id != 0)
            {
                owner_calendar = _CalendarRepository.GetCalendarById(calendar_id);
                events = _EventRepository.GetEvents(owner_calendar).ToList();
                work = owner_calendar.workAmount;
                flex = owner_calendar.flexAmount;
                currYear = owner_calendar.Year;
                workstartday = owner_calendar.firsWorkDay;
                string textFromFile;
                using (StreamReader sr = new StreamReader(owner_calendar.DescriptionPath))
                {
                    textFromFile = sr.ReadToEnd();
                }
                string[] wDays = textFromFile.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                workingDays = new List<DateTime>();
                if (wDays[0] != "") {
                    foreach (string s in wDays)
                    {
                        workingDays.Add(Convert.ToDateTime(s));
                    }
                }

            }
            else if (wD.Count == 0)
            {
                workingDays = new List<DateTime>();
                owner_calendar = new Calendar();
            }
            else {
                workingDays = new List<DateTime>();
                workingDays = wD;
            }
        }
        public void OnPost(DateTime work_start_day, int working, int flexing, int id, int calendar_id) {
            work = working;
            flex = flexing;
            workstartday = work_start_day;
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++) {
                daysOfyear.Add(DateTime.DaysInMonth(work_start_day.Year, i + 1));
            }
            owner = new User();
            owner = _UserRepository.GetUserById(id);
            owner_calendar = new Calendar();
            events = new List<Event>();
            if (calendar_id != 0) {
                owner_calendar = _CalendarRepository.GetCalendarById(calendar_id);
                events = _EventRepository.GetEvents(owner_calendar).ToList();
            }
            workingDays = new List<DateTime>();
            workingDays = CalculateWorkingPeriods(work_start_day, daysOfyear, events, working, flexing);
            Page();
        }
        
        public class EventToDelete
        {
            public int id { get; set; }
            public string wStart { get; set; }
            public int u_id { get; set; }
            public int c_id { get; set; }
            public int w { get; set; }
            public int f { get; set; }
        }
        public class EventToAddOrUpdate
        {
            public int id { get; set; }
            public string wStart { get; set; }
            public int u_id { get; set; }
            public int c_id { get; set; }
            public int w { get; set; }
            public int f { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public DateTime startDate{ get; set; }
            public DateTime endDate { get; set; }

        }
        public void OnPostAddOrUpdate(){
            int eventId = 0;
            DateTime sDt = new DateTime();
            DateTime eDt = new DateTime();
            string name ="";
            string location ="";
            {
                MemoryStream stream = new MemoryStream();
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string requestBody = reader.ReadToEnd();
                    if (requestBody.Length > 0)
                    {
                        var obj = JsonConvert.DeserializeObject<EventToAddOrUpdate>(requestBody);
                        if (obj != null)
                        {
                            sDt = obj.startDate.AddDays(1);
                            eDt = obj.endDate.AddDays(1);
                            location = obj.location;
                            name = obj.name;
                            eventId = obj.id;
                            workstartday = Convert.ToDateTime(obj.wStart);
                            owner = new User();
                            work = obj.w;
                            flex = obj.f;
                            owner = _UserRepository.GetUserById(obj.u_id);
                            owner_calendar = new Calendar();
                            if (obj.c_id >= 1)
                            {
                                owner_calendar = _CalendarRepository.GetCalendarById(obj.c_id);
                            }
                        }
                    }
                }
                
                tempEvent = new Event();
                tempEvent = _EventRepository.GetEvent(eventId);
                if (tempEvent == null)
                {
                    tempEvent = new Event();
                    tempEvent.Start_Date = sDt;
                    tempEvent.End_Date = eDt;
                    tempEvent.Content = name;
                    tempEvent.Location = location;
                    tempEvent.Calendar_ = owner_calendar;
                    _EventRepository.Add(tempEvent);
                }
                else {
                    tempEvent.Start_Date = sDt;
                    tempEvent.End_Date = eDt;
                    tempEvent.Content = name;
                    tempEvent.Location = location;
                    tempEvent.Calendar_ = owner_calendar;
                    _EventRepository.Update(tempEvent);
                }
            }
            events = new List<Event>();
            events = _EventRepository.GetEvents(owner_calendar).ToList();
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                daysOfyear.Add(DateTime.DaysInMonth(workstartday.Year, i + 1));
            }
            workingDays = new List<DateTime>();
            workingDays = CalculateWorkingPeriods(workstartday, daysOfyear, events, work, flex);
            
        }
        public void OnPostDeleteEvent() {

            int eventId = 0;
            
            {
                MemoryStream stream = new MemoryStream();
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string requestBody = reader.ReadToEnd();
                    if (requestBody.Length > 0)
                    {
                        var obj = JsonConvert.DeserializeObject<EventToDelete>(requestBody);
                        if (obj != null)
                        {
                            eventId = obj.id;
                           workstartday = Convert.ToDateTime(obj.wStart);
                            work = obj.w;
                            flex = obj.f;
                            owner = new User();
                            owner = _UserRepository.GetUserById(obj.u_id);
                            owner_calendar = new Calendar();
                            if (obj.c_id >= 1) {
                                owner_calendar = _CalendarRepository.GetCalendarById(obj.c_id);
                            }

                        }
                    }
                }
            }
            _EventRepository.Delete(eventId);
            events = new List<Event>();
            events = _EventRepository.GetEvents(owner_calendar).ToList();
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                daysOfyear.Add(DateTime.DaysInMonth(workstartday.Year, i + 1));
            }
            workingDays = new List<DateTime>();
            workingDays = CalculateWorkingPeriods(workstartday, daysOfyear, events, work, flex);
            

        }
        public void OnPostSave(int user_id,int calendar_id,int currYear, string wsd, int working, int flexing)
        {
            workstartday = new DateTime();
            workstartday = Convert.ToDateTime(wsd);
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                daysOfyear.Add(DateTime.DaysInMonth(workstartday.Year, i + 1));
            }
            workingDays = new List<DateTime>();
            workingDays = CalculateWorkingPeriods(workstartday, daysOfyear, events, working, flexing);
            Calendar initCalendar = new Calendar();
            if (calendar_id == 0)
            {
                initCalendar.creationTime = DateTime.Now;
                initCalendar.IsGeneralized = false;
                initCalendar.User_ = _UserRepository.GetUserById(user_id);
                initCalendar.Name = "test" + initCalendar.creationTime.Millisecond;
                initCalendar.financialForecast = false;
                initCalendar.HourlyRate = 0;
                initCalendar.Year = currYear;
                initCalendar.firsWorkDay = workstartday;
                initCalendar.flexAmount = flexing;
                initCalendar.workAmount = working;
                string wDaysDescription = "";
                foreach (DateTime wTime in workingDays)
                {
                    wDaysDescription += wTime.ToString() + "\n";
                }
                string uploadFolder = Path.Combine(_WebHostEnviroment.WebRootPath, "calendarsDescriptions");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + initCalendar.Name;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                initCalendar.DescriptionPath = filePath;
                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(wDaysDescription);
                }
                _CalendarRepository.Add(initCalendar);

            }
            else {
                initCalendar = _CalendarRepository.GetCalendarById(calendar_id);
                initCalendar.Name = "test";
                initCalendar.financialForecast = false;
                initCalendar.HourlyRate = 0;
                initCalendar.Year = currYear;
                initCalendar.firsWorkDay = workstartday;
                initCalendar.flexAmount = flexing;
                initCalendar.workAmount = working;
                string wDaysDescription = "";
                foreach (DateTime wTime in workingDays)
                {
                    wDaysDescription += wTime.ToString() + "\n";
                }
                using (StreamWriter sw = new StreamWriter(initCalendar.DescriptionPath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(wDaysDescription);
                }
                _CalendarRepository.Update(initCalendar);
            }
            OnGet(user_id, initCalendar.id, workingDays);
        }
        public IActionResult OnPostRedir(int user_id) {
            owner = _UserRepository.GetUserById(user_id);
            string url = Url.Page("UserCabinet", new { id = owner.id });
            return RedirectPermanent(url);
        }
        public List<DateTime> CalculateWorkingPeriods(DateTime work_start_day,List<int> daysOfyear,List<Event> events, int working, int flexing) {
            workingDays = new List<DateTime>();
            if (events == null) {
                events = new List<Event>();
            }
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
                List<DateTime> eventsDateTimes = new List<DateTime>();
                foreach (Event ev in events) {
                    if (ev.Start_Date == ev.End_Date)
                    {
                        eventsDateTimes.Add(ev.Start_Date.Date);
                    }
                    else {
                        while (ev.Start_Date != ev.End_Date) {
                            ev.Start_Date.AddDays(1);
                            eventsDateTimes.Add(ev.Start_Date.Date);
                        }
                    }
                }
                for (int i = work_start_day.Month; i <= daysOfyear.Count; i++)
                {
                    for (int j = tempDay; j <= daysOfyear[i - 1]; j++)
                    {
                        if (tempWorkDays != 0)
                        {
                            DateTime tempDate = new DateTime(work_start_day.Year, i, j);
                            bool evin = false;
                            foreach (DateTime dt in eventsDateTimes)
                            {
                                if (tempDate.Date == dt)
                                {
                                    evin = true;
                                    break;
                                }
                            }
                            if (!evin)
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
            return workingDays;
        }
    }

}
