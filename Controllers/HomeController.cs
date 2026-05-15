using Microsoft.AspNetCore.Mvc;
using Ticket15.DAL;
using Ticket15.Models;

namespace Ticket15.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;

        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index()
        {
            List<Agent> agents = new List<Agent>();

      
            return View(_context.Agents.ToList());

        }
    }
}
