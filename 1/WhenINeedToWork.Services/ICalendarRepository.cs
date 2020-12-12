using System;
using System.Collections.Generic;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public interface ICalendarRepository
    {
        Calendar Add(Calendar newCalendar);
        Calendar Delete(int id);
        List<Calendar> GetCalendars(User user);
        Calendar GetCalendarById(int id);
        Calendar Update(Calendar upEvent);
        int GetLastCalendarId();
    }
}
