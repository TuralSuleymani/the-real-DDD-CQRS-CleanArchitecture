using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VOEConsulting.Flame.Common.Domain.Events;
using VOEConsulting.Flame.BasketContext.Application.Behaviours;
using VOEConsulting.Flame.BasketContext.Application.Events.Dispatchers;

namespace VOEConsulting.Flame.BasketContext.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
                configuration.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
                configuration.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
                configuration.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
            });

            // Register all domain event handlers
            services.Scan(scan => scan
                .FromAssemblyOf<IDomainEventHandler<IDomainEvent>>()
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            services.AddAutoMapper(typeof(BasketMappingProfile));
            return services;
        }
    }
}
