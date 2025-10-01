using System.Diagnostics;
using E_Commerce.DataAccess;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        ApplicationDbContext dbContext = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? categoryName, int? rating, string? productName ,string? PriceRange)
        {
            IQueryable<Product> products = dbContext.products.Include(e => e.Category);

            if (categoryName != null)
            {
                products = products.Where(e => e.Category.Name == categoryName);
            }

            if (productName != null)
            {
                products = products.Where(e => e.Name.Contains(productName));
            }
            if (PriceRange != null)
            {
                if (PriceRange == "low")
                    products = products.Where(e => e.Price < 100);
                else if (PriceRange == "mid")
                    products = products.Where(e => e.Price >= 100 && e.Price <= 500);
                else if (PriceRange == "high")
                    products = products.Where(e => e.Price > 500);
            }

            #region List of categories
            var categories = dbContext.categories.ToList();
            var allProduct =  dbContext.products.Include(e=>e.Category);
            ViewBag.categories = categories;
            #endregion
            return View(allProduct.ToList());
        }

        public IActionResult Details(int productId)
        {
            var product = dbContext.products.Include(e => e.Category).FirstOrDefault(e => e.Id == productId);

            if (product != null)
            {
              
                var productWithRelated = new
                {
                    product = product,
                    Related = dbContext.products
                            .Include(e => e.Category)
                            .Where(e => e.CategoryId == product.CategoryId && e.Id != product.Id)
                            .Take(4)
                            .ToList()
            }
            ;

                return View(productWithRelated);
            }

            return RedirectToAction(nameof(NotFoundPage));
        }

        public IActionResult NotFoundPage()
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
