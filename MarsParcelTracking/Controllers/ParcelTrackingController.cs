using MarsParcelTracking.API.Models;
using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Domain.Entities;
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating parcel");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("{barcode}")]
        public IActionResult UpdateStatus(string barcode, [FromBody] StatusUpdateRequest request)
        {
            try
            {
                var result = _parcelService.UpdateParcelStatus(barcode, request.NewStatus);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Parcel not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error updating parcel status for {barcode}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{barcode}")]
        public IActionResult GetParcel(string barcode)
        {
            try
            {
                var parcel = _parcelService.GetParcel(barcode);
                if (parcel == null)
                {
                    return NotFound();
                }

                return Ok(parcel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error retrieving parcel {barcode}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
