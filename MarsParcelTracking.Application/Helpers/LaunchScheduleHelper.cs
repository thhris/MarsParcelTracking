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
        var today = DateOnly.FromDateTime(now.Date);
        var firstWednesday = GetFirstWednesdayOfMonth(today.Year, today.Month);

        if (firstWednesday < today)
        {
            var nextMonth = today.AddMonths(1);
            return GetFirstWednesdayOfMonth(nextMonth.Year, nextMonth.Month);
        }

        return firstWednesday;
    }

    static DateOnly GetFirstWednesdayOfMonth(int year, int month)
    {
        var date = new DateOnly(year, month, 1);
        while (date.DayOfWeek != DayOfWeek.Wednesday)
            date = date.AddDays(1);
        return date;
    }
}
