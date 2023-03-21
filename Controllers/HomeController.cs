using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}