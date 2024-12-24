using FluentAssertions.Primitives;
using VOEConsulting.Flame.Common.Domain.Events;

namespace VOEConsulting.Flame.BasketContext.Tests.Unit.Extensions
{
    public static class ObjectAssertionsExtensions
    {
        public static AndConstraint<ObjectAssertions> BeEquivalentEventTo<TExpectation>(this ObjectAssertions obj, TExpectation expectation, string because = "",
        params object[] becauseArgs) where TExpectation : IDomainEvent
        {
            return obj.BeEquivalentTo(expectation,
                 options => options.Excluding(t => t.OccurredOnUtc)
                 .Excluding(t => t.Id));
        }
    }
}
