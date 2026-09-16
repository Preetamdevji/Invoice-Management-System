using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Application.DTOs
{
    public class AddressOptionDto
    {
        public int AddressId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
