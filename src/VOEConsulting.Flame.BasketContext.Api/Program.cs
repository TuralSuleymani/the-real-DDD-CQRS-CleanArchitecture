using Serilog;
using System.Reflection;
using VOEConsulting.Flame.BasketContext.Api.Extensions;
using VOEConsulting.Flame.BasketContext.Application;
using VOEConsulting.Flame.BasketContext.Infrastructure;

namespace VOEConsulting.Flame.BasketContext.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Host.UseSerilog((context, services, logger) =>
            {
                logger
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)

                    .Enrich.WithProperty(
                        "service.environment",
                        context.HostingEnvironment.EnvironmentName)

                    .Enrich.WithProperty(
                        "service.version",
                        typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown");
            });


            builder.Services.AddControllers();
            
            builder.Services.AddOpenApi();

            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddObservability(
                    configuration: builder.Configuration,
                    environment: builder.Environment);

            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            var app = builder.Build();

           

            // Apply migrations at runtime
            app.ApplyMigrations();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCorrelationContext();

            app.UseCustomSerilogRequestLogging();
            app.MapControllers();

            app.Run();
        }

        
    }
}
