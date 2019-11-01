using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibManagement.Models
{
    class User
    {
        [Key]
        public int UserId { get; set; }

       
        [MaxLength(100)]
        public string FullName { get; set; }

       
        [MaxLength(50)]
        public string Username { get; set; }

        
        [Column(TypeName = "bit")]
        public bool AdminOrUser { get; set; }

     
        [MaxLength(100)]
        public string Password { get; set; }

        public List<Order> orders { get; set; }
    }
}
