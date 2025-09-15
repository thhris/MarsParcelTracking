namespace MarsParcelTracking.Application.Helpers;

public class BarcodeValidator
{
    private const string BarcodePrefix = "RMARS";
    private const int ExpectedLength = 25;
    private const int NumericSectionStart = 5;
    private const int NumericSectionLength = 19;

    public static bool IsValid(string barcode)
    {
        if (string.IsNullOrEmpty(barcode)) return false;
        if (!barcode.StartsWith(BarcodePrefix)) return false;
        if (barcode.Length != ExpectedLength) return false;
        var numericBarcodePart = barcode.Substring(NumericSectionStart, NumericSectionLength);
        if (!IsStringNumeric(numericBarcodePart)) return false;
        if (!IsLastCharacterCapital(barcode)) return false;

        return true;
    }

    private static bool IsLastCharacterCapital(string barcode)
    {
        return char.IsUpper(barcode.Last());
    }

    private static bool IsStringNumeric(string numericPartBarcode)
    {
        return numericPartBarcode.All(char.IsDigit);
    }
}