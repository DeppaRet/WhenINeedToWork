using System;
using System.Collections.Generic;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public interface IEventRepository
    {
        Event Add(Event newEvent);
        Event Delete(int id);
        IEnumerable<Event> GetEvents(Calendar calendar);
        Event Update(Event upEvent);
    }
}
