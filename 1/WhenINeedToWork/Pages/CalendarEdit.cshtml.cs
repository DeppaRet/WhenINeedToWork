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
        public int currYear = DateTime.Now.Year;
        public int work = 1, flex = 1;
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
            //events = new List<Event>();
            //events = _EventRepository.GetEvents(true).ToList();
            //foreach (Event ev in events)
            //{
            //    _EventRepository.Delete(ev.id);
            //}
            events = new List<Event>();
            events = _EventRepository.GetEvents(true).ToList();
            if (calendar_id != 0)
            {
                owner_calendar = _CalendarRepository.GetCalendarById(calendar_id);
                if (_EventRepository.GetEvents(calendar_id).ToList() != null) {
                    events.AddRange(_EventRepository.GetEvents(calendar_id).ToList());
                }
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
                if (wDays != null) {
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
            events = _EventRepository.GetEvents(true).ToList();
            if (calendar_id != 0)
            {
                owner_calendar = _CalendarRepository.GetCalendarById(calendar_id);
                events.AddRange( _EventRepository.GetEvents(calendar_id).ToList());
            }
            workingDays = new List<DateTime>();
            workingDays = CalculateWorkingPeriods(work_start_day, daysOfyear, events, working, flexing);
        }
        
        public class EventToDelete
        {
            public int id { get; set; }
            public string wStart { get; set; }
            public int u_id { get; set; }
            public int c_id { get; set; }
            public int w { get; set; }
            public int f { get; set; }
            public string name { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
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
                    tempEvent.IsTemp = true;
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
            events = _EventRepository.GetEvents(true).ToList();
            if (owner_calendar.User_ != null)
            {
                events.AddRange(_EventRepository.GetEvents(owner_calendar.id).ToList());
            }
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
            DateTime sDt = new DateTime();
            DateTime eDt = new DateTime();
            string name = "";
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
                            sDt = obj.startDate.AddDays(1);
                            eDt = obj.endDate.AddDays(1);
                            name = obj.name;
                            workstartday = Convert.ToDateTime(obj.wStart);
                            work = obj.w;
                            flex = obj.f;
                            owner = new User();
                            owner = _UserRepository.GetUserById(obj.u_id);
                            owner_calendar = new Calendar();
                            owner_calendar.id = 0;
                            if (obj.c_id >= 1) {
                                owner_calendar = _CalendarRepository.GetCalendarById(obj.c_id);
                            }

                        }
                    }
                }
            }
            _EventRepository.Delete(eventId);
            events = new List<Event>();
            events = _EventRepository.GetEvents(true).ToList();
            if (owner_calendar.User_ != null)
            {
                events.AddRange(_EventRepository.GetEvents(owner_calendar.id).ToList());
            }
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                daysOfyear.Add(DateTime.DaysInMonth(workstartday.Year, i + 1));
            }
            workingDays = new List<DateTime>();
            workingDays = CalculateWorkingPeriods(workstartday, daysOfyear, events, work, flex);
        }

        public void OnPostSaveNewCalendar(int user_id,int calendar_id,int currYear, string wsd, int working, int flexing,string calendarName)
        {
            workstartday = new DateTime();
            workstartday = Convert.ToDateTime(wsd);
            List<int> daysOfyear = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                daysOfyear.Add(DateTime.DaysInMonth(workstartday.Year, i + 1));
            }
            workingDays = new List<DateTime>();
            Calendar initCalendar = new Calendar();
            if (calendar_id == 0)
            {
                events = _EventRepository.GetEvents(true).ToList();
                workingDays = CalculateWorkingPeriods(workstartday, daysOfyear, events, working, flexing);
                initCalendar = new Calendar();
                initCalendar.creationTime = DateTime.Now;
                initCalendar.IsGeneralized = false;
                initCalendar.User_ = _UserRepository.GetUserById(user_id);
                initCalendar.Name = calendarName;
                initCalendar.financialForecast = false;
                initCalendar.HourlyRate = 0;
                initCalendar.Year = workstartday.Year;
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
                int id_new_calendar = _CalendarRepository.GetLastCalendarId();
                initCalendar = _CalendarRepository.GetCalendarById(id_new_calendar);
                foreach (Event eve in events) {
                    eve.Calendar_ = initCalendar;
                    eve.IsTemp = false;
                    _EventRepository.Update(eve);
                }

            }
            else {
                initCalendar = _CalendarRepository.GetCalendarById(calendar_id);
                events = _EventRepository.GetEvents(true).ToList();
                events.AddRange(_EventRepository.GetEvents(calendar_id).ToList());
                workingDays = CalculateWorkingPeriods(workstartday, daysOfyear, events, working, flexing);
                initCalendar.Name = calendarName;
                initCalendar.financialForecast = false;
                initCalendar.HourlyRate = 0;
                initCalendar.Year = workstartday.Year;
                initCalendar.firsWorkDay = workstartday;
                initCalendar.flexAmount = flexing;
                initCalendar.workAmount = working;
                string wDaysDescription = "";
                foreach (Event eve in events)
                {
                    eve.IsTemp = false;
                    _EventRepository.Update(eve);
                }
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
                foreach (Event ev in events)
                {
                    if (ev.Start_Date == ev.End_Date)
                    {
                        eventsDateTimes.Add(ev.Start_Date.Date);
                    }
                    else
                    {
                        DateTime date = new DateTime();
                        date = ev.Start_Date.Date;
                        while (date.CompareTo(ev.End_Date.Date) != 0)
                        {
                            eventsDateTimes.Add(date);
                            date = date.Date.AddDays(1);
                        }
                        eventsDateTimes.Add(date);
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
                                if (tempDate.Date.CompareTo(work_start_day.Date) != 0)
                                {
                                    if (tempDate.Date.CompareTo(dt.Date) == 0)
                                    {
                                        evin = true;
                                        break;
                                    }
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
            return workingDays;
        }
    }

}
