using Api.Data;
using Api.Interfaces;
using Api.Models;
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
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
