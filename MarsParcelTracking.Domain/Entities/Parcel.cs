using MarsParcelTracking.Domain.Enums;

namespace MarsParcelTracking.Domain.Entities;

public class Parcel
{
    public required string Barcode { get; set; }
    public ParcelStatus Status { get; set; }
    public DateOnly LaunchDate { get; set; }
    public int EtaDays { get; set; }
    public DateOnly EstimatedArrivalDate { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required string DeliveryService { get; set; }
    public required string Contents { get; set; }    
    public List<History> History { get; set; } = [];
}

