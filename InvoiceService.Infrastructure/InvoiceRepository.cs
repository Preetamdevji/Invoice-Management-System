using InvoiceService.Application.DTOs;
using InvoiceService.Application.Interfaces;
using InvoiceService.Domain.Entities;
using InvoiceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceService.Infrastructure;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly InvoiceDbContext _context;

    public InvoiceRepository(InvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<List<InvoiceDto>> GetInvoicesAsync()
    {
        var latestInvoiceIds = await _context.Invoices
            .OrderByDescending(i => i.OrderDate)
            .ThenByDescending(i => i.SalesOrderId)
            .Select(i => i.SalesOrderId)
            .Take(10)
            .ToListAsync();

        var query =
            from i in _context.Invoices
            where latestInvoiceIds.Contains(i.SalesOrderId)
            join c in _context.Customers on i.CustomerId equals c.CustomerId
            join p in _context.People on c.PersonId!.Value equals p.BusinessEntityId
            orderby i.OrderDate descending, i.SalesOrderId descending
            select new InvoiceDto
            {
                SalesOrderId = i.SalesOrderId,
                SalesOrderNumber = i.SalesOrderNumber,
                OrderDate = i.OrderDate,
                CustomerId = i.CustomerId,
                CustomerName = p.FirstName + " " + p.LastName,
                TotalDue = i.TotalDue,
                Details = i.Details.Select(d => new InvoiceDetailDto
                {
                    SalesOrderDetailId = d.SalesOrderDetailId,
                    ProductId = d.ProductId,
                    OrderQty = d.OrderQty,
                    UnitPrice = d.UnitPrice,
                    UnitPriceDiscount = d.UnitPriceDiscount,
                    LineTotal = d.LineTotal,
                    ProductName = _context.Products
                        .Where(pr => pr.ProductId == d.ProductId)
                        .Select(pr => pr.Name)
                        .FirstOrDefault() ?? string.Empty,
                    ProductNumber = _context.Products
                        .Where(pr => pr.ProductId == d.ProductId)
                        .Select(pr => pr.ProductNumber)
                        .FirstOrDefault() ?? string.Empty,
                    Color = _context.Products
                        .Where(pr => pr.ProductId == d.ProductId)
                        .Select(pr => pr.Color)
                        .FirstOrDefault(),
                    ListPrice = _context.Products
                        .Where(pr => pr.ProductId == d.ProductId)
                        .Select(pr => pr.ListPrice)
                        .FirstOrDefault()
                }).ToList()
            };

        return await query.ToListAsync();
    }

    public async Task<int> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        var subTotal = dto.Details.Sum(d => d.UnitPrice * (1 - d.UnitPriceDiscount) * d.OrderQty);

        var invoice = new Invoice
        {
            CustomerId = dto.CustomerId,
            OrderDate = DateTime.Now,
            DueDate = dto.DueDate,
            BillToAddressId = dto.BillToAddressId,
            ShipToAddressId = dto.ShipToAddressId,
            ShipMethodId = dto.ShipMethodId,
            SubTotal = subTotal,
            TaxAmt = 0,
            Freight = 0,
            Comment = dto.Comment,
            Details = dto.Details.Select(d => new InvoiceDetail
            {
                ProductId = d.ProductId,
                SpecialOfferId = d.SpecialOfferId,
                OrderQty = d.OrderQty,
                UnitPrice = d.UnitPrice,
                UnitPriceDiscount = d.UnitPriceDiscount
            }).ToList()
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return invoice.SalesOrderId;
    }

    public async Task<List<AddressOptionDto>> GetAddressOptionsAsync()
    {
        return await _context.Addresses
            .OrderBy(a => a.AddressId)
            .Select(a => new AddressOptionDto
            {
                AddressId = a.AddressId,
                DisplayName = a.AddressLine1 + ", " + a.City + ", " + a.PostalCode
            })
            .ToListAsync();
    }

    public async Task<List<LookupDto>> GetShipMethodOptionsAsync()
    {
        return await _context.ShipMethods
            .OrderBy(sm => sm.ShipMethodId)
            .Select(sm => new LookupDto
            {
                Id = sm.ShipMethodId,
                Name = sm.Name
            })
            .ToListAsync();
    }
}