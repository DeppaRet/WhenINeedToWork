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
        public User IUser { get; private set; }

        public IUserRepository _UserRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }
        public void OnGet(int new_calendar_id) { 
            
        }
        public IActionResult OnPost(string login,string password)
        {
            IUser = _UserRepository.GetUser(login);
            if (IUser != null)
            {
                if (IUser.password == password)
                {
                    string url = Url.Page("UserCabinet", new { id = IUser.id});
                    return RedirectPermanent(url);
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
                string url = Url.Page("UserCabinet", new { id = IUser.id });
                return RedirectPermanent(url);
            }
            return Page();
        }
    }
}
