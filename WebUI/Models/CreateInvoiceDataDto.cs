namespace WebUI.Models
{
    public class CreateInvoiceDataDto
    {
        public List<CustomerOptionDto> Customers { get; set; } = new();
        public List<ProductOptionDto> Products { get; set; } = new();
        public List<AddressOptionDto> Addresses { get; set; } = new();
        public List<LookupDto> ShipMethods { get; set; } = new();
    }
}
