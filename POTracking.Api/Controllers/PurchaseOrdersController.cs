using Microsoft.AspNetCore.Mvc;
using PoTracking.Abstractions;
using POTracking.Domain.Dtos;

namespace POTracking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [HttpPost]
        public IActionResult Post(PurchaseOrderCreateRequest request)
        {
            var response = _purchaseOrderService.Create(request);
            if (response.Success)
                return Created($"/purchaseorders/{response.PurchaseOrder?.Number}", response);

            return BadRequest(response);
        }
    }
}