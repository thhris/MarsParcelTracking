using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MarsParcelTracking.Controllers
{
    [ApiController]
    [Route("parcels")]
    public class ParcelTrackingController : ControllerBase
    {
        private readonly ILogger<ParcelTrackingController> _logger;
        private readonly IParcelService _parcelService;
        public ParcelTrackingController(ILogger<ParcelTrackingController> logger, IParcelService parcelService)
        {
            _logger = logger;
            _parcelService = parcelService;
        }

        [HttpPost]
        public IActionResult RegisterParcel([FromBody] Parcel request)
        {
            try
            {
                var result = _parcelService.CreateParcel(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{barcode}")]
        public IActionResult UpdateStatus(string barcode, [FromBody] ParcelStatus newStatus)
        {
            try
            {
                var result = _parcelService.UpdateParcelStatus(barcode, newStatus);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{barcode}")]
        public IActionResult GetParcel(string barcode)
        {
            var parcel = _parcelService.GetParcel(barcode);
            if (parcel == null)
            {
                return NotFound();
            }

            return Ok(parcel);
        }
    }
}
