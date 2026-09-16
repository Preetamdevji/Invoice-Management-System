using InvoiceService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.Interfaces
{
    public interface IProductServiceClient
    {
        Task<List<ProductOptionDto>> GetProductOptionsAsync();
        Task<List<SpecialOfferOptionDto>> GetSpecialOfferOptionsAsync(int productId);
    }
}
