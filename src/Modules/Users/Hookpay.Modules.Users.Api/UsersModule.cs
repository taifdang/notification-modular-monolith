using Hookpay.Modules.Users.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Api
{
    public class UsersModule 
    {      
        public string Name { get; } = "Users";

        public IEnumerable<string> Policies => new[]
        {
            "users"
        };

        public void Register(IServiceCollection services)
        {
            services.AddCore();
        }

        public void Use(WebApplication app)
        {
            app.UseCore();
        }
    }
}
