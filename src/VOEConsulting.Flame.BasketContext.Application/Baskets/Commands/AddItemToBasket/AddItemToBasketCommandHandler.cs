using VOEConsulting.Flame.BasketContext.Domain.Baskets;
using VOEConsulting.Flame.Common.Domain;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.AddItemToBasket
{
    public class AddItemToBasketCommandHandler : CommandHandlerBase<AddItemToBasketCommand, Guid>
    {
        private readonly IBasketRepository _basketRepository;
        private Basket? _basket;

        public AddItemToBasketCommandHandler(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IDomainEventDispatcher domainEventDispatcher)
            : base( domainEventDispatcher, unitOfWork)
        {
            _basketRepository = basketRepository;
        }

        protected override async Task<Result<Guid, IDomainError>> ExecuteAsync(AddItemToBasketCommand request, CancellationToken cancellationToken)
        {

            var quantity = Quantity.Create(request.Quantity.Value, request.Quantity.QuantityLimit, request.Quantity.PricePerUnit);

            var seller = Seller.Create(request.Seller.Name, request.Seller.Rating, request.Seller.ShippingLimit, request.Seller.ShippingCost, request.Seller.Id);

            _basket = await _basketRepository.GetByIdAsync(request.BasketId);

            if (_basket == null)
                return Result.Failure<Guid, IDomainError>(DomainError.NotFound());

            var basketItem = BasketItem.Create(request.BasketItem.Name, quantity, request.BasketItem.ImageUrl, seller,request.BasketItem.ItemId);

            _basket.AddItem(basketItem);

            await _basketRepository.AddBasketItemAsync(request.BasketId, basketItem);

            return Result.Success<Guid, IDomainError>(basketItem.Id);
        }

        protected override IAggregateRoot? GetAggregateRoot(Result<Guid, IDomainError> result)
        {
            // Return the created aggregate root to dispatch domain events
            return _basket;
        }
    }
}
