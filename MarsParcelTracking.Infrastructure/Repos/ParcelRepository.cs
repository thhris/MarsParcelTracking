using MarsParcelTracking.Domain.Entities;
using System.Collections.Concurrent;
using System.Data;

namespace MarsParcelTracking.Infrastructure.Repos;

public class ParcelRepository : IParcelRepository
{
    private readonly ConcurrentDictionary<string, Parcel> _storage = new();

    public Parcel? Get(string barcode)
    {
        return _storage.TryGetValue(barcode, out var parcel) ? parcel : null;
    }

    public void Add(Parcel parcel)
    {
        if (!_storage.TryAdd(parcel.Barcode, parcel))
            throw new DuplicateNameException(parcel.Barcode);
    }

    public void Update(Parcel parcel)
    {
        _storage[parcel.Barcode] = parcel;
    }
}
