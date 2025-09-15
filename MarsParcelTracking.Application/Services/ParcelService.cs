using MarsParcelTracking.Application.Helpers;
using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Domain.Enums;
using MarsParcelTracking.Infrastructure.Repos;

namespace MarsParcelTracking.Application.Services;

public class ParcelService : IParcelService
{
    private readonly IParcelRepository _repository;
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

        parcel.LaunchDate = LaunchScheduleHelper.GetLaunchDate(parcel.DeliveryService, DateTime.UtcNow);
        parcel.EtaDays = LaunchScheduleHelper.GetEtaDays(parcel.DeliveryService);
        parcel.EstimatedArrivalDate = LaunchScheduleHelper.GetEstimatedArrivalTime(parcel.LaunchDate, parcel.EtaDays);
        parcel.History.Add(new History { Status = parcel.Status, Timestamp = DateTime.UtcNow });

        // Save new Parcel and return values
        _repository.Add(parcel);

        return parcel;
    }

    public Parcel UpdateParcelStatus(string barcode, ParcelStatus newStatus)
    {
        if (!BarcodeValidator.IsValid(barcode))
            throw new ArgumentException("Invalid barcode format.");

        var parcel = _repository.Get(barcode) ?? throw new KeyNotFoundException("Parcel not found.");

        if (!StatusTransitionValidator.IsValidTransition(parcel.Status, newStatus))
        {
            throw new ArgumentException("Invalid transition");
        }

        parcel.Status = newStatus;
        parcel.History.Add(new History { Status = newStatus, Timestamp = DateTime.UtcNow });
        _repository.Update(parcel);

        return parcel;
    }
}
