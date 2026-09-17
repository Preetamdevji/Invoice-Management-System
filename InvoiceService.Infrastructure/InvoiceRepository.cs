using InvoiceService.Application.DTOs;
using InvoiceService.Application.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Infrastructure
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public InvoiceRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<InvoiceDto>> GetInvoicesAsync()
        {
            var invoices = new List<InvoiceDto>();

            const string sql = """
            WITH LatestOrders AS
            (
                SELECT TOP 10
                    SalesOrderID
                FROM Sales.SalesOrderHeader
                ORDER BY OrderDate DESC, SalesOrderID DESC
            )
            SELECT 
                c.CustomerID,
                soh.SalesOrderID,
                soh.SalesOrderNumber,
                soh.OrderDate,
                soh.TotalDue,
                sod.SalesOrderDetailID,
                sod.ProductID,
                sod.OrderQty,
                sod.UnitPrice,
                sod.UnitPriceDiscount,
                sod.LineTotal,
                p.Name AS ProductName,
                p.ProductNumber,
                p.Color,
                p.ListPrice
            FROM LatestOrders AS lo
            INNER JOIN Sales.SalesOrderHeader AS soh
                ON lo.SalesOrderID = soh.SalesOrderID
            INNER JOIN Sales.Customer AS c
                ON c.CustomerID = soh.CustomerID
            INNER JOIN Sales.SalesOrderDetail AS sod
                ON soh.SalesOrderID = sod.SalesOrderID
            INNER JOIN Sales.SpecialOfferProduct AS sop
                ON sod.ProductID = sop.ProductID
                AND sod.SpecialOfferID = sop.SpecialOfferID
            INNER JOIN Production.Product AS p
                ON sop.ProductID = p.ProductID
            ORDER BY
                soh.OrderDate DESC,
                soh.SalesOrderID DESC,
                sod.SalesOrderDetailID;
            """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var salesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID"));

                var invoice = invoices.FirstOrDefault(x => x.SalesOrderId == salesOrderId);

                if (invoice == null)
                {
                    invoice = new InvoiceDto
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        SalesOrderId = salesOrderId,
                        SalesOrderNumber = reader.GetString(reader.GetOrdinal("SalesOrderNumber")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        TotalDue = reader.GetDecimal(reader.GetOrdinal("TotalDue"))
                    };

                    invoices.Add(invoice);
                }

                invoice.Details.Add(new InvoiceDetailDto
                {
                    SalesOrderDetailId = reader.GetInt32(reader.GetOrdinal("SalesOrderDetailID")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    ProductNumber = reader.GetString(reader.GetOrdinal("ProductNumber")),
                    Color = reader.IsDBNull(reader.GetOrdinal("Color"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Color")),
                    OrderQty = reader.GetInt16(reader.GetOrdinal("OrderQty")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                    UnitPriceDiscount = reader.GetDecimal(reader.GetOrdinal("UnitPriceDiscount")),
                    LineTotal = reader.GetDecimal(reader.GetOrdinal("LineTotal")),
                    ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice"))
                });
            }

            return invoices;
        }

        public async Task<int> CreateInvoiceAsync(CreateInvoiceDto invoice)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                var subTotal = invoice.Details.Sum(detail =>
                    detail.UnitPrice * (1 - detail.UnitPriceDiscount) * detail.OrderQty);

                const string headerSql = """
                Insert Into Sales.SalesOrderHeader
                (
                DueDate,
                CustomerID,
                BillToAddressID,
                ShipToAddressID,
                ShipMethodID,
                SubTotal,
                TaxAmt,
                Freight,
                Comment
                )
                Output Inserted.SalesOrderID
                Values
                (
                @DueDate,
                @CustomerID,
                @BillToAddressID,
                @ShipToAddressID,
                @ShipMethodID,
                @SubTotal,
                0,
                0,
                @Comment
                );
                """;

                using var command = new SqlCommand(headerSql, connection, transaction);

                command.Parameters.AddWithValue("@DueDate", invoice.DueDate);
                command.Parameters.AddWithValue("@CustomerID", invoice.CustomerId);
                command.Parameters.AddWithValue("@BillToAddressID", invoice.BillToAddressId);
                command.Parameters.AddWithValue("@ShipToAddressID", invoice.ShipToAddressId);
                command.Parameters.AddWithValue("@ShipMethodID", invoice.ShipMethodId);
                command.Parameters.AddWithValue("@SubTotal", subTotal);
                command.Parameters.AddWithValue("@Comment", (object?)invoice.Comment ?? DBNull.Value);

                var result = await command.ExecuteScalarAsync();

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to create Sales Order Header.");
                }

                var salesOrderId = Convert.ToInt32(result);

                const string detailSql = """
                Insert Into Sales.SalesOrderDetail
                (
                SalesOrderID,
                OrderQty,
                ProductID,
                SpecialOfferID,
                UnitPrice,
                UnitPriceDiscount
                )
                Values
                (
                @SalesOrderID,
                @OrderQty,
                @ProductID,
                @SpecialOfferID,
                @UnitPrice,
                @UnitPriceDiscount
                );
                """;
               
                foreach (var detail in invoice.Details)
                {
                    using var detailCommand = new SqlCommand(detailSql, connection, transaction);

                    detailCommand.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                    detailCommand.Parameters.AddWithValue("@OrderQty", detail.OrderQty);
                    detailCommand.Parameters.AddWithValue("@ProductID", detail.ProductId);
                    detailCommand.Parameters.AddWithValue("@SpecialOfferID", detail.SpecialOfferId);
                    detailCommand.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                    detailCommand.Parameters.AddWithValue("@UnitPriceDiscount", detail.UnitPriceDiscount);

                    await detailCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                return salesOrderId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<AddressOptionDto>> GetAddressOptionsAsync()
        {
            var addresses = new List<AddressOptionDto>();

            const string sql = """
            SELECT
                AddressID,
                AddressLine1,
                City,
                PostalCode
            FROM Person.Address
            ORDER BY AddressID;
            """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var addressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                var city = reader.GetString(reader.GetOrdinal("City"));
                var postalCode = reader.GetString(reader.GetOrdinal("PostalCode"));

                addresses.Add(new AddressOptionDto
                {
                    AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                    DisplayName = $"{addressLine1}, {city}, {postalCode}"
                });
            }

            return addresses;
        }

        public async Task<List<LookupDto>> GetShipMethodOptionsAsync()
        {
            var shipMethods = new List<LookupDto>();

            const string sql = """
            SELECT
                ShipMethodID,
                Name
            FROM Purchasing.ShipMethod
            ORDER BY ShipMethodID;
            """;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                shipMethods.Add(new LookupDto
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ShipMethodID")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });
            }

            return shipMethods;
        }
    }
}
