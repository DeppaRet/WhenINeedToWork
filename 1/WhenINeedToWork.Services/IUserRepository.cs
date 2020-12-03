using System;
using System.Collections.Generic;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public interface IUserRepository
    { 
        User GetUser(string email);
        User Add(User newUser);
        User GetUserById(int id);

    }
}
