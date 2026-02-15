using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly LotteryDbContext _context;

        public PurchaseRepository(LotteryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllBuyersOfPresent()
        {
            return await _context.Purchases
                .Include(p => p.Person)
                .Include(p => p.Present)
                .ToListAsync();
        }

        public async Task<List<Purchase>> GetAllPurchasesOfPresent(int presentId)
        {
            return await _context.Purchases
                .Where(p => p.PresentId == presentId)
                .Include(p => p.Person)
                .ToListAsync();
        }

        public async Task<List<Present>> GetSortPresentsByPopular()
        {
            return await _context.Presents
                .OrderByDescending(p => _context.Purchases.Count(pr => pr.PresentId == p.Id))
                .ToListAsync();
        }

        public async Task<List<Present>> GetSortPresentsByPrice()
        {
            return await _context.Presents
                .OrderByDescending(p => p.Price)
                .ToListAsync();
        }

        public async Task AddPurchase(int personId, int basketId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Presents)
                .FirstOrDefaultAsync(b => b.Id == basketId && b.PersonId == personId);

            if (basket == null || basket.Presents == null) return;

            foreach (var item in basket.Presents)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    var purchase = new Purchase
                    {
                        PersonId = personId,
                        PresentId = item.PresentId,
                        PurchaseDate = DateTime.Now
                    };
                    _context.Purchases.Add(purchase);
                }
            }

            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();
        }
    }
}