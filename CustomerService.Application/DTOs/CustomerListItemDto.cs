using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.DTOs
{
    public class CustomerListItemDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
