using Serilog.Context;
using System.Diagnostics;

namespace VOEConsulting.Flame.BasketContext.Api.Middlewares
{
    public sealed class CorrelationContextMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";

        private readonly RequestDelegate _next;

        public CorrelationContextMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // 1) Correlation Id (incoming or generated)
            var correlationId =
                context.Request.Headers.TryGetValue(CorrelationIdHeader, out var cid)
                && !string.IsNullOrWhiteSpace(cid)
                    ? cid.ToString()
                    : context.TraceIdentifier;

            context.Response.Headers[CorrelationIdHeader] = correlationId;

            // 2) OpenTelemetry / Activity tracing
            var activity = Activity.Current;

            string? traceId = activity?.TraceId.ToString();
            string? spanId = activity?.SpanId.ToString();

            // 3) Push into Serilog log context
            using (LogContext.PushProperty("correlation.id", correlationId))
            using (LogContext.PushProperty("trace.id", traceId))
            using (LogContext.PushProperty("span.id", spanId))
            {
                await _next(context);
            }
        }
    }
}
