using System;
using System.Collections.Generic;
using System.Text;

namespace WhenINeedToWork.Models
{
    public class Calendar
    {
        public int id { get; set; }
        public User User_ { get; set; }
        public DateTime creationTime { get; set; }
        public string Name { get; set; }
        public int workAmount { get; set; }
        public int flexAmount { get; set; }
        public DateTime firsWorkDay { get; set; }
        public bool financialForecast { get; set; }
        public int HourlyRate { get; set; }
        public int Year { get; set; }
        public bool IsGeneralized { get; set; }
        public string DescriptionPath { get; set; }
        public List<Event> Events { get; set; }
    }
}
