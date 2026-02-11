using Api.DTOs;

namespace Api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDTOs>> GetAllCategories();
    }
}
