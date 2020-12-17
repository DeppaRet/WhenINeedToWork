using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhenINeedToWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhenINeedToWork.Services;

namespace WhenINeedToWork.Pages
{
    public class LoginModel : PageModel
    {
        public string Message { get; set; }
        public int tempCaledarId { get;set; }
        public User IUser{ get; private set; }
        public IUserRepository _UserRepository;
        public ICalendarRepository _CalendarRepository;

        public LoginModel(IUserRepository userRepository,ICalendarRepository calendarRepository)
        {
            _UserRepository = userRepository;
            _CalendarRepository = calendarRepository;
        }
        public void OnGet(int new_calendar_id) {
            Message = "";
            if (new_calendar_id != 0) {
                tempCaledarId = new_calendar_id;
            }
        }

        public IActionResult OnPost(string login,string password,int new_calendar_id)
        {
            IUser = _UserRepository.GetUser(login);
            if (IUser != null)
            {
                if (IUser.password == password)
                {
                    if (new_calendar_id != 0)
                    {
                        Calendar calendar = new Calendar();
                        calendar = _CalendarRepository.GetCalendarById(new_calendar_id);
                        calendar.User_ = IUser;
                        _CalendarRepository.Update(calendar);
                        calendar = _CalendarRepository.GetCalendarById(new_calendar_id);
                        string url = Url.Page("CalendarEdit",new { id = IUser.id, calendar_id = calendar.id });
                        return RedirectPermanent(url);
                    }
                    else {
                        string url = Url.Page("UserCabinet", new { id = IUser.id });
                        return RedirectPermanent(url);
                    }
                }
                else
                {
                    Message = "Неправильный пароль. Повторите ввод.";
                }
            }
            else
            {
                User NewUser = new User();
                NewUser.password = password;
                NewUser.email = login;
                IUser = _UserRepository.Add(NewUser);
                IUser = _UserRepository.GetUser(login);
                if (new_calendar_id != 0)
                {
                    Calendar calendar = new Calendar();
                    calendar = _CalendarRepository.GetCalendarById(new_calendar_id);
                    calendar.User_ = IUser;
                    _CalendarRepository.Update(calendar);
                    calendar = _CalendarRepository.GetCalendarById(new_calendar_id);
                    string url = Url.Page("CalendarEdit", new { id = IUser.id, calendar_id = calendar.id });
                    return RedirectPermanent(url);
                }
                else
                {
                    string url = Url.Page("UserCabinet", new { id = IUser.id });
                    return RedirectPermanent(url);
                }
            }
            tempCaledarId = new_calendar_id;
            return Page();
        }
    }
}
