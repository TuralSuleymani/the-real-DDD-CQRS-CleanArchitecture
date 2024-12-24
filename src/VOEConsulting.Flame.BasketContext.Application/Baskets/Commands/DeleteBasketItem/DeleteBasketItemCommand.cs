using VOEConsulting.Flame.BasketContext.Application.Abstractions;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.DeleteBasketItem
{
    public record DeleteBasketItemCommand(Guid BasketId, Guid ItemId) : ICommand;

}
