using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibManagement.DAL;
using LibManagement.Models;

namespace LibManagement.Services
{
    class CustomerService
    {
        private readonly LibManagementContext _context;

        public CustomerService()
        {
            _context = new LibManagementContext();
        }
        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        public void Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
        public void Update(Customer customer)
        {
            _context.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public Customer FindById(int id)
        {
            return _context.Customers.Find(id);
        }
        public bool Contain(string name, string phone)
        {
            return _context.Customers.Any(c => c.FullName.Contains(name) || c.PhoneNumber.Contains(phone));
        }

        

        public List<Customer> AllCustomers()
        {
            return _context.Customers.ToList();
        }
    }
}
