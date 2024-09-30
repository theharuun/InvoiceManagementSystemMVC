namespace InvoiceManagementSystemMVC.Entities
{
    public class Apartment
    {
        public int ApartmentId { get; set; }
        public short? ApartmentTypeId { get; set; }
        public string Floor { get; set; }
        public string Block { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }

        public ApartmentType ApartmentTypeNavigation { get; set; }
        public UserAdmin UserAdmin { get; set; }
        public ICollection<Bill> Bills { get; set; }  // Bir Apartment birden fazla Bill'e sahip olabilir
    }
    

}
