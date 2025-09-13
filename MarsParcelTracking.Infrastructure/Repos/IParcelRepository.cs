using MarsParcelTracking.Domain.Entities;

namespace MarsParcelTracking.Infrastructure.Repos;

public interface IParcelRepository
{
    Parcel? Get(string barcode);
    void Add(Parcel parcel);
    void Update(Parcel parcel);

}
