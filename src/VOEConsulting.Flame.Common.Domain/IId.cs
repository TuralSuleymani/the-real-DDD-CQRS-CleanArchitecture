namespace VOEConsulting.Flame.Common.Domain
{
    public interface IId : IComparable, IComparable<IId>, IComparable<Guid>, IEquatable<IId>
    {
        Guid Value { get; }
    }
}
