using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Models;

namespace WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<InvoiceDto> Invoices { get; set; } = new();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var result = await client
                .GetFromJsonAsync<List<InvoiceDto>>("api/invoices");

            Invoices = result ?? new List<InvoiceDto>();
        }
    }
}