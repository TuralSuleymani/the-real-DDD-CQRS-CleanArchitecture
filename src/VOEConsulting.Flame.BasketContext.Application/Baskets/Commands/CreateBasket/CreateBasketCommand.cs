namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.CreateBasket
{
    public record CreateBasketCommand(decimal TaxPercentage, CustomerDto Customer) : ICommand<Guid>;

}
