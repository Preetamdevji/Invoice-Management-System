using InvoiceService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<List<InvoiceDto>> GetInvoicesAsync();
        Task<int> CreateInvoiceAsync(CreateInvoiceDto invoice);
        Task<List<AddressOptionDto>> GetAddressOptionsAsync();
        Task<List<LookupDto>> GetShipMethodOptionsAsync();


    }
}
