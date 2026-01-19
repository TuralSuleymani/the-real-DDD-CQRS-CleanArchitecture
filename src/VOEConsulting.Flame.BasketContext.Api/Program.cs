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

            builder.Host.UseSerilog((ctx, lc) =>
            {
                lc.ReadFrom.Configuration(ctx.Configuration)
                  .Enrich.WithProperty("service.environment", ctx.HostingEnvironment.EnvironmentName)
                  .Enrich.WithProperty("service.version",
                      typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown");
            });

            builder.Services.AddControllers();
            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            var app = builder.Build();

            app.UseCorrelationContext();

            app.UseCustomSerilogRequestLogging();

            // Apply migrations at runtime
            app.ApplyMigrations();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        
    }
}
