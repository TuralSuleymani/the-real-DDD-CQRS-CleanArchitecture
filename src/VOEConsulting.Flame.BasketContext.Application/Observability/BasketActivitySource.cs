using System.Diagnostics;

namespace VOEConsulting.Flame.BasketContext.Application.Observability
{
    public class BasketActivitySource
    {
        public const string ServiceName = "flame.basket-service";
        public static readonly ActivitySource Instance = new(ServiceName);
    }
}
