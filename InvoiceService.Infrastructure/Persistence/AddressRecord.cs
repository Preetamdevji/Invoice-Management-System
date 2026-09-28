namespace InvoiceService.Infrastructure.Persistence;

public class AddressRecord
{
    public int AddressId { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}