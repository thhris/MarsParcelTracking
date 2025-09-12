using MarsParcelTracking.Application.Interfaces;
using MarsParcelTracking.Domain.Entities;

namespace MarsParcelTracking.Application.Services;

public class ParcelService : IParcelService
{
    public Parcel GetParcel(string barcode)
    {
        // Retrieve and return Parcel by barcode

        throw new NotImplementedException();
    }

    public Parcel CreateParcel(Parcel parcel)
    {
        // Validate Parcel barcode

        // Set initial status

        // Set Origin and Destination to hardcoded values

        // Compute and store LaunchDate, EtaDays, and EstimatedArrivalDate

        // Save new Parcel and return values

        // NOTE: Use Earth UTC time for all date calculations

        throw new NotImplementedException();
    }

    public Parcel UpdateParcelStatus(string barcode, string newStatus)
    {
        // Validate Parcel barcode

        // Check if new status is possible from current status

        // Update Parcel status

        throw new NotImplementedException();
    }
}
