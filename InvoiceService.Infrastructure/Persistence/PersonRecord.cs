namespace InvoiceService.Infrastructure.Persistence;

public class PersonRecord
{
    public int BusinessEntityId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}