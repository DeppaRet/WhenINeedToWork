using System;
using System.Collections.Generic;
using System.Text;

namespace WhenINeedToWork.Models
{
    public class Calendar
    {
        public int id { get; set; }
        public int User_id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public bool IsGeneralized { get; set; }
        public string DescriptionPath { get; set; }
        public List<Event> Events { get; set; }
    }
}
