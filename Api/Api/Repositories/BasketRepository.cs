using Api.Data;
using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly LotteryDbContext _context;
        public BasketRepository(LotteryDbContext context)
        {
            _context = context;
        }

        public async Task AddToBasket(int personId, int presentId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Presents)
                .FirstOrDefaultAsync(b => b.PersonId == personId);

            if (basket == null)
            {
                basket = new Basket
                {
                    PersonId = personId,
                    Presents = new List<Api.Models.BasketItem>()
                };
                _context.Baskets.Add(basket);
            }

            basket.Presents ??= new List<Api.Models.BasketItem>();

            var existingItem = basket.Presents.FirstOrDefault(i => i.PresentId == presentId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                basket.Presents.Add(new Api.Models.BasketItem
                {
                    PresentId = presentId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromBasket(int personId, int presentId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Presents)
                .FirstOrDefaultAsync(b => b.PersonId == personId);

            if (basket != null)
            {
                var item = basket.Presents.FirstOrDefault(i => i.PresentId == presentId);
                if (item != null)
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                    }
                    else
                    {
                        basket.Presents.Remove(item);
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<Basket?> GetMyBasket(int personId)
        {
            return await _context.Baskets
                .Include(b => b.Presents!)
                .ThenInclude(bi => bi.Present)
                .FirstOrDefaultAsync(b => b.PersonId == personId);
        }

        public async Task ClearItemCompletely(int personId, int presentId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Presents)
                .FirstOrDefaultAsync(b => b.PersonId == personId);

            if (basket != null)
            {
                var itemsToRemove = basket.Presents
                    .Where(pi => pi.PresentId == presentId)
                    .ToList();

                if (itemsToRemove.Any())
                {
                    _context.Set<Api.Models.BasketItem>().RemoveRange(itemsToRemove);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}