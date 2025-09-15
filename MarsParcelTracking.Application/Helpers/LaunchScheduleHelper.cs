using MarsParcelTracking.Domain.Enums;

namespace MarsParcelTracking.Application.Helpers;

public static class LaunchScheduleHelper
{
    private static readonly DateOnly FirstStandardLaunch = new DateOnly(2025, 10, 1);
    private const int NextLaunchInterval = 26;

    public static DateOnly GetLaunchDate(DeliveryServiceType service, DateTime now)
    {
        return service switch
        {
            DeliveryServiceType.Standard => GetNextStandardLaunchDate(),
            DeliveryServiceType.Express => GetNextExpressLaunchDate(now),
            _ => throw new ArgumentException("Invalid delivery service")
        };
    }

    public static int GetEtaDays(DeliveryServiceType service)
    {
        return service switch
        {
            DeliveryServiceType.Standard => 180,
            DeliveryServiceType.Express => 90,
            _ => throw new ArgumentException("Invalid delivery service")
        };
    }

    public static DateOnly GetEstimatedArrivalTime(DateOnly launchDate, int etaDays)
    {
        return launchDate.AddDays(etaDays);
    }

    private static DateOnly GetNextStandardLaunchDate()
    {
        var nextLaunchDate = FirstStandardLaunch;
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        while (nextLaunchDate < today)
            nextLaunchDate = nextLaunchDate.AddMonths(NextLaunchInterval);

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
