using System;
using System.Collections.Generic;
using System.Text;

namespace WhenINeedToWork.Models
{
    public class Event
    {
        public int id { get; set; }
        public int Calendar_id { get; set; }
        public Calendar Calendar { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
       
    }
}
