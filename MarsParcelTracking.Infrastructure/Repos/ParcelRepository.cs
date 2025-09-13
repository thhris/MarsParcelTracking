using MarsParcelTracking.Domain.Entities;

namespace MarsParcelTracking.Infrastructure.Repos;

public class ParcelRepository : IParcelRepository
{
    private readonly Dictionary<string, Parcel> _storage = [];

    public Parcel? Get(string barcode)
    {
        return _storage.TryGetValue(barcode, out var parcel) ? parcel : null;
    }

    public void Add(Parcel parcel)
    {
        _storage[parcel.Barcode] = parcel;
    }

    public void Update(Parcel parcel)
    {
        _storage[parcel.Barcode] = parcel;
    }
}
