using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.DTOs
{
    public class AddressOptionDto
    {
        public int AddressId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
