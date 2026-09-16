using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.DTOs
{
    public class InvoiceDetailDto
    {
        public int SalesOrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public string? Color { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal ListPrice { get; set; }
    }
}
