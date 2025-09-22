using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LiteDAC;

public static class LiteDbExtension
{
    public static void AddLiteDbService(this IServiceCollection services)
    {
        services.TryAddScoped<LiteDb>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var context = sp.GetRequiredService<IHttpContextAccessor>();

            var subdomain = config.GetSection("SubDomain").Value;

            if (String.IsNullOrEmpty(subdomain))
            {
                subdomain = context.HttpContext.Request.Host.Host.Split('.')[0];
            }
            
            return new LiteDb(config, subdomain);
        });
    }
    
    public static void AddLiteDbService(this IServiceCollection services, string domain)
    {
        services.TryAddScoped<LiteDb>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new LiteDb(config, domain);
        });
    }
}