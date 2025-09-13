using MarsParcelTracking.Application.Helpers;
using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Domain.Enums;
using MarsParcelTracking.Infrastructure.Repos;

namespace MarsParcelTracking.Application.Services;

public class ParcelService : IParcelService
{
    private readonly IParcelRepository _repository;
    private static readonly DateOnly FirstStandardLaunch = new DateOnly(2025, 10, 1);
    public ParcelService(IParcelRepository repository)
    {
        _repository = repository;
    }

    public Parcel? GetParcel(string barcode)
    {
        if (!BarcodeValidator.IsValid(barcode))
            throw new ArgumentException("Invalid barcode format.");

        Parcel? parcel = _repository.Get(barcode);

        return parcel;
    }

    public Parcel CreateParcel(Parcel parcel)
    {
        // Validate Parcel barcode
        if (!BarcodeValidator.IsValid(parcel.Barcode))
            throw new ArgumentException("Invalid barcode format.");

        // Set hardcoded values
        parcel.Status = ParcelStatus.Created;
        parcel.Origin = "Starport Thames Estuary";
        parcel.Destination = "New London";

        parcel.LaunchDate = GetLaunchDate(parcel.DeliveryService, DateTime.UtcNow);
        parcel.EtaDays = GetEtaDays(parcel.DeliveryService);
        parcel.EstimatedArrivalDate = GetEstimatedArrivalTime(parcel.LaunchDate, parcel.EtaDays);
        parcel.History.Add(new History { Status = parcel.Status, Timestamp = DateTime.Now });

        // Save new Parcel and return values
        _repository.Add(parcel);

        return parcel;
    }

    public Parcel UpdateParcelStatus(string barcode, ParcelStatus newStatus)
    {
        // Validate Parcel barcode
        if (!BarcodeValidator.IsValid(barcode))
            throw new ArgumentException("Invalid barcode format.");

        var parcel = _repository.Get(barcode);
        if (parcel == null)
        {
            throw new KeyNotFoundException("Parcel not found.");
        }

        if (!StatusTransitionValidator.IsValidTransition(parcel.Status, newStatus))
        {
            throw new ArgumentException("Invalid transition");
        }

        parcel.Status = newStatus;
        parcel.History.Add(new History { Status = newStatus, Timestamp = DateTime.Now });
        _repository.Update(parcel);

        return parcel;
    }

    public static DateOnly GetLaunchDate(string service, DateTime now)
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

    private static DateOnly GetEstimatedArrivalTime(DateOnly launchDate, int etaDays)
    {
        return launchDate.AddDays(etaDays);
    }

    private static DateOnly GetNextStandardLaunchDate()
    {
        var nextLaunchDate = FirstStandardLaunch;
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        while (nextLaunchDate < today)
            nextLaunchDate = nextLaunchDate.AddMonths(26);

        return nextLaunchDate;
    }

    private static DateOnly GetNextExpressLaunchDate(DateTime now)
    {
        var firstWednesday = new DateOnly(now.Year, now.Month, 1);
        while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
            firstWednesday = firstWednesday.AddDays(1);

        return firstWednesday;
    }

}
