using FluentValidation;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.AddItemToBasket
{
    public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommand>
    {
        public AddItemToBasketCommandValidator()
        {
            RuleFor(x => x.BasketId).NotEmpty();
            RuleFor(x => x.BasketItem).NotEmpty();
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.Seller).NotNull();
        }
    }

}
