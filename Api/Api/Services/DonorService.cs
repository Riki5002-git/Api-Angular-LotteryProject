using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        private readonly ILogger<DonorService> _logger;

        public DonorService(IDonorRepository donorRepository, ILogger<DonorService> logger)
        {
            _donorRepository = donorRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<DonorDTOs>> GetAllDonors()
        {
            _logger.LogInformation("Fetching all donors.");
            var donors = await _donorRepository.GetAllDonors();
            return donors.Select(MapToResponseDto);
        }

        public async Task AddDonor(DonorDTOs donorDto)
        {
            _logger.LogInformation("Adding a new donor: {Email}", donorDto.Email);
            var donor = new Donor
            {
                FirstName = donorDto.FirstName,
                LastName = donorDto.LastName,
                Password = donorDto.Password,
                UserName = donorDto.UserName,
                Email = donorDto.Email,
                Phone = donorDto.Phone
            };
            await _donorRepository.AddDonor(donor);
        }

        public async Task<DonorDTOs?> GetDonorById(int id)
        {
            _logger.LogInformation("Fetching donor by ID: {Id}", id);
            var donor = await _donorRepository.GetDonorById(id);
            if (donor == null) throw new Exception("Donor not found");
            return MapToResponseDto(donor);
        }

        public async Task UpdateDonor(int id, DonorDTOs donorDto)
        {
            _logger.LogInformation("Updating donor with ID: {Id}", id);
            var existingDonor = await _donorRepository.GetDonorById(id);
            if (existingDonor == null) throw new Exception("Donor not found for update");

            existingDonor.FirstName = donorDto.FirstName;
            existingDonor.LastName = donorDto.LastName;
            existingDonor.Email = donorDto.Email;
            existingDonor.Phone = donorDto.Phone;
            existingDonor.UserName = donorDto.UserName;
            existingDonor.Password = donorDto.Password;

            await _donorRepository.UpdateDonor(existingDonor);
        }

        public async Task DeleteDonor(int id)
        {
            _logger.LogInformation("Deleting donor with ID: {Id}", id);
            var existingDonor = await _donorRepository.GetDonorById(id);
            if (existingDonor == null) throw new Exception("Donor not found for deletion");

            await _donorRepository.DeleteDonor(id);
        }

        public async Task<DonorDTOs?> GetDonorByName(string firstName, string lastName)
        {
            var donor = await _donorRepository.GetDonorByName(firstName, lastName);
            if (donor == null) throw new Exception("Donor not found");
            return MapToResponseDto(donor);
        }

        public async Task<DonorDTOs?> GetDonorByEmail(string email)
        {
            var donor = await _donorRepository.GetDonorByEmail(email);
            if (donor == null) throw new Exception("Donor not found");
            return MapToResponseDto(donor);
        }

        public async Task<DonorDTOs?> GetDonorByPresent(string presentName)
        {
            var donor = await _donorRepository.GetDonorByPresent(presentName);
            if (donor == null) throw new Exception("Donor not found");
            return MapToResponseDto(donor);
        }

        public async Task<List<PresentDTOs?>> GetDonorsPresents(int id)
        {
            var presents = await _donorRepository.GetDonorsPresents(id);
            return presents.Select(p => p == null ? null : new PresentDTOs
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                PictureUrl = p.PictureUrl,
                PurchasesAmount = p.PurchasesAmount,
                CategoryId = p.CategoryId,
                DonorId = p.DonorId
            }).ToList();
        }

        private static DonorDTOs MapToResponseDto(Donor donor)
        {
            return new DonorDTOs
            {
                Id = donor.Id,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Password = donor.Password,
                UserName = donor.UserName,
                Email = donor.Email,
                Phone = donor.Phone
            };
        }
    }
}