using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Infrastructure.Repos;

namespace MarsParcelTracking.Application.Services;

public class ParcelService : IParcelService
{
    private readonly IParcelRepository _repository;
    private static readonly DateTime FirstStandardLaunch = new DateTime(2025, 10, 1);
    public ParcelService(IParcelRepository repository)
    {
        _repository = repository;
    }

    public Parcel GetParcel(string barcode)
    {
        // Retrieve and return Parcel by barcode

        throw new NotImplementedException();
    }

    public Parcel CreateParcel(Parcel parcel)
    {
        // Validate Parcel barcode

        // Set hardcoded values
        parcel.Status = "Created";
        parcel.Origin = "Starport Thames Estuary";
        parcel.Destination = "New London";

        parcel.LaunchDate = GetLaunchDate(parcel.DeliveryService, DateTime.UtcNow);
        parcel.EtaDays = GetEtaDays(parcel.DeliveryService);
        parcel.EstimatedArrivalDate = GetEstimatedArrivalTime(parcel.LaunchDate, parcel.EtaDays);

        // Save new Parcel and return values
        _repository.Add(parcel);

        return parcel;
    }

    public Parcel UpdateParcelStatus(string barcode, string newStatus)
    {
        // Validate Parcel barcode

        // Check if new status is possible from current status

        // Update Parcel status

        throw new NotImplementedException();
    }

    public static DateTime GetLaunchDate(string service, DateTime now)
    {
        return service switch
        {
            "Standard" => GetNextStandardLaunchDate(),
            "Express" => GetNextExpressLaunchDate(now),
            _ => throw new ArgumentException("Invalid delivery service")
        };
    }

    public static int GetEtaDays(string service)
    {
        return service switch
        {
            "Standard" => 180,
            "Express" => 90,
            _ => throw new ArgumentException("Invalid delivery service")
        };
    }

    private static DateTime GetEstimatedArrivalTime(DateTime launchDate, int etaDays)
    {
        return launchDate.AddDays(etaDays);
    }

    private static DateTime GetNextStandardLaunchDate()
    {
        var nextLaunchDate = FirstStandardLaunch;
        while (nextLaunchDate < DateTime.UtcNow)
            nextLaunchDate = nextLaunchDate.AddMonths(26);

        return nextLaunchDate;
    }

    private static DateTime GetNextExpressLaunchDate(DateTime now)
    {
        var firstWednesday = new DateTime(now.Year, now.Month, 1);
        while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
            firstWednesday = firstWednesday.AddDays(1);

        return firstWednesday;
    }

}
