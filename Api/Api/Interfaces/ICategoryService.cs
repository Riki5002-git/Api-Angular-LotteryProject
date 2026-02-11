using Api.DTOs;

namespace Api.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTOs>> GetAllCategories();
    }
}
