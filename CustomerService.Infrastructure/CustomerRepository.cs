using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerListItemDto>> GetAllAsync()
    {
        var query =
            from c in _context.Customers
            join p in _context.People
                on c.PersonId!.Value equals p.BusinessEntityId
            orderby c.CustomerId
            select new CustomerListItemDto
            {
                CustomerId = c.CustomerId,
                FullName = p.FirstName + " " + p.LastName
            };

        return await query.ToListAsync();
    }

    public async Task<List<CustomerOptionDto>> GetCustomerOptionsAsync()
    {
        return await _context.Customers
            .Where(c => c.PersonId.HasValue)
            .OrderBy(c => c.CustomerId)
            .Select(c => new CustomerOptionDto
            {
                CustomerId = c.CustomerId
            })
            .ToListAsync();
    }
}