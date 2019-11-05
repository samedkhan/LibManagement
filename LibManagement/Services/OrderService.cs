using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibManagement.DAL;
using LibManagement.Models;
using LibManagement.Helpers;


namespace LibManagement.Services
{
    public class OrderService
    {
        private readonly LibManagementContext _context;

        public OrderService()
        {
            _context = new LibManagementContext();
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Include("user").Include("customer").Include("book").ToList();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public Order Find(int id)
        {
            return _context.Orders.Find(id);
        }

        public decimal CalculateTotalPay()
        {
            return _context.Orders.Sum(o => o.TotalPrice);
        }

        public decimal CalculateCashPayment()
        {
            return _context.Orders.Where(o => o.Status != true).Sum(o => o.TotalPrice);
        }

        public bool CheckOrderStatus()
        {
            return _context.Orders.Any(o => o.Status != true);
        }
    }
}
