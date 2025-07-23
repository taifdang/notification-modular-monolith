
namespace Hookpay.Shared.Utils;

public static class TypeProvider
{
    public static Type? GetTypeFromAssembly(string typeName)
    {
        var result = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes().Where(y => y.FullName == typeName || y.Name == typeName))
            .FirstOrDefault();

        return result;
    }
}
