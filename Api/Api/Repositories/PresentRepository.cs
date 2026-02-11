using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class PresentRepository : IPresentRepository
    {
        private readonly LotteryDbContext _context;
        public PresentRepository(LotteryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Present>> GetAllPresents()
        {
            return await _context.Presents
                .Include(p => p.Winner)
                .ToListAsync();
        }

        public async Task AddPresent(Present present)
        {
            _context.Presents.Add(present);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePresent(int id)
        {
            var present = await _context.Presents.FindAsync(id);
            if (present != null)
            {
                _context.Presents.Remove(present);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePresent(Present present)
        {
            _context.Presents.Update(present);
            await _context.SaveChangesAsync();
        }

        public async Task<Present?> GetPresentById(int id)
        {
            return await _context.Presents.FindAsync(id);
        }

        public async Task<Present?> GetPresentsByPresentName(string name)
        {
            return await _context.Presents.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Present>> GetPresentsByDonorName(string donorName)
        {
            var donor = await _context.Donors
                .FirstOrDefaultAsync(d => d.FirstName == donorName);
            if (donor == null) throw new Exception("Donor not found");
            return await _context.Presents
                .Where(p => p.DonorId == donor.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Present>> GetPresentsByPurchasesAmount(int amount)
        {
            return await _context.Presents
                .Where(p => p.PurchasesAmount == amount)
                .ToListAsync();
        }

        public async Task<double> GetPresentPrice(int id)
        {
            var present = await _context.Presents.FindAsync(id);
            return present?.Price ?? 0;
        }

        public async Task AddPictureUrl(int id, string url)
        {
            var present = await _context.Presents.FindAsync(id);
            if (present != null)
            {
                present.PictureUrl = url;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Donor?> GetDonorsPresent(string PresentName)
        {
            return await (from p in _context.Presents
                         join d in _context.Donors on p.DonorId equals d.Id
                         where p.Name == PresentName
                         select d).FirstOrDefaultAsync();
        }
    }
}