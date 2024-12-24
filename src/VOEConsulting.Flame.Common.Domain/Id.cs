using VOEConsulting.Flame.Common.Domain.Extensions;
namespace VOEConsulting.Flame.Common.Domain
{
    public sealed record Id<TModel> : IId, IComparable, IComparable<IId>, IComparable<Guid>, IEquatable<IId>
    {
        public Guid Value { get; init; }

        // Constructors
        public Id(Guid value) => Value = value.EnsureNotDefault(nameof(value));
        public Id() : this(Guid.NewGuid()) { }

        // Factory methods
        public static Id<TModel> New() => new(Guid.NewGuid());
        public static Id<TModel> FromId<TNewModel>(Id<TNewModel> id) => new(id.Value);
        public static Id<TModel> FromGuid(Guid id) => new(id);
        public static Id<TModel> FromString(string id) => new(Guid.Parse(id));

        // Implicit conversions
        public static implicit operator Guid?(Id<TModel>? id) => id?.Value;

        public static implicit operator Guid(Id<TModel> id) => id.Value;

        public static implicit operator Id<TModel>(Guid id) => new(id);

        // Comparisons
        public int CompareTo(object? obj)
        {
            if (obj is IId otherId) return CompareTo(otherId);
            if (obj is Guid otherGuid) return CompareTo(otherGuid);
            if (obj == null) return 1;

            throw new ArgumentException("Object must be of type IId or Guid", nameof(obj));
        }

        public int CompareTo(IId? other) => other?.Value.CompareTo(Value) ?? 1;
        public int CompareTo(Guid other) => Value.CompareTo(other);

        // Equality
        public bool Equals(IId? other) => other?.Value == Value;
    }

}
