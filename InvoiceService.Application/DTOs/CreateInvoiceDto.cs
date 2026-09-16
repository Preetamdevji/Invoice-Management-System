using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.DTOs
{
    public class CreateInvoiceDto
    {
        public int CustomerId { get; set; }
        public DateTime DueDate { get; set; }
        public int BillToAddressId { get; set; }
        public int ShipToAddressId { get; set; }
        public int ShipMethodId { get; set; }
        public string? Comment { get; set; }

        public List<CreateInvoiceDetailDto> Details { get; set; } = new();
    }
}
