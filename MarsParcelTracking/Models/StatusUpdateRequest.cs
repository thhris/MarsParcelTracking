using MarsParcelTracking.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MarsParcelTracking.API.Models;

public class StatusUpdateRequest
{
    [Required]
    public ParcelStatus NewStatus { get; set; }
}
