using MarsParcelTracking.Domain.Enums;

namespace MarsParcelTracking.Domain.Entities;
public class History
{
    public ParcelStatus Status { get; set; }
    public DateTime Timestamp { get; set; }
}