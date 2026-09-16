using ProductService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductOptionDto>> GetProductOptionsAsync();
        Task<List<SpecialOfferOptionDto>> GetSpecialOfferOptionsAsync(int productId);
    }
}
