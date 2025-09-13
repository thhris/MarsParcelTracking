using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Domain.Enums;

namespace MarsParcelTracking.Application.Interfaces;

public interface IParcelService
{
    Parcel? GetParcel(string barcode);
    Parcel CreateParcel(Parcel parcel);
    Parcel UpdateParcelStatus(string barcode, ParcelStatus newStatus);
}
