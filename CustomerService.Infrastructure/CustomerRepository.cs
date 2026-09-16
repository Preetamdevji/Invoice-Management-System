using CustomerService.Application.Interfaces;
using CustomerService.Application.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public CustomerRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CustomerListItemDto>> GetAllAsync()
        {
            var customers = new List<CustomerListItemDto>();

            const string sql = """
            SELECT
                Top 10
                c.CustomerID,
                p.FirstName,
                p.LastName
            FROM Sales.Customer c
            INNER JOIN Person.Person p
                ON c.PersonID = p.BusinessEntityID
            ORDER BY c.CustomerID;
            """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                var lastName = reader.GetString(reader.GetOrdinal("LastName"));

                customers.Add(new CustomerListItemDto
                {
                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                    FullName = $"{firstName} {lastName}"
                });
            }

            return customers;
        }


        public async Task<List<CustomerOptionDto>> GetCustomerOptionsAsync()
        {
            var customers = new List<CustomerOptionDto>();

            const string sql = """
            SELECT
                CustomerID
            FROM Sales.Customer
            WHERE PersonID IS NOT NULL
            ORDER BY CustomerID;
            """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(new CustomerOptionDto
                {
                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID"))
                });
            }

            return customers;
        }
    }
}
