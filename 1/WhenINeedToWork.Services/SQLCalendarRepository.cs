using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public class SQLCalendarRepository : ICalendarRepository
    {
        private readonly AppDbContext _context;

        public SQLCalendarRepository(AppDbContext context)
        {
            _context = context;
        }
        public Calendar Add(Calendar newCalendar)
        {
            _context.Calendars.Add(newCalendar);
            _context.SaveChanges();
            return newCalendar;
        }

        public Calendar Delete(int id)
        {
            var calendarToDelete = _context.Calendars.Find(id);
            if (calendarToDelete != null) {
                _context.Calendars.Remove(calendarToDelete);
                _context.SaveChanges();
            }
            return calendarToDelete;
        }

        public Calendar GetCalendarById(int id)
        {
            return _context.Calendars.FirstOrDefault(x => x.id == id);
        }

        public int GetLastCalendarId() {
            return _context.Calendars.Max(p => p.id);
        }
        public List<Calendar> GetCalendars(User user)
        {
            IEnumerable<Calendar> query = _context.Calendars;
            query = query.Where(x => x.User_ == user);
            return query.ToList();
        }

        public Calendar Update(Calendar upCalendar)
        {
            var calendar = _context.Calendars.Attach(upCalendar);
            calendar.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return upCalendar;
        }
    }
}
