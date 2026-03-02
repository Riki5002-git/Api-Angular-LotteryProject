using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Api.Services
{
    public class PresentService : IPresentService
    {
        private readonly IPresentRepository _presentRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly ILogger<PresentService> _logger;
        private readonly IDistributedCache _cache;

        public PresentService(IPresentRepository presentRepository, IDonorRepository donorRepository, ILogger<PresentService> logger, IDistributedCache cache)
        {
            _presentRepository = presentRepository;
            _donorRepository = donorRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task AddPictureUrl(int id, string url)
        {
            _logger.LogInformation("Updating picture URL for present ID: {Id}", id);
            await _presentRepository.AddPictureUrl(id, url);
        }

        public async Task AddPresent(PresentDTOs presentDTO)
        {
            _logger.LogInformation("Adding new present: {Name}", presentDTO.Name);
            await _presentRepository.AddPresent(new Present
            {
                Name = presentDTO.Name,
                Description = presentDTO.Description,
                Price = presentDTO.Price,
                PurchasesAmount = presentDTO.PurchasesAmount,
                DonorId = presentDTO.DonorId,
                CategoryId = presentDTO.CategoryId,
                PictureUrl = presentDTO.PictureUrl
            });
        }

        public async Task DeletePresent(int id)
        {
            _logger.LogInformation("Attempting to delete present ID: {Id}", id);
            var existingPresent = await _presentRepository.GetPresentById(id);
            if (existingPresent == null)
            {
                _logger.LogWarning("Delete failed: Present ID {Id} not found", id);
                throw new Exception("Present not found");
            }
            await _presentRepository.DeletePresent(id);
        }

        public async Task<List<PresentDTOs>> GetAllPresents()
        {
            string cacheKey = "all-presents";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("--- Cache HIT: Fetching from Redis ---");
                return JsonSerializer.Deserialize<List<PresentDTOs>>(cachedData);
            }

            _logger.LogInformation("--- Cache MISS: Fetching from Database ---");
            var presents = await _presentRepository.GetAllPresents();
            var results = presents.Select(MapToResponseDto).ToList();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
            };

            var serializedData = JsonSerializer.Serialize(results);
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

            return results;
        }

        public async Task<DonorDTOs?> GetDonorsPresent(string PresentName)
        {
            _logger.LogInformation("Fetching donor for present: {PresentName}", PresentName);
            var donor = await _presentRepository.GetDonorsPresent(PresentName);
            if (donor == null) return null;

            return new DonorDTOs
            {
                Id = donor.Id,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Email = donor.Email,
                Phone = donor.Phone
            };
        }

        public async Task<PresentDTOs?> GetPresentById(int id)
        {
            _logger.LogInformation("Fetching present details for ID: {Id}", id);
            var present = await _presentRepository.GetPresentById(id);
            if (present == null) throw new Exception("Present not found");
            return MapToResponseDto(present);
        }

        public async Task<double> GetPresentPrice(int id)
        {
            var present = await _presentRepository.GetPresentById(id);
            if (present == null) throw new Exception("Present not found");
            return present.Price;
        }

        public async Task<IEnumerable<PresentDTOs>> GetPresentsByDonorName(string donorName)
        {
            _logger.LogInformation("Searching presents for donor: {DonorName}", donorName);
            var presents = await _presentRepository.GetPresentsByDonorName(donorName);
            if (!presents.Any())
            {
                _logger.LogWarning("No presents found for donor: {DonorName}", donorName);
            }
            return presents.Select(MapToResponseDto);
        }

        public async Task<PresentDTOs?> GetPresentsByPresentName(string name)
        {
            var present = await _presentRepository.GetPresentsByPresentName(name);
            if (present == null) throw new Exception("Present not found");
            return MapToResponseDto(present);
        }

        public async Task<IEnumerable<PresentDTOs>> GetPresentsByPurchasesAmount(int amount)
        {
            var presents = await _presentRepository.GetPresentsByPurchasesAmount(amount);
            return presents.Select(MapToResponseDto);
        }

        public async Task UpdatePresent(int id, PresentDTOs presentDTO)
        {
            var existingPresent = await _presentRepository.GetPresentById(id);
            if (existingPresent == null) throw new Exception("Present not found for update");

            existingPresent.Name = presentDTO.Name;
            existingPresent.Description = presentDTO.Description;
            existingPresent.Price = presentDTO.Price;
            existingPresent.PurchasesAmount = presentDTO.PurchasesAmount;
            existingPresent.DonorId = presentDTO.DonorId;
            existingPresent.CategoryId = presentDTO.CategoryId;
            existingPresent.PictureUrl = presentDTO.PictureUrl;

            await _presentRepository.UpdatePresent(existingPresent);
        }

        private static PresentDTOs MapToResponseDto(Present present)
        {
            return new PresentDTOs
            {
                Id = present.Id,
                Name = present.Name,
                Description = present.Description,
                DonorId = present.DonorId,
                Price = present.Price,
                PictureUrl = present.PictureUrl,
                CategoryId = present.CategoryId,
                PurchasesAmount = present.PurchasesAmount,
                WinnerId = present.WinnerId,
                Winner = present.Winner
            };
        }
    }
}