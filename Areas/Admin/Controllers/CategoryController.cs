using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index(int page = 1 , int pageSize = 5)
        {
            var category = categoryRepository.Get();

            int totalCategorys = category.Count();
            int totalPages = (int)Math.Ceiling(totalCategorys / (double)pageSize);

            if (page < 1 || page > totalPages)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });

            category = category.OrderBy(e=>e.Id).Skip((page - 1)*pageSize).Take(pageSize).ToList();

            ViewBag.currentPage = page;
            ViewBag.TptalPage = totalPages;
            ViewBag.PageSize = pageSize;

            return View(category.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            //ModelState.Remove("Products");
            if(ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();
                TempData["notifaction"] = "Created Category Successfuly";
                return RedirectToAction(nameof(Index));

            }
               
            
                return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(e=>e.Id == categoryId);

            if (category != null) 
               return View(category);
            
            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
           
                categoryRepository.Edit(category);
                categoryRepository.Commit();

                TempData["notifaction"] = "Update Category Successfuly";


                return RedirectToAction(nameof(Index));
            
        }

       
        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetOne(e=>e.Id==categoryId);
            if (category != null)
            {

                categoryRepository.Delete(category);
                categoryRepository.Commit();
                TempData["notifaction"] = "Delete Category Successfuly";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
