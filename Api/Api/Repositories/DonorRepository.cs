using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly LotteryDbContext _context;
        public DonorRepository(LotteryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Donor>> GetAllDonors()
        {
            return await _context.Donors.ToListAsync();
        }

        public async Task AddDonor(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }

        public async Task<Donor?> GetDonorById(int id)
        {
            return await _context.Donors.FindAsync(id);
        }

        public async Task UpdateDonor(Donor donor)
        {
            _context.Donors.Update(donor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDonor(int id)
        {
            var donor = await GetDonorById(id);
            if (donor != null)
            {
                _context.Donors.Remove(donor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Donor?> GetDonorByName(string firstName, string lastName)
        {
            return await _context.Donors.FirstOrDefaultAsync(d => d.FirstName == firstName && d.LastName == lastName);
        }

        public async Task<Donor?> GetDonorByEmail(string email)
        {
            return await _context.Donors.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<Donor?> GetDonorByPresent(string present)
        {
            var curPresent = await _context.Presents.FirstOrDefaultAsync(p => p.Name == present);
            if (curPresent == null) return null;
            return await _context.Donors
                .FirstOrDefaultAsync(d => d.Id == curPresent.DonorId);
        }

        public async Task<List<Present?>> GetDonorsPresents(int id)
        {
            {
                var presents = await _context.Presents
                    .Where(p => p.DonorId == id)
                    .ToListAsync();
                return presents;
            }
        }
    }
}
