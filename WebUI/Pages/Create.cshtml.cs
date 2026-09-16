using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Models;

namespace WebUI.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateInvoiceDataDto FormData { get; set; } = new();

        [BindProperty]
        public CreateInvoiceDto Invoice { get; set; } = new();

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            await LoadFormDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var response = await client.PostAsJsonAsync("api/invoices", Invoice);

            if (!response.IsSuccessStatusCode)
            {
                await LoadFormDataAsync();
                ModelState.AddModelError(string.Empty, "Failed to create invoice.");
                return Page();
            }

            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnGetSpecialOffersAsync(int productId)
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var offers = await client
                .GetFromJsonAsync<List<SpecialOfferOptionDto>>(
                    $"api/invoices/products/{productId}/special-offers");

            return new JsonResult(offers ?? new List<SpecialOfferOptionDto>());
        }
        private async Task LoadFormDataAsync()
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var result = await client
                .GetFromJsonAsync<CreateInvoiceDataDto>("api/invoices/create-data");

            FormData = result ?? new CreateInvoiceDataDto();
        }
    }
}