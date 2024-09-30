using InvoiceManagementSystemMVC.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManagementSystemMVC.Models
{
    
   
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(TcNo), IsUnique = true)]
    public class AddUserViewModel
    {
        
            [Required(ErrorMessage = "Name is Required!!!")]
            [MaxLength(100, ErrorMessage = "Max lenght 100")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Surname is Required!!!")]
            [MaxLength(100, ErrorMessage = "Max lenght 100")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "TcNo is Required!!!")]
            [MaxLength(11, ErrorMessage = "Max lenght 11")]
            public string TcNo { get; set; }

            [Required(ErrorMessage = "Email is Required!!!")]
            [DataType(DataType.EmailAddress)]
            [MaxLength(100, ErrorMessage = "Max lenght 100")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is Required!!!")]
            [DataType(DataType.Password)]
            [MaxLength(20, ErrorMessage = "Max lenght 20")]
            public string Password { get; set; }
            public int? ApartmentId { get; set; }
            public bool? Role { get; set; }

            public Apartment Apartment { get; set; }
        }

    }
