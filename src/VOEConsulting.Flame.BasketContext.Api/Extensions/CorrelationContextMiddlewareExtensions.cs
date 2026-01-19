using VOEConsulting.Flame.BasketContext.Api.Middlewares;

namespace VOEConsulting.Flame.BasketContext.Api.Extensions
{
    public static class CorrelationContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationContext(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationContextMiddleware>();
        }
    }
}
