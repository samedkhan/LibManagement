using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibManagement.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        
        [MaxLength(200)]
        [Required]
        public string FullName { get; set; }

        [MaxLength(100)]
        [Required]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "bit")]
        public bool IsPassiv { get; set; }

        [Column(TypeName ="date")]
        public DateTime CreatedAt { get; set; }
        
        public List<Order> orders { get; set; }
    }
}
