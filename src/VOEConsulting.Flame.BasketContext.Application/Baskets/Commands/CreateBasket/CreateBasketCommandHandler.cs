using VOEConsulting.Flame.BasketContext.Domain.Baskets;
using VOEConsulting.Flame.Common.Domain;

namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Commands.CreateBasket
{
    public class CreateBasketCommandHandler : CommandHandlerBase<CreateBasketCommand, Guid>
    {
        private readonly IBasketRepository _basketRepository;
        private Basket? _createdBasket;

        public CreateBasketCommandHandler(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IDomainEventDispatcher domainEventDispatcher)
            : base(domainEventDispatcher, unitOfWork)
        {
            _basketRepository = basketRepository;
        }

        protected override async Task<Result<Guid, IDomainError>> ExecuteAsync(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Core operation
            if(await _basketRepository.IsExistByCustomerIdAsync(request.Customer.Id))
            {
                return Result.Failure<Guid, IDomainError>(DomainError.Conflict("Basket already exist for the given customer"));
            }
            Customer customer = Customer.Create(request.Customer.IsEliteMember, request.Customer.Id);
            _createdBasket = Basket.Create(request.TaxPercentage, customer);

            await _basketRepository.AddAsync(_createdBasket, cancellationToken);

            return Result.Success<Guid, IDomainError>(_createdBasket.Id);
        }

        protected override IAggregateRoot? GetAggregateRoot(Result<Guid, IDomainError> result)
        {
            // Return the created aggregate root to dispatch domain events
            return _createdBasket;
        }
    }
}
