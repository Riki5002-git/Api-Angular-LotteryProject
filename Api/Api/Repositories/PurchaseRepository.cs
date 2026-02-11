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
                .Include(p => p.Present) // בלי השורה הזו, הנתון יחזור כ-null
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
            var personExists = await _context.Persons.AnyAsync(p => p.Id == personId);
            if (!personExists) throw new Exception($"User with ID {personId} not found in database.");
            var basket = await _context.Baskets
                .Include(b => b.Presents)
                .FirstOrDefaultAsync(b => b.Id == basketId && b.PersonId == personId);

            if (basket == null) throw new Exception("Basket not found or does not belong to this user");
            if (basket.Presents == null || !basket.Presents.Any())
            {
                return;
            }
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
            _context.Remove(basket);
            await _context.SaveChangesAsync();
        }
    }
}