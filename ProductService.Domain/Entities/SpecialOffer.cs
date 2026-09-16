using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Domain.Entities
{
    public class SpecialOffer
    {
        public int SpecialOfferId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPct { get; set; }
    }
}
