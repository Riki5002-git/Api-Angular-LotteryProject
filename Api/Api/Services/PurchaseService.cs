using Api.DTOs;
using Api.Interfaces;

namespace Api.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<List<PurchaseDTOs>> GetAllBuyersOfPresent()
        {
            var purchases = await _purchaseRepository.GetAllBuyersOfPresent();

            return purchases.Select(p => new PurchaseDTOs
            {
                Id = p.Id,
                Person = p.Person,
                Present = p.Present,
                PurchaseDate = p.PurchaseDate
            }).ToList();
        }

        public async Task<List<PurchaseDTOs>> GetAllPurchasesOfPresent(int presentId)
        {
            var purchases = await _purchaseRepository.GetAllPurchasesOfPresent(presentId);
            return purchases.Select(MapToDTO).ToList();
        }

        public async Task<List<PresentDTOs>> GetSortPresentsByPopular()
        {
            var presents = await _purchaseRepository.GetSortPresentsByPopular();
            return presents.Select(present => new PresentDTOs
            {
                Id = present.Id,
                Name = present.Name,
                Description = present.Description,
                Price = present.Price,
                PurchasesAmount = present.PurchasesAmount,
                PictureUrl = present.PictureUrl
            }).ToList();
        }

        public async Task<List<PresentDTOs>> GetSortPresentsByPrice()
        {
            var presents = await _purchaseRepository.GetSortPresentsByPrice();
            return presents.Select(present => new PresentDTOs
            {
                Id = present.Id,
                Name = present.Name,
                Description = present.Description,
                Price = present.Price,
                PurchasesAmount = present.PurchasesAmount,
                PictureUrl = present.PictureUrl
            }).ToList();
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

        public async Task AddPurchase(int personId, int basketId)
        {
            await _purchaseRepository.AddPurchase(personId, basketId);
        }
    }
}
