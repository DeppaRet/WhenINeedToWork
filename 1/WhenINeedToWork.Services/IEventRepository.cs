using System;
using System.Collections.Generic;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public interface IEventRepository
    {
        Event Add(Event newEvent);
        Event GetEvent(int event_id);
        Event GetEvent(DateTime sdt,DateTime edt,string name,Calendar clr);
        Event Delete(DateTime sdt, DateTime edt, string name, Calendar clr);
        Event Delete(int event_id);
        IEnumerable<Event> GetEvents(int calendar_id);
        IEnumerable<Event> GetEvents(bool type);
        Event Update(Event upEvent);
    }
}
