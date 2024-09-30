using InvoiceManagementSystemMVC.DataContext;
using InvoiceManagementSystemMVC.Entities;
using InvoiceManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InvoiceManagementSystemMVC.Controllers
{
    
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.UserAdmins.ToList());
        }

    

        [Authorize]
        public IActionResult SecurePage() 
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.Name = userName;

            var userRole = HttpContext.User.FindFirst("Role")?.Value; // Kullanıcı rolünü al
            ViewBag.Role = userRole;

            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel viewModel)
        {
            var user = new UserAdmin
            {
                Name = viewModel.Name,
                Surname = viewModel.Surname,
                TcNo = viewModel.TcNo,
                Email = viewModel.Email,
                Password = viewModel.Password,
                ApartmentId = viewModel.ApartmentId,
                Role =viewModel.Role
            };

            await context.UserAdmins.AddAsync(user);

            await context.SaveChangesAsync();

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await context.UserAdmins.FindAsync(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserAdmin viewModel)
        {
          
            // Kullanıcıyı veritabanında bul
            var user = await context.UserAdmins.FindAsync(viewModel.UserId);
            if (user != null)
            {
                // Kullanıcı bilgilerini güncelle
                user.Name = viewModel.Name;
                user.Surname = viewModel.Surname;
                user.TcNo = viewModel.TcNo;
                user.Email = viewModel.Email;
                user.ApartmentId = viewModel.ApartmentId;
                user.Role = viewModel.Role;

                // Değişiklikleri kaydet
                await context.SaveChangesAsync();

                // Başarı mesajını ayarla
                TempData["SuccessMessage"] = "User updated successfully.";

            }
            else
            {
                // Kullanıcı bulunamadıysa hata mesajı ayarla
                TempData["ErrorMessage"] = "User not found.";
            }

            // Admin sayfasına yönlendir
            return View(new UserAdmin());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Kullanıcıyı ve ilgili apartman bilgilerini al
            var user = await context.UserAdmins
                .Include(u => u.Apartment) // Apartment ilişkisini dahil et
                .ThenInclude(a=>a.ApartmentTypeNavigation)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }

            return View(user); // Detayları görüntülemek için view'ı döndür
        }


        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await context.UserAdmins.FindAsync(id);

            if (user == null)
            {
                return RedirectToAction("Index","Admin");
            }
            context.UserAdmins.Remove(user);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");

        }
        


    }
}
