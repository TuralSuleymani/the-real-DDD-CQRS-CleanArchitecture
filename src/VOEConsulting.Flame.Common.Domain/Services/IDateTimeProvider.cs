namespace VOEConsulting.Flame.Common.Domain.Services
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow();
    }
}
