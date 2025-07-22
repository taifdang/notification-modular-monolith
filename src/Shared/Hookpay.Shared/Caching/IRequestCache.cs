
namespace Hookpay.Shared.Caching;

public interface IRequestCache
{
    void Set<T>(string key,T value,TimeSpan? expire = null);
    void Get<T>(string key);
}
