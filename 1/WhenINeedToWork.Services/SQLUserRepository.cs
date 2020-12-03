 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public SQLUserRepository(AppDbContext context) {
            _context = context;
        }

        public User Add(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.id == id);
        }
        public User GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) {
                return null;
            }
            return _context.Users.FirstOrDefault(x => x.email == email);
        }
    }
}
