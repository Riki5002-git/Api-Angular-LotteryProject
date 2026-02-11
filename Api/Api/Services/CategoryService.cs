using Api.DTOs;
using Api.Interfaces;

namespace Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDTOs>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return categories.Select(c => new CategoryDTOs
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}