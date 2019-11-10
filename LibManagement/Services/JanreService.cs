using System.Collections.Generic;
using System.Linq;
using LibManagement.DAL;
using LibManagement.Models;

namespace LibManagement.Services
{

    class JanreService
    {
        private readonly LibManagementContext _context;

        public JanreService()
        {
            _context = new LibManagementContext();
        }

        public void Add(Janre janre)
        {
            _context.Janres.Add(janre);
        }
        public List<Janre> AllJanre()
        {
            return _context.Janres.ToList();
        }
    }
}
