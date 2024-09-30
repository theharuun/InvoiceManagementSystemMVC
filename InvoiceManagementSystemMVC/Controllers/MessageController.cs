using InvoiceManagementSystemMVC.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManagementSystemMVC.Entities;
using InvoiceManagementSystemMVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceManagementSystemMVC.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mesajları Listeleme
        [HttpGet]
        public async Task<IActionResult> Index(int userId)
        {
            Console.WriteLine(userId);
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.ReceiverId == userId || m.SenderId == userId) // Hem alıcı hem de gönderici kontrolü
                .ToListAsync();

            return View(messages); // Filtrelenmiş mesajları görüntülemek için bir View döndürüyoruz
        }


        [Authorize]
        public async Task<IActionResult> UserIndex()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; // Kullanıcının e-posta adresini al
            var user = await _context.UserAdmins.FirstOrDefaultAsync(u => u.Email == userEmail); // Kullanıcıyı bul

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            var messages = await _context.Messages
                .Include(m => m.Sender) // Gönderen bilgilerini dahil et
                .Include(m => m.Receiver) // Alıcı bilgilerini dahil et
                .Where(m => m.ReceiverId == user.UserId || m.SenderId == user.UserId) // Kullanıcıya ait mesajları filtrele
                .ToListAsync();

              ViewBag.UserId = user.UserId; // Kullanıcı ID'sini ViewBag ile geçiyoruz

            return View(messages); // Kullanıcıya ait mesajları görünümde kullan
        }




        // Kullanıcının hem gelen hem de gönderilen mesajlarını göstermek için
        public async Task<IActionResult> ListMessage()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    // Eğer kullanıcı ID'si yoksa hata döndür veya kullanıcıyı yönlendir
                    return BadRequest("Kullanıcı ID'si bulunamadı.");
                }

                var userId = int.Parse(userIdClaim.Value); // ID'yi string'ten int'e çeviriyoruz

                var messages = await _context.Messages
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                    .ToListAsync();

                // Gelen ve gönderilen mesajları ayırt etmek için ViewData kullanabiliriz
                ViewData["ReceivedMessages"] = messages.Where(m => m.ReceiverId == userId).ToList();
                ViewData["SentMessages"] = messages.Where(m => m.SenderId == userId).ToList();

                return View("ListMessage", messages); // ListMessage görünümüne mesajları gönder
            }
            else
            {
                // Kullanıcı oturum açmamışsa giriş sayfasına yönlendir
                return RedirectToAction("Login", "Admin");
            }
        }


        // GET: Yeni Mesaj Gönder
        public IActionResult SendMessage(int userId)
        {

            if (userId == 9)
            {
                return BadRequest("Admin Kendisine Mesaj Gönderemez .");
            }
            // Get the logged-in user's ID from claims
            var senderId = 9;

            var viewModel = new SendMessageViewModel
            {
                SenderId = senderId,// Set the sender ID appropriately
                ReceiverId = userId
            };

            return View(viewModel); // Send the view model to the view
        }

      


        // POST: Mesaj Gönderme
        [HttpPost]
        public async Task<IActionResult> SendMessage(int userId,SendMessageViewModel viewModel)
        {
           
                var message = new Message
                {
                    Content = viewModel.Content,
                    SentDate = viewModel.SentDate,
                    SenderId = viewModel.SenderId,
                    ReceiverId = userId
                };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Message", new { userId = userId });


        }

        public IActionResult SendMessageByUser(int receiverId,int userId)
        {
           

            var viewModel = new SendMessageViewModel
            {
                SenderId = userId,// Set the sender ID appropriately
                ReceiverId = receiverId
            };

            return View(viewModel); // Send the view model to the view   
        }
        // POST: Mesaj Gönderme
        [HttpPost]
        public async Task<IActionResult> SendMessageByUser(int userId, SendMessageViewModel viewModel)
        {

            var message = new Message
            {
                Content = viewModel.Content,
                SentDate = viewModel.SentDate,
                SenderId = viewModel.SenderId,
                ReceiverId = 9
                
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserIndex", "Message"); // Mesaj gönderildikten sonra mesajları listele

        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Kullanıcıyı ve ilgili mesajı al
            var message = await _context.Messages
                .Include(m => m.Sender)   // Include the sender
                .Include(m => m.Receiver) // Include the receiver
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa 404 döndür
            }

            // Detayları görüntülemek için view'ı döndür
            return View(message); // Pass the message directly to the view
        }

        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            // Eğer kullanıcının rolü belirlenemiyorsa varsayılan sayfaya yönlendir

            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; // Kullanıcının e-posta adresini al
            var user = await _context.UserAdmins.FirstOrDefaultAsync(u => u.Email == userEmail); // Kullanıcıyı bul
            

            // Eğer kullanıcı adminse adminin "Index" sayfasına yönlendir
            if (user.Role is true)
            {
                return RedirectToAction("ListMessage", "Message");
            }
            else
            {
                return RedirectToAction("UserIndex", "Message");
            }

        
        }

    }
}
