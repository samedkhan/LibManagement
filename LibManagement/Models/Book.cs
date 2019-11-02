using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibManagement.Models
{
    class Book
    { 
    
        [Key]
        public int BookId { get; set; }

        [MaxLength(200)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string AutorName { get; set; }
                

        public int TotalPiece { get; set; }

        
        [Column(TypeName = "money")]
        
        public decimal SalePrice { get; set; }

        [Column(TypeName = "money")]
        
        public decimal RentPrice { get; set; }

        public int JanreId { get; set; }

        public Janre janre { get; set; }

        public List<Order> Orders { get; set; }

    }
}
