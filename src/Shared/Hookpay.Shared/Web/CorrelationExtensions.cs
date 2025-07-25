﻿
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Hookpay.Shared.Web;

public static class CorrelationExtensions
{
    private const string CorrelationId = "correlationId";
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if(!context.Request.Headers.TryGetValue(CorrelationId, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString("N");  
            }

            context.Items[CorrelationId] = correlationId.ToString();

            await next();
        });
    }

    public static Guid GetCorrelationId(this HttpContext context)
    {
        context.Items.TryGetValue(CorrelationId, out var correlationId);

        return string.IsNullOrEmpty(correlationId?.ToString()) ? Guid.NewGuid() : new Guid(correlationId.ToString()!);
    }
}
