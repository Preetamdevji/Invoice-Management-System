using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Infrastructure;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductOptionDto>> GetProductOptionsAsync()
    {
        return await _context.Products
            .Where(p => p.ListPrice > 0)
            .OrderBy(p => p.ProductId)
            .Select(p => new ProductOptionDto
            {
                ProductId = p.ProductId,
                ProductName = p.Name,
                ProductNumber = p.ProductNumber,
                ListPrice = p.ListPrice
            })
            .ToListAsync();
    }

    public async Task<List<SpecialOfferOptionDto>> GetSpecialOfferOptionsAsync(int productId)
    {
        var query =
            from sop in _context.SpecialOfferProducts
            join so in _context.SpecialOffers
                on sop.SpecialOfferId equals so.SpecialOfferId
            where sop.ProductId == productId
            orderby so.SpecialOfferId
            select new SpecialOfferOptionDto
            {
                SpecialOfferId = so.SpecialOfferId,
                Description = so.Description,
                DiscountPct = so.DiscountPct
            };

        return await query.ToListAsync();
    }
}