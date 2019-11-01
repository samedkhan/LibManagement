using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LibManagement.Models;

namespace LibManagement.DAL
{
    class LibManagementContext:DbContext
    {
        public LibManagementContext() : base("LibManagement")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<BookOrder> BookOrders { get; set; }

        public DbSet<Janre> Janres { get; set; }


    }
}
