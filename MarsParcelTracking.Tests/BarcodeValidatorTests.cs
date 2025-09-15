using FluentAssertions;
using MarsParcelTracking.Application.Helpers;

namespace MarsParcelTracking.Tests;

[TestFixture]
public class BarcodeValidatorTests
{
    // Valid pattern: RMARS + 19 digits + 1 uppercase letter = 25 chars
    private const string ValidBarcode = "RMARS1234567890123456789M";

    [Test]
    public void IsValid_ShouldReturnTrue_ForValidBarcode()
    {
        // Arrange
        var barcode = ValidBarcode;

        // Act
        var result = BarcodeValidator.IsValid(barcode);

        // Assert
        result.Should().BeTrue();
    }

    [TestCase(null)]
    [TestCase("")]
    public void IsValid_ShouldReturnFalse_ForNullOrEmpty(string? barcode)
    {
        // Act
        var result = BarcodeValidator.IsValid((string)barcode!);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenPrefixInvalid()
    {
        // Arrange
        var barcode = "XMARS1234567890123456789M";

        // Act
        var result = BarcodeValidator.IsValid(barcode);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenLengthInvalid()
    {
        // Arrange
        var barcode = "RMARS1234567890123456789";

        // Act
        var result = BarcodeValidator.IsValid(barcode);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenNumericSectionHasNonDigits()
    {
        // Arrange
        var barcode = "RMARS12345X7890123456789M";

        // Act
        var result = BarcodeValidator.IsValid(barcode);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenLastCharNotUppercase()
    {
        // Arrange
        var barcode = "RMARS1234567890123456789m";

        // Act
        var result = BarcodeValidator.IsValid(barcode);

        // Assert
        result.Should().BeFalse();
    }
}