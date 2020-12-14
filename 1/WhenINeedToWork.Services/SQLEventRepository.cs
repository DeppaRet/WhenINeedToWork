using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public class SQLEventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public SQLEventRepository(AppDbContext context)
        {
            _context = context;
        }
        public Event Add(Event newEvent)
        {
            _context.Event.Add(newEvent);
            _context.SaveChanges();
            return newEvent;
        }

        public Event Delete(int id)
        {
            var eventToDelete = _context.Event.Find(id);
            if (eventToDelete != null)
            {
                _context.Event.Remove(eventToDelete);
                _context.SaveChanges();
            }
            return eventToDelete;
        }

        public Event GetEvent(int event_id)
        {
            return _context.Event.FirstOrDefault(x => x.id == event_id);
        }

        public IEnumerable<Event> GetEvents(Calendar calendar)
        {
            IEnumerable<Event> query = _context.Event;
            query = query.Where(x => x.Calendar_ == calendar);
            return query.ToList();
        }

        public Event Update(Event upEvent)
        {
            var Uevent = _context.Event.Attach(upEvent);
            Uevent.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return upEvent;
        }
    }
}
