using Microsoft.AspNetCore.Mvc;
using PoTracking.Abstractions;

namespace POTracking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_customerService.GetAll());
        }
    }
}
