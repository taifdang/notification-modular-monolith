
namespace BuildingBlocks.Utils
{
    public static class TypeProvider
    {
        public static Type? GetTypeFromCurrentDomainAssembly(string typeName)
        {
            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a
                    .GetTypes()
                    .Where(x => x.FullName == typeName || x.Name == typeName))
                .FirstOrDefault();

            return result;
        }
    }
}
