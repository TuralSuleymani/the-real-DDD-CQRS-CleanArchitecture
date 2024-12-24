using MediatR;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.CalculateTotalAmount
{
    public record CalculateTotalAmountCommand(Guid BasketId) : IRequest;

}
