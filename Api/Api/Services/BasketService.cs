using Api.Dtos;
using Api.Interfaces;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPresentRepository _presentRepository;
        private readonly ILogger<BasketService> _logger;

        public BasketService(IBasketRepository basketRepository, IPresentRepository presentRepository, ILogger<BasketService> logger)
        {
            _basketRepository = basketRepository;
            _presentRepository = presentRepository;
            _logger = logger;
        }

        public async Task AddToBasket(int personId, int presentId)
        {
            _logger.LogInformation("Adding item {PresentId} to basket for person {PersonId}", presentId, personId);

            var present = await _presentRepository.GetPresentById(presentId);
            if (present == null)
            {
                _logger.LogWarning("Add to basket failed: Present {PresentId} not found", presentId);
                throw new Exception("Present not found");
            }

            await _basketRepository.AddToBasket(personId, presentId);
        }

        public async Task RemoveFromBasket(int personId, int presentId)
        {
            _logger.LogInformation("Removing item {PresentId} from basket for person {PersonId}", presentId, personId);

            var basket = await _basketRepository.GetMyBasket(personId);
            if (basket == null)
            {
                _logger.LogWarning("Remove failed: Basket not found for person {PersonId}", personId);
                throw new Exception("Basket not found");
            }

            await _basketRepository.RemoveFromBasket(personId, presentId);
        }

        public async Task<BasketDTOs?> GetMyBasket(int personId)
        {
            _logger.LogInformation("Fetching basket details for person {PersonId}", personId);
            var basket = await _basketRepository.GetMyBasket(personId);
            if (basket == null) return null;
            return new BasketDTOs
            {
                Id = basket.Id,
                PersonId = basket.PersonId,
                Presents = basket.Presents?.Select(bi => new Dtos.BasketItem
                {
                    Id = bi.Id,
                    PresentId = bi.PresentId,
                    Quantity = bi.Quantity,
                    Present = bi.Present
                }).ToList()
            };
        }

        public async Task ClearItemCompletely(int personId, int presentId)
        {
            _logger.LogInformation("Clearing item {PresentId} completely from basket for person {PersonId}", presentId, personId);
            await _basketRepository.ClearItemCompletely(personId, presentId);
        }
    }
}