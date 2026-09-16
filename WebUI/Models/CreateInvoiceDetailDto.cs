namespace WebUI.Models
{
    public class CreateInvoiceDetailDto
    {
        public int ProductId { get; set; }
        public int SpecialOfferId { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
    }
}
