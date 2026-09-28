using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Models;

namespace WebUI.Pages
{
    public class CreateStep1Model : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<CustomerOptionDto> Customers { get; set; } = new();
        public List<AddressOptionDto> Addresses { get; set; } = new();
        public List<LookupDto> ShipMethods { get; set; } = new();

        public CreateStep1Model(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var result = await client
                .GetFromJsonAsync<CreateInvoiceDataDto>("api/invoices/create-data");

            if (result != null)
            {
                Customers = result.Customers;
                Addresses = result.Addresses;
                ShipMethods = result.ShipMethods;
            }
        }
    }
}