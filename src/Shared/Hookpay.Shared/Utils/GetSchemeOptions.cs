using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Utils;

public class TokenScheme() { }
public class SignalRScheme() { }

public static class GetSchemeOptions<T>
{
    public static readonly string CustomScheme = nameof(T);
}
