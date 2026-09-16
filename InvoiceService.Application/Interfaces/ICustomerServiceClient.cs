using InvoiceService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.Interfaces
{
    public interface ICustomerServiceClient
    {
        Task<List<CustomerOptionDto>> GetCustomerOptionsAsync();
    }
}
