using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Caching;

public interface IRequestCache
{
    void Set<T>(string key,T value,TimeSpan? expire = null);
    void Get<T>(string key);
}
