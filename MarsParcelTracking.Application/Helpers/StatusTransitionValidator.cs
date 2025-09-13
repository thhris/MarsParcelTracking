using MarsParcelTracking.Domain.Enums;

namespace MarsParcelTracking.Application.Helpers;

public class StatusTransitionValidator
{
    private static readonly IReadOnlyDictionary<ParcelStatus, ParcelStatus[]> ValidTransitions =
        new Dictionary<ParcelStatus, ParcelStatus[]>
        {
            { ParcelStatus.Created, new[] { ParcelStatus.OnRocketToMars } },
            { ParcelStatus.OnRocketToMars, new[] { ParcelStatus.LandedOnMars, ParcelStatus.Lost } },
            { ParcelStatus.LandedOnMars, new[] { ParcelStatus.OutForMartianDelivery } },
            { ParcelStatus.OutForMartianDelivery, new[] { ParcelStatus.Delivered, ParcelStatus.Lost } }
        };

    public static bool IsValidTransition(ParcelStatus current, ParcelStatus next)
    {
        return ValidTransitions.TryGetValue(current, out var allowed) &&
               allowed.Contains(next);
    }
}
