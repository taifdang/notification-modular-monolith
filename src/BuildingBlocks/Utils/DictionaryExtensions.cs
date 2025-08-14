namespace BuildingBlocks.Utils;

public static class DictionaryExtensions
{
    public static Dictionary<string,object?> SetPayloads(params (string key, object? value)[] items)
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
