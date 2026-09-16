using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Application.DTOs
{
    public class SpecialOfferOptionDto
    {
        public int SpecialOfferId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPct { get; set; }
    }
}
