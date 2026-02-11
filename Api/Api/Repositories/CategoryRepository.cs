using Api.Data;
using Api.DTOs;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LotteryDbContext _context;
        public CategoryRepository(LotteryDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryDTOs>> GetAllCategories()
        {
            return await _context.Categories
                .Select(c => new CategoryDTOs
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
