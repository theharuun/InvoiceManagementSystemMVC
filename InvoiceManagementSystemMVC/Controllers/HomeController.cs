using InvoiceManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using InvoiceManagementSystemMVC.DataContext;

namespace InvoiceManagementSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.UserAdmins.Where(s => s.Email == model.Email && s.Password == model.Password && s.Role == model.Role).FirstOrDefault();
                if (user != null)
                {
                    //success
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("Name"  ,user.Name),
                         new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Add user ID
                        new Claim("Role", user.Role == true ? "Admin" : user.Role == false ? "User" : "Guest") // Role based on Role value
                    };



                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                    // Kullanýcýnýn rolüne göre yönlendirme
                    if (user.Role is false)
                    {
                        return RedirectToAction("SecurePage", "User"); // Kullanýcý rolü ise UserController'a yönlendir
                    }
                    else
                    {
                        return RedirectToAction("SecurePage", "Admin"); // Diðer roller için AdminController'a yönlendir
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not correct .");
                    ModelState.AddModelError("", "Please select corectly your role .");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
