namespace BuildingBlocks.Utils;

//ref: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
/// <summary>
/// Create a dictionary from a set of key-value pairs
/// </summary>
public static class DictionaryExtensions
{
    public static IDictionary<string,object?> SetPayloads(params (string key, object? value)[] items)
    {
        var payloads = new Dictionary<string,object?>();
        foreach (var (key,value) in items)
        {
            if (payloads.ContainsKey(key))
                throw new ArgumentException($"Duplicate key: {key}");
            payloads[key] = value;
        }
        return payloads;
    }
}
