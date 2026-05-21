using Microsoft.AspNetCore.Mvc;

namespace Idempotency.Net.AspNetCore.IntegrationTests.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    [Idempotent]
    public IActionResult CreateOrder()
    {
        return Ok(Guid.NewGuid().ToString());
    }
}