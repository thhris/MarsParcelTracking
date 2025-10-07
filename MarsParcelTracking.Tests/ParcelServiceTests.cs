using FluentAssertions;
using MarsParcelTracking.Application.Helpers;
using MarsParcelTracking.Application.Services;
using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Domain.Enums;
using MarsParcelTracking.Infrastructure.Repos;

namespace MarsParcelTracking.Tests;

[TestFixture]
public class ParcelServiceTests
{
    private IParcelRepository _repository = null!;
    private ParcelService _service = null!;
    private const string ValidBarcode = "RMARS1234567890123456789M";

    [SetUp]
    public void SetUp()
    {
        _repository = new ParcelRepository();
        _service = new ParcelService(_repository);
    }

    [Test]
    public void CreateParcel_ShouldInitializeDerivedFields_AndPersist()
    {
        var parcel = NewParcel();
        var created = _service.CreateParcel(parcel);

        created.Status.Should().Be(ParcelStatus.Created);
        created.History.Should().HaveCount(1);
        created.EtaDays.Should().BeGreaterThan(0);
        created.EstimatedArrivalDate.Should().Be(created.LaunchDate.AddDays(created.EtaDays));

        _repository.Get(ValidBarcode).Should().NotBeNull();
    }

    [Test]
    public void CreateParcel_ShouldThrow_ForInvalidBarcode()
    {
        var parcel = NewParcel(barcode: "BAD");
        var act = () => _service.CreateParcel(parcel);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid barcode*");
    }

    [Test]
    public void GetParcel_ShouldReturnParcel_WhenExists()
    {
        var parcel = _service.CreateParcel(NewParcel());
        var fetched = _service.GetParcel(parcel.Barcode);
        fetched.Should().NotBeNull();
        fetched!.Barcode.Should().Be(parcel.Barcode);
    }

    [Test]
    public void GetParcel_ShouldThrow_ForInvalidBarcode()
    {
        var act = () => _service.GetParcel("X");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void UpdateParcelStatus_ShouldAdvanceStatus_AndAppendHistory()
    {
        _service.CreateParcel(NewParcel());

        var updated1 = _service.UpdateParcelStatus(ValidBarcode, ParcelStatus.OnRocketToMars);
        updated1.Status.Should().Be(ParcelStatus.OnRocketToMars);
        updated1.History.Should().HaveCount(2);

        var updated2 = _service.UpdateParcelStatus(ValidBarcode, ParcelStatus.LandedOnMars);
        updated2.Status.Should().Be(ParcelStatus.LandedOnMars);
        updated2.History.Should().HaveCount(3);
    }

    [Test]
    public void UpdateParcelStatus_ShouldThrow_ForInvalidTransition()
    {
        _service.CreateParcel(NewParcel());
        var act = () => _service.UpdateParcelStatus(ValidBarcode, ParcelStatus.Delivered);
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Invalid transition*");
    }

    [Test]
    public void UpdateParcelStatus_ShouldThrow_WhenParcelMissing()
    {
        var act = () => _service.UpdateParcelStatus(ValidBarcode, ParcelStatus.OnRocketToMars);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public void GetLaunchDate_Express_ShouldReturnFirstWednesday()
    {
        var date = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        var launch = LaunchScheduleHelper.GetLaunchDate(DeliveryServiceType.Express, date);
        launch.Year.Should().Be(2025);
        launch.Month.Should().Be(2);
        launch.Day.Should().Be(5);
    }

    [Test]
    public void GetLaunchDate_Standard_ShouldReturnFutureLaunchDate()
    {
        var now = DateTime.UtcNow;
        var launch = LaunchScheduleHelper.GetLaunchDate(DeliveryServiceType.Standard, now);
        launch.ToDateTime(TimeOnly.MinValue).Date
              .Should().BeOnOrAfter(DateTime.UtcNow.Date);
        launch.Day.Should().Be(1);
    }

    [Test]
    public void GetLaunchDate_ShouldThrow_ForInvalidService()
    {
        var invalid = (DeliveryServiceType)999;
        var act = () => LaunchScheduleHelper.GetLaunchDate(invalid, DateTime.UtcNow);
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Invalid delivery service*");
    }

    [Test]
    public void GetEtaDays_ShouldReturnCorrectValues()
    {
        LaunchScheduleHelper.GetEtaDays(DeliveryServiceType.Standard).Should().Be(180);
        LaunchScheduleHelper.GetEtaDays(DeliveryServiceType.Express).Should().Be(90);
    }

    [Test]
    public void GetEtaDays_ShouldThrow_ForInvalidService()
    {
        var invalid = (DeliveryServiceType)999;
        var act = () => LaunchScheduleHelper.GetEtaDays(invalid);
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Invalid delivery service*");
    }

    private Parcel NewParcel(string barcode = ValidBarcode) => new Parcel
    {
        Barcode = barcode,
        Sender = "Alice",
        Recipient = "Bob",
        DeliveryService = DeliveryServiceType.Standard,
        Contents = "Sample contents",
        History = new List<History>()
    };
}