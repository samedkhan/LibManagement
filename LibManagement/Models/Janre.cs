using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibManagement.Models;


namespace LibManagement.Models
{
    class Janre
    {
        public int JanreId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}
