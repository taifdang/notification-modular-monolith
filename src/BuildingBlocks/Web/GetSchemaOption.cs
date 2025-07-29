
namespace BuildingBlocks.Web;


public class TokenSchema() { }
public class SignalrSchema() { }

public static class GetSchemaOption<T>
{
    public static readonly string GetScheme = nameof(T);
}
