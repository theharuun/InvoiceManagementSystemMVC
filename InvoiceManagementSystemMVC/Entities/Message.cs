using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceManagementSystemMVC.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

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
