namespace MarsParcelTracking.Domain.Entities;

public class Parcel
{
    public required string Barcode { get; set; }
    public string? Status { get; set; }
    public DateTime LaunchDate { get; set; }
    public int EtaDays { get; set; }
    public DateTime EstimatedArrivalDate { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required string DeliveryService { get; set; }
    public required string Contents { get; set; }    
    public List<History> History { get; set; } = [];
}

