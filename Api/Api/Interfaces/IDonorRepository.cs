using Api.Models;

namespace Api.Interfaces
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetAllDonors();
        Task AddDonor(Donor donor);
        Task<Donor?> GetDonorById(int id);
        Task UpdateDonor(Donor donor);
        Task DeleteDonor(int id);
        Task<Donor?> GetDonorByName(string firstName, string lastName);
        Task<Donor?> GetDonorByEmail(string email);
        Task<Donor?> GetDonorByPresent(string present);
        Task<List<Present?>> GetDonorsPresents(int id);
    }
}
