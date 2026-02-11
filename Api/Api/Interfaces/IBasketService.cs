using Api.Dtos;

namespace Api.Interfaces
{
    public interface IBasketService
    {
        Task AddToBasket(int personId, int presentId);
        Task RemoveFromBasket(int personId, int presentId);
        Task<BasketDTOs?> GetMyBasket(int personId);
        Task ClearItemCompletely(int personId, int presentId);
    }
}
