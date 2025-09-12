using MarsParcelTracking.Domain.Entities;

namespace MarsParcelTracking.Application.Interfaces;

public interface IParcelService
{
    Parcel GetParcel(string barcode);
    Parcel CreateParcel(Parcel parcel);
    Parcel UpdateParcelStatus(string barcode, string newStatus);
}
