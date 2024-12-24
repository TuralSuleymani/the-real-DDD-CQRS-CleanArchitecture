using VOEConsulting.Flame.Common.Domain.Events;
using VOEConsulting.Flame.Common.Domain.Extensions;

namespace VOEConsulting.Flame.Common.Domain
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        IReadOnlyCollection<IDomainEvent> PopDomainEvents();
        void ClearEvents();
    }

    public abstract class AggregateRoot<TModel> : Entity<TModel>, IAggregateRoot
        where TModel : IAuditableEntity
    {
        private readonly IList<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
        {
            var events = _domainEvents.ToList();
            ClearEvents();
            return events;
        }
        public void ClearEvents()
        {
            _domainEvents.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            domainEvent.EnsureNonNull();
            _domainEvents.Add(domainEvent);
        }
    }
}
