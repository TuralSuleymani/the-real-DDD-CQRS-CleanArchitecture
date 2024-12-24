namespace VOEConsulting.Flame.BasketContext.Application.Baskets.Queries.GetBasket
{
    public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, BasketDto>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public GetBasketQueryHandler(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto, IDomainError>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetByIdAsync(request.BasketId);
            if (basket is null)
                return Result.Failure<BasketDto, IDomainError>(DomainError.NotFound("Basket not found."));

            // Map the Basket domain object to a BasketDto
            var basketDto = _mapper.Map<BasketDto>(basket);
            return Result.Success<BasketDto, IDomainError>(basketDto);

        }
    }
}
