using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string HealthCheck()
        {
            return "Mentor Booking System: " + DateTime.Now;
        }
    }
}
