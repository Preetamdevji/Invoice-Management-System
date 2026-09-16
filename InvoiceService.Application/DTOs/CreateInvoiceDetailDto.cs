using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.DTOs
{
    public class CreateInvoiceDetailDto
    {
        public int ProductId { get; set; }
        public int SpecialOfferId { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
    }
}
