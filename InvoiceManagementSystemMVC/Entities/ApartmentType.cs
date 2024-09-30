namespace InvoiceManagementSystemMVC.Entities
{
    public class ApartmentType
    {
        public short ATypeId { get; set; }
        public string ApartmentTypeName { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }
    }

}
