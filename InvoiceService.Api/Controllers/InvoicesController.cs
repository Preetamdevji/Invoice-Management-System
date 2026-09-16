using InvoiceService.Application.DTOs;
using InvoiceService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerServiceClient _customerServiceClient;
        private readonly IProductServiceClient _productServiceClient;
        public InvoicesController
            (
            IInvoiceRepository invoiceRepository,
            ICustomerServiceClient customerServiceClient,
            IProductServiceClient productServiceClient
            )
            
        {
            _invoiceRepository = invoiceRepository;
            _customerServiceClient = customerServiceClient;
            _productServiceClient = productServiceClient;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _invoiceRepository.GetInvoicesAsync();
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto invoice)
        {
            var salesOrderId = await _invoiceRepository.CreateInvoiceAsync(invoice);
            return Ok(new { SalesOrderId = salesOrderId });
        }

        [HttpGet("addresses")]
        public async Task<IActionResult> GetAddressOptions()
        {
            var addresses = await _invoiceRepository.GetAddressOptionsAsync();
            return Ok(addresses);
        }

        [HttpGet("ship-methods")]
        public async Task<IActionResult> GetShipMethodOptions()
        {
            var shipMethods = await _invoiceRepository.GetShipMethodOptionsAsync();
            return Ok(shipMethods);
        }

        [HttpGet("create-data")]
        public async Task<IActionResult> GetCreateInvoiceData()
        {
            var customers = await _customerServiceClient.GetCustomerOptionsAsync();
            var products = await _productServiceClient.GetProductOptionsAsync();
            var addresses = await _invoiceRepository.GetAddressOptionsAsync();
            var shipMethods = await _invoiceRepository.GetShipMethodOptionsAsync();

            return Ok(new
            {
                Customers = customers,
                Products = products,
                Addresses = addresses,
                ShipMethods = shipMethods
            });        
        }

        [HttpGet("products/{productId}/special-offers")]
        public async Task<IActionResult> GetProductSpecialOffers(int productId)
        {
            var offers = await _productServiceClient.GetSpecialOfferOptionsAsync(productId);
            return Ok(offers);
        }
    }
}
