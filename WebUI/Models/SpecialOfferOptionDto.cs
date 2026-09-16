namespace WebUI.Models
{
    public class SpecialOfferOptionDto
    {
        public int SpecialOfferId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPct { get; set; }
    }
}
