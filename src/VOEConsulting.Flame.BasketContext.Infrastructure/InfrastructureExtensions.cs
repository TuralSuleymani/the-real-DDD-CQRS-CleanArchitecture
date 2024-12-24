using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using VOEConsulting.Flame.BasketContext.Application.Abstractions.Messaging;
using VOEConsulting.Flame.BasketContext.Application.Repositories;
using VOEConsulting.Flame.BasketContext.Infrastructure.Persistence.Repositories;
using VOEConsulting.Infrastructure.Persistence;
namespace VOEConsulting.Flame.BasketContext.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static void ApplyMigrations(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<BasketAppDbContext>();
                    context.Database.Migrate();
                    Console.WriteLine("Database migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
                    throw;
                }
            }
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Get Kafka configuration from appsettings.json
            var kafkaConfig = configuration.GetSection("Kafka");
            var bootstrapServers = kafkaConfig["BootstrapServers"];
            var defaultTopic = kafkaConfig["DefaultTopic"];

            // Register KafkaIntegrationEventPublisher
            services.AddSingleton<IIntegrationEventPublisher>(sp =>
            {
                return new KafkaIntegrationEventPublisher(bootstrapServers!, defaultTopic!);
            });

            // Register DbContext with a connection string
            services.AddDbContext<BasketAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
