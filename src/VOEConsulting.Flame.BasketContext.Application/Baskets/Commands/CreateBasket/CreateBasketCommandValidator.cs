using FluentValidation;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.CreateBasket
{
    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.Customer).NotNull();
            RuleFor(x => x.TaxPercentage).GreaterThanOrEqualTo(0);
        }
    }
}
