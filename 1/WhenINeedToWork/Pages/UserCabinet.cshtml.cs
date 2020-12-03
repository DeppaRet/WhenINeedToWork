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

        public IUserRepository _UserRepository;

        public UserCabinetModel(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }
        public IActionResult OnGet(int id)
        {
            IUser = _UserRepository.GetUserById(id);
            return Page();
        }
    }
}
