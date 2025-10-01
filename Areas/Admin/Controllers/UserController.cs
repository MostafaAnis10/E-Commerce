using E_Commerce.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IApplicationUserRepository _userRepository;

        public UserController(IApplicationUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public IActionResult Index(string? quary , int page = 1 , int pageSize = 5)
        {
           var users =  _userRepository.Get();
            if(quary != null)
            { 
                users = users.Where(e => e.UserName.Contains(quary) || e.Email.Contains(quary)).ToList();
            }

            int totalUsers = users.Count();
            int totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            if (totalPages < page - 1)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });

            users = users.Skip((page - 1) * 5).Take(5);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Query = quary;

            return View(users.ToList());


        }
    } 
}  
