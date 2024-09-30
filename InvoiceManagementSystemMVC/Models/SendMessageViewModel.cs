using InvoiceManagementSystemMVC.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManagementSystemMVC.Models
{
    public class SendMessageViewModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentDate { get; set; } = DateTime.UtcNow; // Varsayılan olarak şu anki tarih

        // Gönderen kullanıcıyı tanımlamak için
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public UserAdmin Sender { get; set; }

        // Alıcı kullanıcıyı tanımlamak için
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public UserAdmin Receiver { get; set; }
    }
}
