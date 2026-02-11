using Api.DTOs;

namespace Api.Interfaces
{
    public interface IPresentService
    {
        Task<List<PresentDTOs>> GetAllPresents();
        Task<PresentDTOs?> GetPresentById(int id);
        Task AddPresent(PresentDTOs presentDTO);
        Task UpdatePresent(int id, PresentDTOs presentDTO);
        Task DeletePresent(int id);
        Task<PresentDTOs?> GetPresentsByPresentName(string name);
        Task<IEnumerable<PresentDTOs>> GetPresentsByDonorName(string donorName);
        Task<IEnumerable<PresentDTOs>> GetPresentsByPurchasesAmount(int amount);
        Task<double> GetPresentPrice(int id);
        Task AddPictureUrl(int id, string url);
        Task<DonorDTOs?> GetDonorsPresent(string PresentName);
    }
}
