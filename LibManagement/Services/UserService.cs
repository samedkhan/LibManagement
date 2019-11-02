using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibManagement.DAL;
using LibManagement.Models;

namespace LibManagement.Services
{

    class UserService
    {
        private readonly LibManagementContext _context;

        public UserService()
        {
            _context = new LibManagementContext();
        }

        public void Add(User user)
        {

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(User user)
        {

            _context.Users.Remove(user);
            _context.SaveChanges();
        }


        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User Find(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public User Contain(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            //    return _context.Users.Any(u => u.Username.Contains(username) && u.Password.Contains(password));
        }

        public User FindUser(string username, string password)
        {
            return _context.Users.Where(u => u.Username.Contains(username) && u.Password.Contains(password)).FirstOrDefault();
        }

    }
}
