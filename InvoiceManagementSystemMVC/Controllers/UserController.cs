using InvoiceManagementSystemMVC.DataContext;
using InvoiceManagementSystemMVC.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InvoiceManagementSystemMVC.Controllers
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; // Kullanıcının e-posta adresini al
            var userAdmin = context.UserAdmins.FirstOrDefault(u => u.Email == userEmail); // Kullanıcıyı al

            // Kullanıcı bulunamazsa, uygun bir hata mesajı veya yönlendirme yapabilirsiniz
            if (userAdmin == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return View(new List<UserAdmin> { userAdmin }); // Sadece bu kullanıcıyı listele
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
        public async Task<IActionResult> Details(int id)
        {
            // Kullanıcıyı ve ilgili apartman bilgilerini al
            var user = await context.UserAdmins
                .Include(u => u.Apartment) // Apartment ilişkisini dahil et
                .ThenInclude(a => a.ApartmentTypeNavigation)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User is NOT FOUND"); // Kullanıcı bulunamazsa 404 döndür
            }

            return View(user); // Detayları görüntülemek için view'ı döndür
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            // Kullanıcıyı ID'sine göre veritabanından al
            var user = await context.UserAdmins.FindAsync(id);

            // Kullanıcı bulunamadıysa hata mesajı döndür
            if (user == null)
            {
                return NotFound("User is NOT FOUND");
            }

            return View(user); // Kullanıcının verisini döndür
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
                user.Password = viewModel.Password;

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
    }
}
