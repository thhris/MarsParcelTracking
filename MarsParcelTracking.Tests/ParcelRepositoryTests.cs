using FluentAssertions;
using System.Data;
using MarsParcelTracking.Domain.Entities;
using MarsParcelTracking.Domain.Enums;
using MarsParcelTracking.Infrastructure.Repos;

namespace MarsParcelTracking.Tests;

[TestFixture]
public class ParcelRepositoryTests
{
    private ParcelRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new ParcelRepository();
    }

    [Test]
    public void Add_ShouldStoreParcel()
    {
        // Arrange
        var parcel = CreateParcel("RMARS1234567890123456789M");

        // Act
        _repository.Add(parcel);
        var result = _repository.Get(parcel.Barcode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(parcel);
    }

    [Test]
    public void Add_ShouldThrow_WhenDuplicateBarcode()
    {
        // Arrange
        var barcode = "RMARS1234567890123456789M";
        _repository.Add(CreateParcel(barcode));

        // Act
        var act = () => _repository.Add(CreateParcel(barcode));

        // Assert
        act.Should().Throw<DuplicateNameException>();
    }

    [Test]
    public void Get_ShouldReturnNull_WhenBarcodeNotFound()
    {
        // Arrange
        var missingBarcode = "RMARS1234567890123456789M";

        // Act
        var result = _repository.Get(missingBarcode);

        // Assert
        result.Should().BeNull();
    }


    private Parcel CreateParcel(string barcode) => new Parcel
    {
        Barcode = barcode,
        Status = ParcelStatus.Created,
        Sender = "Anders Hejlsberg",
        Recipient = "Elon Musk",
        DeliveryService = DeliveryServiceType.Standard,
        Contents = "Signed C# language specification and a birthday card",
        History = new List<History>
        {
            new() { Status = ParcelStatus.Created, Timestamp = DateTime.UtcNow }
        }
    };
}