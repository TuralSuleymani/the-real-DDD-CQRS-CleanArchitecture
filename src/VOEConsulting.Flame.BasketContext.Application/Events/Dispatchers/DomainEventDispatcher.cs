using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VOEConsulting.Flame.Common.Domain;
using VOEConsulting.Flame.Common.Domain.Events;

namespace VOEConsulting.Flame.BasketContext.Application.Events.Dispatchers
{
    public class DomainEventDispatcherWithoutMediatr : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcherWithoutMediatr(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            var eventQueue = new Queue<IDomainEvent>(events);

            while (eventQueue.Count > 0)
            {
                var domainEvent = eventQueue.Dequeue();
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                var handlers = _serviceProvider.GetServices(handlerType);

                foreach (var handler in handlers)
                {
                    var handleMethod = handlerType.GetMethod("Handle");
                    if (handleMethod == null) continue;

                    await (Task)handleMethod.Invoke(handler, new object[] { domainEvent, cancellationToken });

                    // If the handler raises additional events, add them to the queue
                    if (domainEvent is IAggregateRoot aggregateRoot)
                    {
                        var additionalEvents = aggregateRoot.PopDomainEvents();
                        foreach (var additionalEvent in additionalEvents)
                        {
                            eventQueue.Enqueue(additionalEvent);
                        }
                    }
                }
            }
        }

    }

    //default Dispatcher
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> initialEvents, CancellationToken cancellationToken = default)
        {
            var eventQueue = new Queue<IDomainEvent>(initialEvents);

            while (eventQueue.Count > 0)
            {
                var currentEvent = eventQueue.Dequeue();

                // Publish the current event
                await _mediator.Publish(currentEvent, cancellationToken);

                // If the current event is associated with an aggregate, check for new events
                if (currentEvent is IAggregateRoot aggregateRoot)
                {
                    var additionalEvents = aggregateRoot.PopDomainEvents();
                    foreach (var additionalEvent in additionalEvents)
                    {
                        eventQueue.Enqueue(additionalEvent);
                    }
                }
            }
        }
    }
}
