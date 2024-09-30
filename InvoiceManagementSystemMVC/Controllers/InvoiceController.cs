using InvoiceManagementSystemMVC.DataContext;
using InvoiceManagementSystemMVC.Entities;
using InvoiceManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InvoiceManagementSystemMVC.Controllers
{
    public class InvoiceController : Controller
    {

        private readonly ApplicationDbContext context;

        public InvoiceController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Bills.ToList());
        }

        [Authorize]
        public IActionResult UserIndex()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; // Kullanıcının e-posta adresini al
            if (string.IsNullOrEmpty(userEmail))
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // Kullanıcıyı e-posta adresine göre bul
            var user = context.UserAdmins.FirstOrDefault(u => u.Email == userEmail);
            if (user == null || user.ApartmentId == null)
            {
                return NotFound("Kullanıcı veya apartman bulunamadı.");
            }

            // Kullanıcının apartmanına ait faturaları al
            var invoices = context.Bills.Where(b => b.ApartmentId == user.ApartmentId).ToList(); // Kullanıcının apartmanına ait faturaları al

            return View(invoices); // Faturaları view'a gönder
        }




        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBillViewModel viewModel)
        {
            // Kontrol edilecek alanlar
            var fieldsToValidate = new (string FieldName, object Value)[]
            {
                ("BillType", viewModel.BillType),
                ("BillPayment", viewModel.BillPayment),
                ("ApartmentId", viewModel.ApartmentId),
                ("DueDate", viewModel.DueDate),
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

       


            var bill = new Bill
            {
                
                BillType = viewModel.BillType,
                BillPayment=viewModel.BillPayment,
                ApartmentId = viewModel.ApartmentId,
                DueDate = viewModel.DueDate,
                Status = viewModel.Status,
            };

            await context.Bills.AddAsync(bill);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Invoice");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bill = await context.Bills.FindAsync(id);

            return View(bill);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Bill viewModel)
        {

            // Kullanıcıyı veritabanında bul
            var bill = await context.Bills.FindAsync(viewModel.BillId);
            if (bill != null)
            {
                // Kullanıcı bilgilerini güncelle
                bill.BillType = viewModel.BillType;
                bill.BillPayment = viewModel.BillPayment;
                bill.ApartmentId = viewModel.ApartmentId;
                bill.DueDate = viewModel.DueDate;
                bill.Status = viewModel.Status;


                // Değişiklikleri kaydet
                await context.SaveChangesAsync();

                // Başarı mesajını ayarla
                TempData["SuccessMessage"] = "Bill updated successfully.";

            }
            else
            {
                // Kullanıcı bulunamadıysa hata mesajı ayarla
                TempData["ErrorMessage"] = "Bill not found.";
            }

            // Admin sayfasına yönlendir
            return View(new Bill());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Kullanıcıyı ve ilgili apartman bilgilerini al
            var bill = await context.Bills
                .Include(u => u.BillTypeN)
                .Include(u => u.Apartment) // Apartment ilişkisini dahil et
                .ThenInclude(u=>u.UserAdmin)
                .FirstOrDefaultAsync(u => u.BillId == id);

            if (bill == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }

            return View(bill); // Detayları görüntülemek için view'ı döndür
        }


        public async Task<IActionResult> Delete(int id)
        {
            var bill = await context.Bills.FindAsync(id);

            if (bill == null)
            {
                return RedirectToAction("Index", "Invoice");
            }
            context.Bills.Remove(bill);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Invoice");

        }

    }
}
