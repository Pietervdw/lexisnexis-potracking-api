using Microsoft.AspNetCore.Mvc;
using PoTracking.Abstractions;
using POTracking.Domain;
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

        [HttpPatch("{number}/status")]
        public IActionResult ChangeStatus(string number, [FromBody] PurchaseOrderStatusUpdateRequest request)
        {
            var response = _purchaseOrderService.ChangeStatus(number, request.Status);
            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var response = _purchaseOrderService.GetAll();
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }

        [HttpGet("{number}")]
        public IActionResult Get(string number)
        {
            var response = _purchaseOrderService.GetByNumber(number);
            if (response.Success)
                return Ok(response);

            return NotFound(response);
        }
    }
}