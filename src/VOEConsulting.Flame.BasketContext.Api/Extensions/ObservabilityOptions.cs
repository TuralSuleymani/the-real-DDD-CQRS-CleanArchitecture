using OpenTelemetry.Trace;
using VOEConsulting.Flame.BasketContext.Application.Observability;

namespace VOEConsulting.Flame.BasketContext.Api.Extensions
{
    public sealed class ObservabilityOptions
    {
        public required string ServiceName { get; init; }
        public required string ServiceVersion { get; init; }
        public required string EnvironmentName { get; init; }
        public required string InstanceId { get; init; }

        public Uri OtlpEndpoint { get; init; } = new("http://localhost:4317");
        public bool EnableOtlp { get; init; } = true;
        public bool EnableConsoleExporter { get; init; } = false;

        public double TraceSamplingRatio { get; init; } = 0.10;

        public bool EmitDbStatement { get; init; } = false;

        public string CorrelationHeaderName { get; init; } = "X-Correlation-Id";

        public IReadOnlyList<string> ActivitySources { get; init; } = Array.Empty<string>();
        public IReadOnlyList<string> Meters { get; init; } = Array.Empty<string>();

        public Sampler BuildSampler()
            => new ParentBasedSampler(new TraceIdRatioBasedSampler(Math.Clamp(TraceSamplingRatio, 0.0, 1.0)));

        public static ObservabilityOptions From(IConfiguration config, IHostEnvironment env)
        {
            var serviceName =
                config["OTEL_SERVICE_NAME"]
                ?? Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME")
                ?? BasketActivitySource.ServiceName;

            var serviceVersion =
                config["SERVICE_VERSION"]
                ?? Environment.GetEnvironmentVariable("SERVICE_VERSION")
                ?? typeof(ObservabilityOptions).Assembly.GetName().Version?.ToString()
                ?? "1.0.0";

            var endpoint =
                config["OTEL_EXPORTER_OTLP_ENDPOINT"]
                ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")
                ?? "http://localhost:4317";

            var enableOtlp =
                bool.TryParse(config["OTEL_EXPORTER_OTLP_ENABLED"], out var otlpEnabled)
                    ? otlpEnabled
                    : true;

            var enableConsole =
                bool.TryParse(config["OTEL_CONSOLE_EXPORTER_ENABLED"], out var consoleEnabled)
                    ? consoleEnabled
                    : env.IsDevelopment();

            var ratio =
                double.TryParse(config["OTEL_TRACES_SAMPLING_RATIO"], out var r)
                    ? Math.Clamp(r, 0.0, 1.0)
                    : (env.IsDevelopment() ? 1.0 : 0.10);

            var emitDbStatement =
                bool.TryParse(config["OTEL_DB_STATEMENT_ENABLED"], out var dbStmt)
                    ? dbStmt
                    : false;

            return new ObservabilityOptions
            {
                ServiceName = serviceName,
                ServiceVersion = serviceVersion,
                EnvironmentName = env.EnvironmentName,
                InstanceId = Environment.MachineName,

                OtlpEndpoint = new Uri(endpoint),
                EnableOtlp = enableOtlp,
                EnableConsoleExporter = enableConsole,

                TraceSamplingRatio = ratio,
                EmitDbStatement = emitDbStatement,

                ActivitySources = new[]
                {
                BasketActivitySource.ServiceName
            },

                Meters = new[]
                {
                BasketMetrics.MeterName
            }
            };
        }
    }
}
