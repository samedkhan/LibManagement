using System.Collections.Generic;
using System.Linq;
using LibManagement.DAL;
using LibManagement.Models;

namespace LibManagement.Services
{

    class BookService
    {
        private readonly LibManagementContext _context;

        public BookService()
        {
            _context = new LibManagementContext();
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);

            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            _context.Entry(book).State = System.Data.Entity.EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);

            _context.SaveChanges();
        }

        public List<Book> AllBook()
        {
            List<Book> Books;
            Books = _context.Books.Include("janre").ToList();
            return Books;
        }

       

        public bool Contain(string name)
        {
            return _context.Books.Include("janre").Any(b => b.Name.Contains(name));
        }

        public Book Find(int id)
        {
            return _context.Books.Where(b => b.BookId == id).FirstOrDefault();
        }


        public int Sum()
        {
            return _context.Books.Sum(b => b.DigitForSum);
        }
    }
}
