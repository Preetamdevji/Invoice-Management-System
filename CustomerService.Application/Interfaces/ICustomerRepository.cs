using CustomerService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<CustomerListItemDto>> GetAllAsync();
        Task<List<CustomerOptionDto>> GetCustomerOptionsAsync();
    }
}
