using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Interfaces;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetOptions()
        {
            var options = await _productRepository.GetProductOptionsAsync();
            return Ok(options);
        }

        [HttpGet("{productId}/special-offers")]
        public async Task<IActionResult> GetSpecialOffers(int productId)
        {
            var offers = await _productRepository.GetSpecialOfferOptionsAsync(productId);
            return Ok(offers);
        }
    }
}
