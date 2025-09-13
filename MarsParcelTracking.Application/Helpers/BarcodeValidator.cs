namespace MarsParcelTracking.Application.Helpers;

public class BarcodeValidator
{
    public static bool IsValid(string barcode)
    {
        if (string.IsNullOrEmpty(barcode)) return false;
        if (!barcode.StartsWith("RMARS")) return false;
        if (barcode.Length != 25) return false;
        var numericBarcodePart = barcode.Substring(5, 19);
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