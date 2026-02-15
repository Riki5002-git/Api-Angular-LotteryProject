using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            _logger.LogInformation("Attempting to fetch all categories.");

            var categories = await _categoryRepository.GetAllCategories();

            if (categories == null || !categories.Any())
            {
                _logger.LogWarning("No categories were found.");
                return new List<Category>();
            }

            _logger.LogInformation("Successfully retrieved {Count} categories.", categories.Count);

            return categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}