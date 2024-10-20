using ArtFold.Data;
using ArtFold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ArtFold.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ArtFoldDbContext _context;

        public HomeController(ILogger<HomeController> logger, ArtFoldDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult GetFeaturedProduct()
        {
            var products = _context.Products
                .Include(p => p.Category)
                //.OrderBy(p => Guid.NewGuid())
                .Take(8).ToList();
            return PartialView("_FeaturedProduct", products);
        }

        public IActionResult GetProductsByCategory(string categoryName)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.CategoryName == categoryName)
                .ToList();
            return PartialView("_FeaturedProduct", products);
        }

        public IActionResult GetRandomProduct()
        {
            var products = _context.Products
                .OrderBy(p => Guid.NewGuid())
                .Take(3)
                .ToList();
            return PartialView("_TopProduct", products);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
