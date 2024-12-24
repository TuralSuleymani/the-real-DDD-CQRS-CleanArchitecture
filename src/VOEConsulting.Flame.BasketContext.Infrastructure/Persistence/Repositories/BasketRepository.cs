using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VOEConsulting.Flame.BasketContext.Application.Repositories;
using VOEConsulting.Flame.BasketContext.Domain.Baskets;
using VOEConsulting.Flame.BasketContext.Infrastructure.Entities;
using VOEConsulting.Flame.Common.Core.Exceptions;
using VOEConsulting.Infrastructure.Persistence;

namespace VOEConsulting.Flame.BasketContext.Infrastructure.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketAppDbContext _dbContext;
        private readonly IMapper _mapper;
        public BasketRepository(BasketAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddAsync(Basket basket, CancellationToken cancellationToken)
        {
            var basketEntity = _mapper.Map<BasketEntity>(basket);
            await _dbContext.AddAsync(basketEntity);
        }

        public async Task RemoveBasketItemAsync(Guid basketId, Guid basketItemId)
        {
            // Load the basket
            var basket = await GetByBasketEntityIdAsync(basketId);

            // Find the basket item to be removed
            var basketItem = basket.BasketItems
                .FirstOrDefault(bi => bi.Id == basketItemId)
                ?? throw new FlameApplicationException("Basket item not found.");

            // Get the associated seller
            var seller = basketItem.Seller;

            // Remove the basket item
            _dbContext.BasketItems.Remove(basketItem);

            // Check if any other basket items reference this seller
            bool isSellerReferenced = _dbContext.BasketItems
                .Any(bi => bi.Seller.Id == seller.Id && bi.Id != basketItemId);

            if (!isSellerReferenced)
            {
                // Delete the seller if no other references exist
                _dbContext.Sellers.Remove(seller);
            }
        }

        public async Task AddBasketItemAsync(Guid basketId, BasketItem basketItem)
        {
            var basketItemEntity = _mapper.Map<BasketItemEntity>(basketItem);
            basketItemEntity.BasketId = basketId;
            // Load the basket
            var basketEntity = await GetByBasketEntityIdAsync(basketId);

            if (basketEntity is null)
                throw new FlameApplicationException("Basket is doesn't exist");

            var existingBasketItem = basketEntity.BasketItems.FirstOrDefault(x => x.Name == basketItem.Name);

            if (existingBasketItem is not null)
                throw new FlameApplicationException("This basket item already exists");

            // Check if the seller already exists in the database
            var existingSeller = await _dbContext.Sellers
                .FirstOrDefaultAsync(s => s.Name == basketItem.Seller.Name);

            // Reuse existing seller or associate the new one
            if (existingSeller is not null)
            {
                // Reuse the existing seller
                basketItemEntity.SellerId = existingSeller.Id;
                basketItemEntity.Seller = null; // Avoid attaching the Seller again
            }
            else
            {
                // Use the new seller
                basketItemEntity.SellerId = basketItemEntity.Seller.Id;
                _dbContext.Sellers.Attach(basketItemEntity.Seller); // Attach the new Seller
            }

            // Add the basket item to the basket

            await _dbContext.BasketItems.AddAsync(basketItemEntity);

        }

        public Task DeleteAsync(Guid id)
        {
            return Task.FromResult(_dbContext.Remove(id));
        }

        public Task<IEnumerable<Basket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Basket?> GetByIdAsync(Guid id)
        {
            var basketEntity = await _dbContext.Baskets
                             .Include(b => b.Customer)
                             .Include(b => b.Coupon)
                             .Include(b => b.BasketItems)
                                 .ThenInclude(bi => bi.Seller)
                             .Where(b => b.Id == id)
                             .FirstOrDefaultAsync();

            return _mapper.Map<Basket>(basketEntity);
        }

        private async Task<BasketEntity> GetByBasketEntityIdAsync(Guid basketId)
        {
            // Retrieve the basket including its items and associated sellers
            return await _dbContext.Baskets
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Seller)
                .FirstOrDefaultAsync(b => b.Id == basketId)
                ?? throw new InvalidOperationException("Basket not found.");
        }

        public Task UpdateAsync(Basket entity)
        {
            return Task.FromResult(_dbContext.Update(entity));
        }

        public async Task<bool> IsExistsAsync(Guid id)
        {
            return (await _dbContext.Baskets.FirstOrDefaultAsync(x => x.Id == id)) != null;
        }

        public async Task<bool> IsExistByCustomerIdAsync(Guid customerId)
        {
            return (await _dbContext.Baskets.FirstOrDefaultAsync(x => x.CustomerId == customerId)) != null;
        }
    }
}
