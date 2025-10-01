using E_Commerce.Models;
using E_Commerce.Repositories;
using E_Commerce.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductController(IProductRepository productRepository , ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index(Product product, int page = 1 , int pageSize = 5)
        {
            var products = productRepository.Get(includes: [e => e.Category]);

            int totalUsers = products.Count();
            int totalPage = (int)Math.Ceiling(totalUsers / (double)pageSize);

            products = products.Skip((page - 1)*pageSize).Take(5).ToList();
            if (page < 1 || page > totalPage)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });

            ViewBag.CurrentPage =page;
            ViewBag.TotalPage = totalPage;
           


            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = categoryRepository.Get();

            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();

            return View(new Product());
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile? file)
        {


            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Save img in wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save img name in db
                    product.Img = fileName;
                }

                productRepository.Create(product);
                productRepository.Commit();

                return RedirectToAction("Index");
            }

            var categories = categoryRepository.Get();
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int productId)
        {
            var productById= productRepository.GetOne(e=>e.Id==productId);

             var categories = categoryRepository.Get();
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList();
            if (productById != null)
            {
                return View(productById);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Product product , IFormFile? file)
        {
            var productById = productRepository.GetOne(e=>e.Id == product.Id, tracked:false);
            if (productById != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", productById.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                product.Img = fileName;
            }
            else
                product.Img = productById.Img;

            if(product != null)
            {
                productRepository.Edit(product);
                productRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var product = productRepository.GetOne(e => e.Id == productId);

            if (product != null)
            {
                // Delete old img from wwwroot
                if (product.Img != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.Img);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Delete img name in db
                productRepository.Delete(product);
                productRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
