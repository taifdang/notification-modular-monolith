using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hookpay.Shared.Modules
{
    public static class Extensions
    {
        public static IHostBuilder ConfigureModules(this IHostBuilder builder)
     => builder.ConfigureAppConfiguration((ctx, cfg) =>
     {
         foreach (var settings in GetSettings("*"))
         {
             cfg.AddJsonFile(settings);
         }

         foreach (var settings in GetSettings($"*.{ctx.HostingEnvironment.EnvironmentName}"))
         {
             cfg.AddJsonFile(settings);
         }

         IEnumerable<string> GetSettings(string pattern)
             => Directory.EnumerateFiles(ctx.HostingEnvironment.ContentRootPath,
                 $"module.{pattern}.json", SearchOption.AllDirectories);
     });
        public static WebApplicationBuilder ConfigureModules(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;
            foreach (var settings in GetSettings("*"))
            {
                builder.Configuration.AddJsonFile(settings);
            }

            foreach (var settings in GetSettings($"*.{env.EnvironmentName}"))
            {
                builder.Configuration.AddJsonFile(settings);
            }

            IEnumerable<string> GetSettings(string pattern) =>
                Directory.EnumerateFiles(env.ContentRootPath, $"module.{pattern}.json", SearchOption.AllDirectories);

            return builder;
        }
    }
}
