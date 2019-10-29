using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace LibManagement.Models
{
    class BookOrder
    {
        [Key]
        public int BookOrderId { get; set; }

        public int BookId { get; set; }

        public Book book { get; set; }

        public int OrderId { get; set; }

        public Order order { get; set; }
    }
}
