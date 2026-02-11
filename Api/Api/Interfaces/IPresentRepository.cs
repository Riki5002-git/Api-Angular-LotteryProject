using Api.Models;

namespace Api.Interfaces
{
    public interface IPresentRepository
    {
        Task<IEnumerable<Present>> GetAllPresents();
        Task AddPresent(Present present);
        Task DeletePresent(int id);
        Task UpdatePresent(Present present);
        Task<Present?> GetPresentById(int id);
        Task<Present?> GetPresentsByPresentName(string name);
        Task<IEnumerable<Present>> GetPresentsByDonorName(string donorName);
        Task<IEnumerable<Present>> GetPresentsByPurchasesAmount(int amount);
        Task<double> GetPresentPrice(int id);
        Task AddPictureUrl(int id, string url);
        Task<Donor> GetDonorsPresent(string PresentName);
    }
}