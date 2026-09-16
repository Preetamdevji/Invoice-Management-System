using Microsoft.Data.SqlClient;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ProductRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<ProductOptionDto>> GetProductOptionsAsync()
        {
            var products = new List<ProductOptionDto>();

            const string sql = """
                SELECT
                ProductID,
                Name,
                ProductNumber,
                ListPrice
                FROM Production.Product
                WHERE ListPrice > 0
                ORDER BY Name;
                """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new ProductOptionDto
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader.GetString(reader.GetOrdinal("Name")),
                    ProductNumber = reader.GetString(reader.GetOrdinal("ProductNumber")),
                    ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice"))
                });
               
            }

            return products;

        }

        public async Task<List<SpecialOfferOptionDto>> GetSpecialOfferOptionsAsync(int productId)
        {
            var offers = new List<SpecialOfferOptionDto>();

            const string sql = """
            SELECT
                so.SpecialOfferID,
                so.Description,
                so.DiscountPct
            FROM Sales.SpecialOfferProduct sop
            INNER JOIN Sales.SpecialOffer so
                ON sop.SpecialOfferID = so.SpecialOfferID
            WHERE sop.ProductID = @ProductID
            ORDER BY so.SpecialOfferID;
            """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductID", productId);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                offers.Add(new SpecialOfferOptionDto
                {
                    SpecialOfferId = reader.GetInt32(reader.GetOrdinal("SpecialOfferID")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    DiscountPct = reader.GetDecimal(reader.GetOrdinal("DiscountPct"))
                });
            }

            return offers;
        }
    }
}
