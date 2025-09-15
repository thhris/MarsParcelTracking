using MarsParcelTracking.Application.Helpers;
using MarsParcelTracking.Domain.Enums;
using FluentAssertions;

namespace MarsParcelTracking.Tests;

[TestFixture]
public class StatusTransitionValidatorTests
{
    [TestCase(ParcelStatus.Created, ParcelStatus.OnRocketToMars, true)]
    [TestCase(ParcelStatus.Created, ParcelStatus.LandedOnMars, false)]
    [TestCase(ParcelStatus.OnRocketToMars, ParcelStatus.LandedOnMars, true)]
    [TestCase(ParcelStatus.OnRocketToMars, ParcelStatus.Lost, true)]
    [TestCase(ParcelStatus.OnRocketToMars, ParcelStatus.Delivered, false)]
    [TestCase(ParcelStatus.LandedOnMars, ParcelStatus.OutForMartianDelivery, true)]
    [TestCase(ParcelStatus.LandedOnMars, ParcelStatus.Delivered, false)]
    [TestCase(ParcelStatus.OutForMartianDelivery, ParcelStatus.Delivered, true)]
    [TestCase(ParcelStatus.OutForMartianDelivery, ParcelStatus.Lost, true)]
    public void IsValidTransition_ShouldReturnExpected(ParcelStatus current, ParcelStatus next, bool expected)
    {
        // Assert
        StatusTransitionValidator.IsValidTransition(current, next).Should().Be(expected);
    }
}