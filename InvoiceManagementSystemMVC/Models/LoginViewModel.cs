using System.ComponentModel.DataAnnotations;

namespace InvoiceManagementSystemMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is Required!!!")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Max lenght 100")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required!!!")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Max lenght 20")]
        public string Password { get; set; }

        public bool? Role { get; set; }
    }
}
