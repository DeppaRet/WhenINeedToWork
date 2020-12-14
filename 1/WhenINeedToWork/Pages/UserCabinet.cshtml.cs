using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhenINeedToWork.Models;
using WhenINeedToWork.Services;


namespace WhenINeedToWork.Pages
{
    public class UserCabinetModel : PageModel
    {
        public User IUser { get; private set; }
        public List<Calendar> AllUserCalendars;

        public IUserRepository _UserRepository;
        public string email;

        public ICalendarRepository _CalendarRepository;
        public IEventRepository _EventRepository;

        public UserCabinetModel(IUserRepository userRepository,ICalendarRepository calendarRepository,IEventRepository eventRepository)
        {
            _UserRepository = userRepository;
            _CalendarRepository = calendarRepository;
            _EventRepository = eventRepository;
        }
        public IActionResult OnGet(int id)
        {
            IUser = _UserRepository.GetUserById(id);
            email = IUser.email;
            AllUserCalendars = _CalendarRepository.GetCalendars(IUser);
            return Page();
        }
        public void OnPost(int id,int calendar_id) {
            System.IO.File.Delete(_CalendarRepository.GetCalendarById(calendar_id).DescriptionPath);
            Calendar calendar = new Calendar();
            calendar = _CalendarRepository.GetCalendarById(calendar_id);
            List<Event> events = new List<Event>();
            events = _EventRepository.GetEvents(calendar).ToList();
            foreach (Event ev in events) {
                _EventRepository.Delete(ev.id);
            }
            _CalendarRepository.Delete(calendar_id);
            IUser = _UserRepository.GetUserById(id);
            email = IUser.email;
            AllUserCalendars = _CalendarRepository.GetCalendars(IUser);
        }
    }
}
