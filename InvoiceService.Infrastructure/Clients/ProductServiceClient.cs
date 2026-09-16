using InvoiceService.Application.DTOs;
using InvoiceService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace InvoiceService.Infrastructure.Clients
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _httpClient;
        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProductOptionDto>> GetProductOptionsAsync()
        {
            var result = await _httpClient.
                GetFromJsonAsync<List<ProductOptionDto>>("api/products/options");
            
            return result ?? new List<ProductOptionDto>();
        }

        public async Task<List<SpecialOfferOptionDto>> GetSpecialOfferOptionsAsync(int productId)
        {
            var result = await _httpClient.
                GetFromJsonAsync<List<SpecialOfferOptionDto>>($"api/products/{productId}/special-offers");

            return result ?? new List<SpecialOfferOptionDto>();
        }

    }
}
