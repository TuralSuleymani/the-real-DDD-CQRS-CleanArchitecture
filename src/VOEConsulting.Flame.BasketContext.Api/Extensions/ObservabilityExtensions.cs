using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace VOEConsulting.Flame.BasketContext.Api.Extensions
{
    public static class ObservabilityExtensions
    {
        public static IServiceCollection AddObservability(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            var options = ObservabilityOptions.From(configuration, environment);

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName: options.ServiceName, serviceVersion: options.ServiceVersion)
                .AddAttributes(new[]
                {
                new KeyValuePair<string, object>("deployment.environment", options.EnvironmentName),
                new KeyValuePair<string, object>("service.instance.id", options.InstanceId),
                });

           services.AddOpenTelemetry()
                .ConfigureResource(rb =>
                {
                    rb.Clear();
                    rb.AddService(
                        serviceName: options.ServiceName,
                        serviceVersion: options.ServiceVersion);

                    rb.AddAttributes(new[]
                    {
                        new KeyValuePair<string, object>("deployment.environment", options.EnvironmentName),
                        new KeyValuePair<string, object>("service.instance.id", options.InstanceId),
                    });
                })
                .WithTracing(tracing =>
                {
                    tracing.SetSampler(options.BuildSampler());

                    foreach (var src in options.ActivitySources)
                        tracing.AddSource(src);

                    tracing.AddAspNetCoreInstrumentation(o =>
                    {
                        o.RecordException = true;

                        o.EnrichWithHttpRequest = (activity, request) =>
                        {
                            activity.SetTag("correlation.trace_id", activity.TraceId.ToString());

                             var cid = request.Headers[options.CorrelationHeaderName].ToString();
                             if (!string.IsNullOrWhiteSpace(cid)) activity.SetTag("correlation.id", cid);

                            activity.SetTag("http.client_ip",
                                request.HttpContext.Connection.RemoteIpAddress?.ToString());
                        };

                        o.EnrichWithHttpResponse = (activity, response) =>
                        {
                            activity.SetTag("http.response_content_length", response.ContentLength ?? 0);
                        };
                    });

                    tracing.AddHttpClientInstrumentation(o => o.RecordException = true);

                    tracing.AddEntityFrameworkCoreInstrumentation(o =>
                    {
                        o.EnrichWithIDbCommand = (activity, command) =>
                        {
                            if (options.EmitDbStatement)
                            {
                                // semantic convention tag for db statement
                                activity.SetTag("db.statement", command.CommandText);
                            }
                        };
                    });

                    if (options.EnableOtlp)
                    {
                        tracing.AddOtlpExporter(o =>
                        {
                            o.Endpoint = options.OtlpEndpoint;
                            o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                    }

                    if (options.EnableConsoleExporter)
                        tracing.AddConsoleExporter();
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddHttpClientInstrumentation();
                    metrics.AddRuntimeInstrumentation();
                    metrics.AddProcessInstrumentation();

                    foreach (var meter in options.Meters)
                        metrics.AddMeter(meter);

                    if (options.EnableOtlp)
                    {
                        metrics.AddOtlpExporter(o =>
                        {
                            o.Endpoint = options.OtlpEndpoint;
                            o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                    }

                    if (options.EnableConsoleExporter)
                        metrics.AddConsoleExporter();
                });

            return services;
        }
    }
}
