using Api.Models;

namespace Api.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
    }
}
