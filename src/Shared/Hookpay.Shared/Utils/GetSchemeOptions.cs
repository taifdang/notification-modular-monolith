
namespace Hookpay.Shared.Utils;

public class TokenScheme() { }
public class SignalRScheme() { }

public static class GetSchemeOptions<T>
{
    public static readonly string CustomScheme = nameof(T);
}
