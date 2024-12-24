using VOEConsulting.Flame.BasketContext.Application.Abstractions;
using VOEConsulting.Flame.BasketContext.Application.Baskets.Dtos;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Queries.GetBasket
{
    public record GetBasketQuery(Guid BasketId) : IQuery<BasketDto>;

}
