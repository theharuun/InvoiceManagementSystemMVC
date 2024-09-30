using InvoiceManagementSystemMVC.Entities;

namespace InvoiceManagementSystemMVC.Models
{
    public class CreateBillViewModel
    {
        public int? BillType { get; set; }
        public long? BillPayment { get; set; }
        public int? ApartmentId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Status { get; set; }

        public BillType BillTypeN { get; set; }
        public Apartment Apartment { get; set; }
    }
}
