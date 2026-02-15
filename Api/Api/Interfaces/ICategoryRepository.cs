using Api.DTOs;
using Api.Models;

namespace Api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
    }
}
