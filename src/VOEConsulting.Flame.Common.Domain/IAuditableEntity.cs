namespace VOEConsulting.Flame.Common.Domain
{
    public interface IAuditableEntity
    {
        public DateTimeOffset CreatedAtUtc { get; }
        public DateTimeOffset LastModifiedAtUtc { get; }

    }
}
