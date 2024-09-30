using InvoiceManagementSystemMVC.DataContext;
using InvoiceManagementSystemMVC.Entities;
using InvoiceManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InvoiceManagementSystemMVC.Controllers
{
    public class ApartmentController : Controller
    {
        private readonly ApplicationDbContext context;

        public ApartmentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Apartman listesini görüntüle
        public async Task<IActionResult> Index()
        {
            var apartments = await context.Apartments
                .Include(a => a.UserAdmin) // UserAdmin ile ilişkili bilgileri dahil et
                .ToListAsync();
            return View(apartments);
        }

        [Authorize]
        public async Task<IActionResult> UserIndex()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; // Kullanıcının e-posta adresini al

            var apartments = await context.Apartments
                .Include(a => a.UserAdmin) // UserAdmin ile ilişkili bilgileri dahil et
                .Where(a => a.UserAdmin.Email == userEmail) // Kullanıcıya ait daireleri filtrele
                .ToListAsync();

            return View(apartments); // Kullanıcıya ait daireleri görünümde kullan
        }




        // Yeni apartman oluşturma sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        // Yeni apartman oluşturma işlemi
        [HttpPost]
        public async Task<IActionResult> Create(CreateApartmentViewModel viewModel)
        {
            // Kontrol edilecek alanlar
            var fieldsToValidate = new (string FieldName, object Value)[]
            {
                ("ApartmentTypeId", viewModel.ApartmentTypeId),
                ("Floor", viewModel.Floor),
                ("Block", viewModel.Block),
                ("Status", viewModel.Status)
            };


            // Her alanı kontrol et
            foreach (var field in fieldsToValidate)
            {
                if (field.Value == null)
                {
                    ModelState.AddModelError(field.FieldName, $"{field.FieldName} is required!");
                }
            }


            var apartment = new Apartment
            {
                ApartmentTypeId = viewModel.ApartmentTypeId,
                Floor = viewModel.Floor,
                Block = viewModel.Block,
                UserId = viewModel.UserId,
                Status = viewModel.Status,
            };

            await context.Apartments.AddAsync(apartment);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Apartment");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var apartment = await context.Apartments.FindAsync(id);

            return View(apartment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Apartment viewModel)
        {

            // Kullanıcıyı veritabanında bul
            var apartment = await context.Apartments.FindAsync(viewModel.ApartmentId);
            if (apartment != null)
            {
                apartment.ApartmentTypeId = viewModel.ApartmentTypeId;
                apartment.Floor = viewModel.Floor;
                apartment.Block = viewModel.Block;
                apartment.UserId = viewModel.UserId;
                apartment.Status = viewModel.Status;

                // Değişiklikleri kaydet
                await context.SaveChangesAsync();

                // Başarı mesajını ayarla
                TempData["SuccessMessage"] = "Apartment updated successfully.";

            }
            else
            {
                // Kullanıcı bulunamadıysa hata mesajı ayarla
                TempData["ErrorMessage"] = "Apartment not found.";
            }

            // Admin sayfasına yönlendir
            return View(new Apartment());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Kullanıcıyı ve ilgili apartman bilgilerini al
            var bill = await context.Apartments
                .Include(u => u.ApartmentTypeNavigation)
                .Include(u => u.UserAdmin) // Apartment ilişkisini dahil et
                .Include(u=>u.Bills)
                .ThenInclude(u => u.BillTypeN)
                .FirstOrDefaultAsync(u => u.ApartmentId == id);

            if (bill == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }

            return View(bill); // Detayları görüntülemek için view'ı döndür
        }


        public async Task<IActionResult> Delete(int id)
        {
            var apartment = await context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return RedirectToAction("Index", "Apartment");
            }
            context.Apartments.Remove(apartment);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Apartment");

        }
    }
}

