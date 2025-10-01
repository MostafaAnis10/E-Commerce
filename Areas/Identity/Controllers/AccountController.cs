using E_Commerce.Models;
using E_Commerce.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Company"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ()
                {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Address = registerVM.Address,
                
                };

             var result =  await _userManager.CreateAsync(applicationUser, registerVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser , "Customer");
                    // Success Register
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    // Error
                    ModelState.AddModelError("Password", "Don't match the constraints");
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
              var appUser =await _userManager.FindByEmailAsync(loginVM.Email);
                if(appUser != null)
                {
                 var result = await  _userManager.CheckPasswordAsync(appUser, loginVM.Password);
                    if(result)
                    {
                        //Login
                        await _signInManager.SignInAsync(appUser, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "Customer" }); 
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Don't match the constraints");
                        ModelState.AddModelError("Email", "Can not found the email");
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Don't match the constraints");
                    ModelState.AddModelError("Email", "Can not found the email");
                }

                
            }
            return View(loginVM);
        }

        public async Task<IActionResult> Logout ()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
            
        }

        public IActionResult AccessDenied() => View();
    }
}
