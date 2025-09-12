using Microsoft.AspNetCore.Mvc;

namespace MarsParcelTracking.Controllers
{
    [ApiController]
    [Route("parcels")]
    public class ParcelTrackingController : ControllerBase
    {
        private readonly ILogger<ParcelTrackingController> _logger;

        public ParcelTrackingController(ILogger<ParcelTrackingController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetParcel")]
        public IActionResult GetParcel()
        {
            return Ok();
        }
    }
}
