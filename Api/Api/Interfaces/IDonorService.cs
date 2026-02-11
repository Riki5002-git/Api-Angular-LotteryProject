using Api.DTOs;
using Api.Models;

namespace Api.Interfaces
{
    public interface IDonorService
    {
        Task<IEnumerable<DonorDTOs>> GetAllDonors();
        Task AddDonor(DonorDTOs donorDto);
        Task<DonorDTOs?> GetDonorById(int id);
        Task UpdateDonor(int id, DonorDTOs donorDto);
        Task DeleteDonor(int id);
        Task<DonorDTOs?> GetDonorByName(string firstName, string lastName);
        Task<DonorDTOs?> GetDonorByEmail(string email);
        Task<DonorDTOs?> GetDonorByPresent(string present);
        Task<List<PresentDTOs?>> GetDonorsPresents(int id);
    }
}
