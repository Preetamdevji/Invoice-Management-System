using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Domain.Entities
{
    internal class Invoice
    {
        public int SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public int CustomerId { get; set; }
        public int BillToAddressId { get; set; }
        public int ShipToAddressId { get; set; }
        public int ShipMethodId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalDue { get; set; }
        public string? Comment { get; set; }

        public List<InvoiceDetail> Details { get; set; } = new();
    }
}
