using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Models;

namespace WebUI.Pages
{
    public class CreateStep2Model : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAntiforgery _antiforgery;

        public List<ProductOptionDto> Products { get; set; } = new();
        public string AntiforgeryToken { get; set; } = string.Empty;

        public CreateStep2Model(IHttpClientFactory httpClientFactory, IAntiforgery antiforgery)
        {
            _httpClientFactory = httpClientFactory;
            _antiforgery = antiforgery;
        }

        public async Task OnGetAsync()
        {
            // Token generate karo aur cookie set karo response mein
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            AntiforgeryToken = tokens.RequestToken;

            var client = _httpClientFactory.CreateClient("InvoiceService");

            var result = await client
                .GetFromJsonAsync<CreateInvoiceDataDto>("api/invoices/create-data");

            if (result != null)
            {
                Products = result.Products;
            }
        }

        public async Task<IActionResult> OnGetSpecialOffersAsync(int productId)
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var response = await client.GetAsync(
                $"api/invoices/products/{productId}/special-offers");

            if (!response.IsSuccessStatusCode)
            {
                return new JsonResult(new List<SpecialOfferOptionDto>());
            }

            var raw = await response.Content.ReadAsStringAsync();

            var offers = System.Text.Json.JsonSerializer.Deserialize<List<SpecialOfferOptionDto>>(
                raw,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new JsonResult(offers ?? new List<SpecialOfferOptionDto>());
        }

        
        public async Task<IActionResult> OnPostSubmitAsync([FromBody] CreateInvoiceDto invoice)
        {
            var client = _httpClientFactory.CreateClient("InvoiceService");

            var response = await client.PostAsJsonAsync("api/invoices", invoice);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                return new JsonResult(new { success = false, error = errorBody }) { StatusCode = 400 };
            }

            var resultBody = await response.Content.ReadFromJsonAsync<object>();
            return new JsonResult(new { success = true, data = resultBody });
        }
    }
}