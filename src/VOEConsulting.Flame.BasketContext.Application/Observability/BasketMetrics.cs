using System.Diagnostics.Metrics;

namespace VOEConsulting.Flame.BasketContext.Application.Observability
{
    public static class BasketMetrics
    {
        public const string MeterName = "basket.service";
        private static readonly Meter Meter = new(MeterName, "1.0.0");

        public static readonly Counter<long> BasketCreateAttempts =
            Meter.CreateCounter<long>("basket_create_attempts_total");

        public static readonly Counter<long> BasketCreateSuccess =
            Meter.CreateCounter<long>("basket_create_success_total");

        public static readonly Counter<long> BasketCreateConflicts =
            Meter.CreateCounter<long>("basket_create_conflicts_total");

        public static readonly Histogram<double> BasketCreateDuration =
            Meter.CreateHistogram<double>(
                "basket_create_duration_seconds",
                unit: "s");
    }
}
