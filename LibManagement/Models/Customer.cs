using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibManagement.Models
{
    class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [MaxLength(50)]
        [Required]
        public string IdCode { get; set; }

        [MaxLength(100)]
        [Required]
        public string FullName { get; set; }

        [MaxLength(100)]
        [Required]
        public string PhoneNumber { get; set; }

        public List<Order> orders { get; set; }
    }
}
