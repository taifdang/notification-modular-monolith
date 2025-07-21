using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Modules
{
    public interface IModule
    {
        string Name { get; }
        IEnumerable<string> Policies { get; }
        void Register(IServiceCollection module);
        void Use(WebApplication app);
    }
}
