using System;
using System.Collections.Generic;
using System.Text;

namespace WhenINeedToWork.Models
{
    public class Event
    {
        public int id { get; set; }
        public Calendar Calendar_ { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
       
    }
}
