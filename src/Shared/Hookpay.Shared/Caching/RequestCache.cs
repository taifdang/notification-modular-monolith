using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Caching;

public class RequestCache : IRequestCache
{
    private readonly IMemoryCache _cache;
    public RequestCache(IMemoryCache cache) {  _cache = cache; }
    public void Get<T>(string key)
    {
       _cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, TimeSpan? expire = null)
    {
        _cache.Set<T>(key, value, expire ?? TimeSpan.FromMinutes(5));
    }
}
