namespace Wallet.Extensions.Infrastructure;
public static class WalletExtensions
{
    public static string? GetPaymentCode(string? @params)
    {
        if (string.IsNullOrWhiteSpace(@params)) return @params;
        var parts = @params.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2 && parts[0].Equals("NAPCOIN",StringComparison.OrdinalIgnoreCase))
            return parts[1];
        return @params;
    }

    public static string GeneratePaymentCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 8)
           .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}
