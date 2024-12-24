using VOEConsulting.Flame.BasketContext.Application.Abstractions;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.UpdateBasketItem
{
    public record UpdateBasketItemCountCommand(Guid BasketId, Guid ItemId, int Quantity) : ICommand;

}
