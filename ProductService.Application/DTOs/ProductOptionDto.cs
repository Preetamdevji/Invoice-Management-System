using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Application.DTOs
{
    public class ProductOptionDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }
    }
}
