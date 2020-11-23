using System;
using System.Collections.Generic;
using System.Text;

namespace WhenINeedToWork.Models
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public List<Calendar> Calendars { get; set; }
    }
}
