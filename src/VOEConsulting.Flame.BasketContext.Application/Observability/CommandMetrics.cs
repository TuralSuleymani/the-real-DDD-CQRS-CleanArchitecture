using System.Diagnostics.Metrics;

namespace VOEConsulting.Flame.BasketContext.Application.Observability
{
    public static class CommandMetrics
    {
        public const string MeterName = "application.commands";

        private static readonly Meter Meter =
            new(MeterName, "1.0.0");

        public static readonly Counter<long> Attempts =
            Meter.CreateCounter<long>(
                "command_attempts_total",
                description: "Total number of command execution attempts");

        public static readonly Counter<long> Success =
            Meter.CreateCounter<long>(
                "command_success_total",
                description: "Total number of successful command executions");

        public static readonly Counter<long> Failures =
            Meter.CreateCounter<long>(
                "command_failures_total",
                description: "Total number of failed command executions");

        public static readonly Histogram<double> Duration =
            Meter.CreateHistogram<double>(
                "command_duration_seconds",
                unit: "s",
                description: "Command execution duration in seconds");
    }
}
