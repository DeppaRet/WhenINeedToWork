using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WhenINeedToWork.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost(string login,string password)
        {
            string s = login + password;
            return Page();
        }
    }
}
