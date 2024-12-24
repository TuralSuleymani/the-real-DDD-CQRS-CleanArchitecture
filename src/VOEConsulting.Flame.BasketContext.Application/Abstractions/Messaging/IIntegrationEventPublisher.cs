using Confluent.Kafka;
using System.Text.Json;
using VOEConsulting.Flame.BasketContext.Application.Events.Integration;
using VOEConsulting.Flame.Common.Core.Events;

namespace VOEConsulting.Flame.BasketContext.Application.Abstractions.Messaging
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : IntegrationEvent;
    }

    public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }

    public interface IIntegrationEventConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }

    public class BasketValidatedHandler : IIntegrationEventHandler<BasketCreatedIntegrationEvent>
    {
        public Task HandleAsync(BasketCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class BasketValidationFailedHandler : IIntegrationEventHandler<BasketCreatedIntegrationEvent>
    {
        public async Task HandleAsync(BasketCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Basket validation failed: Error = {@event.EventType}");
            await Task.CompletedTask;
        }
    }

    public class IntegrationEventConsumer : IIntegrationEventConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly Dictionary<Type, object> _handlers; // Maps event types to their handlers
        private readonly Dictionary<string, Type> _eventTypeMappings; // Maps topics to event types

        public IntegrationEventConsumer(
            IConsumer<string, string> consumer,
            Dictionary<Type, object> handlers,
            Dictionary<string, Type> eventTypeMappings)
        {
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _eventTypeMappings = eventTypeMappings ?? throw new ArgumentNullException(nameof(eventTypeMappings));
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Consume message
                    var consumeResult = _consumer.Consume(cancellationToken);

                    // Get the event type from the topic
                    if (_eventTypeMappings.TryGetValue(consumeResult.Topic, out var eventType))
                    {
                        // Deserialize the message into the event type
                        var integrationEvent = JsonSerializer.Deserialize(consumeResult.Message.Value, eventType);

                        if (integrationEvent != null)
                        {
                            // Dispatch the event to the appropriate handler
                            await DispatchEventAsync(integrationEvent, cancellationToken);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No event type mapping found for topic: {consumeResult.Topic}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during message consumption: {ex.Message}");
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task DispatchEventAsync(object integrationEvent, CancellationToken cancellationToken)
        {
            var eventType = integrationEvent.GetType();

            // Find the handler for the event type
            if (_handlers.TryGetValue(eventType, out var handler))
            {
                if (handler is IIntegrationEventHandler<IntegrationEvent> typedHandler)
                {
                    // Safely cast and invoke the handler
                    await typedHandler.HandleAsync((dynamic)integrationEvent, cancellationToken);
                }
                else
                {
                    Console.WriteLine($"Handler for {eventType.Name} does not implement the correct interface.");
                }
            }
            else
            {
                Console.WriteLine($"No handler found for event type: {eventType.Name}");
            }
        }
    }



}
