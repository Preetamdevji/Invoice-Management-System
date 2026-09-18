using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure;
using CustomerService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddScoped<SqlConnectionFactory>(sp =>
//    new SqlConnectionFactory(
//        builder.Configuration.GetConnectionString("AdventureWorks")!));

var connectionString = builder.Configuration.GetConnectionString("AdventureWorks");
builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
