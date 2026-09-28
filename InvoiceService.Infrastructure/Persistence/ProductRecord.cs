namespace InvoiceService.Infrastructure.Persistence;

public class ProductRecord
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public string? Color { get; set; }
    public decimal ListPrice { get; set; }
}