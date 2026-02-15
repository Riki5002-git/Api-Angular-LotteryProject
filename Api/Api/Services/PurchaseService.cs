using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(IPurchaseRepository purchaseRepository, IPersonRepository personRepository, ILogger<PurchaseService> logger)
        {
            _purchaseRepository = purchaseRepository;
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<List<PurchaseDTOs>> GetAllBuyersOfPresent()
        {
            _logger.LogInformation("Fetching all buyers of presents.");
            var purchases = await _purchaseRepository.GetAllBuyersOfPresent();
            return purchases.Select(MapToDTO).ToList();
        }

        public async Task<List<PurchaseDTOs>> GetAllPurchasesOfPresent(int presentId)
        {
            _logger.LogInformation("Fetching all purchases for present ID: {PresentId}", presentId);
            var purchases = await _purchaseRepository.GetAllPurchasesOfPresent(presentId);
            return purchases.Select(MapToDTO).ToList();
        }

        public async Task<List<PresentDTOs>> GetSortPresentsByPopular()
        {
            _logger.LogInformation("Fetching presents sorted by popularity.");
            var presents = await _purchaseRepository.GetSortPresentsByPopular();
            return presents.Select(p => MapPresentToDTO(p)).ToList();
        }

        public async Task<List<PresentDTOs>> GetSortPresentsByPrice()
        {
            _logger.LogInformation("Fetching presents sorted by price.");
            var presents = await _purchaseRepository.GetSortPresentsByPrice();
            return presents.Select(p => MapPresentToDTO(p)).ToList();
        }

        public async Task AddPurchase(int personId, int basketId)
        {
            _logger.LogInformation("Attempting to process purchase for person ID: {PersonId}", personId);
            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                _logger.LogError("Purchase failed: User {Id} not found", personId);
                throw new Exception("המשתמש לא נמצא במערכת.");
            }

            await _purchaseRepository.AddPurchase(personId, basketId);
            _logger.LogInformation("Purchase completed successfully for person ID: {PersonId}", personId);
        }

        private static PurchaseDTOs MapToDTO(Purchase purchase)
        {
            return new PurchaseDTOs
            {
                Id = purchase.Id,
                Person = purchase.Person,
                Present = purchase.Present,
                PurchaseDate = purchase.PurchaseDate
            };
        }

        private static PresentDTOs MapPresentToDTO(Present present)
        {
            return new PresentDTOs
            {
                Id = present.Id,
                Name = present.Name,
                Description = present.Description,
                Price = present.Price,
                PurchasesAmount = present.PurchasesAmount,
                PictureUrl = present.PictureUrl
            };
        }
    }
}