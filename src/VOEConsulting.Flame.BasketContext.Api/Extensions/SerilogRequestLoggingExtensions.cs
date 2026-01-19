using Serilog;
using Serilog.Events;

namespace VOEConsulting.Flame.BasketContext.Api.Extensions
{
    public static class SerilogRequestLoggingExtensions
    {
        public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder app)
        {
            return app.UseSerilogRequestLogging(opts =>
            {
                opts.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} => {StatusCode} in {Elapsed:0.0000} ms";

                opts.EnrichDiagnosticContext = (diag, http) =>
                {
                    diag.Set("http.request.method", http.Request.Method);
                    diag.Set("url.path", http.Request.Path.Value);
                    diag.Set("url.query", http.Request.QueryString.Value);
                    diag.Set("client.ip", http.Connection.RemoteIpAddress?.ToString());
                    diag.Set("user_agent.original", http.Request.Headers.UserAgent.ToString());
                };

                opts.GetLevel = (http, elapsed, ex) =>
                {
                    if (http.Request.Path.StartsWithSegments("/health"))
                        return LogEventLevel.Verbose;

                    if (ex != null || http.Response.StatusCode >= 500) return LogEventLevel.Error;
                    if (http.Response.StatusCode >= 400) return LogEventLevel.Warning;
                    return LogEventLevel.Information;
                };
            });
        }
    }
}
