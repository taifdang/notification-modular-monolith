using Hookpay.Modules.Topups.Core.Extensions;
using Hookpay.Shared.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Api
{
    public class TopupsModule : IModule
    {       
        public string Name { get; } = "Topups";

        public IEnumerable<string> Policies => new[] 
        {
            "topups"
        };

        public void Register(IServiceCollection module)
        {
            module.AddCore();
        }

        public void Use(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
