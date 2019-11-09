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

        public int Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.BookId;
        }

        public int Delete(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return order.BookId;
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

        public decimal CalculateTotalPayByDate(DateTime date1, DateTime date2)
        {
            return _context.Orders.Where(o=>o.CreatedAt >= date1 && o.CreatedAt<=date2).Sum(o => o.TotalPrice);
        }

        public decimal CalculateCashPayment()
        {
            return _context.Orders.Where(o => o.Status != true).Sum(o => o.TotalPrice);
        }

        public decimal CalculateCashPaymentByDate(DateTime date1, DateTime date2)
        {
            return _context.Orders.Where(o => o.Status != true && (o.CreatedAt >= date1 && o.CreatedAt <= date2)).Sum(o => o.TotalPrice);
        }

        public bool CheckOrderStatus()
        {
            return _context.Orders.Any(o => o.Status != true);
        }

        public int Sum()
        {
            return _context.Orders.Sum(o => o.DigitForSum);
        }
    }
}
