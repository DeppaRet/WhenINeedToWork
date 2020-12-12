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
    public class UserCabinetModel : PageModel
    {
        public User IUser { get; private set; }
        public List<Calendar> AllUserCalendars;

        public IUserRepository _UserRepository;
        

        public ICalendarRepository _CalendarRepository;

        public UserCabinetModel(IUserRepository userRepository,ICalendarRepository calendarRepository)
        {
            _UserRepository = userRepository;
            _CalendarRepository = calendarRepository;
        }
        public IActionResult OnGet(int id)
        {
            IUser = _UserRepository.GetUserById(id);
            AllUserCalendars = _CalendarRepository.GetCalendars(IUser);
            return Page();
        }
    }
}
