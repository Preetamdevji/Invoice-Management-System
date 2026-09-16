using InvoiceService.Application.DTOs;
using InvoiceService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace InvoiceService.Infrastructure.Clients
{
    public class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _httpClient;
        public CustomerServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CustomerOptionDto>> GetCustomerOptionsAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<List<CustomerOptionDto>>("api/customers/options");

            return result ?? new List<CustomerOptionDto>();
        }
    }
}
