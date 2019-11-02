using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibManagement.Models
{
    class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Deadline { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal TotalRentPrice { get; set; }


        [Column(TypeName = "money")]
        public decimal FineForLate { get; set; }


        [Required]
        [Column(TypeName = "bit")]
        public bool Status { get; set; }

        public int UserId { get; set; }

        public User user { get; set; }

        public int CustomerId { get; set; }

        public Customer customer { get; set; }

        public int BookId { get; set; }

        public Book book { get; set; }

    }
}
