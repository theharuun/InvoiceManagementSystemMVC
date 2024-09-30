namespace InvoiceManagementSystemMVC.Entities
{
    public class BillType
    {
        public int BTypeId { get; set; }
        public string BillTypeName { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }

}
