using Api.Dtos;
using Api.Interfaces;

namespace Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public async Task AddToBasket(int personId, int presentId)
        {
            await _basketRepository.AddToBasket(personId, presentId);
        }

        public async Task RemoveFromBasket(int personId, int presentId)
        {
            await _basketRepository.RemoveFromBasket(personId, presentId);
        }

        public async Task<BasketDTOs?> GetMyBasket(int personId)
        {
            var basket = await _basketRepository.GetMyBasket(personId);
            return basket;
        }

        public async Task ClearItemCompletely(int personId, int presentId)
        {
            await _basketRepository.ClearItemCompletely(personId, presentId);
        }
    }
}
