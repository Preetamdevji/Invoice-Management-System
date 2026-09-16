namespace WebUI.Models
{
    public class InvoiceDto
    {
        public int CustomerId { get; set; }
        public int SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalDue { get; set; }

        public List<InvoiceDetailDto> Details { get; set; } = new();
    }
}
