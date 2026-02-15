using Api.Models;

namespace Api.Interfaces
{
    public interface IBasketRepository
    {
        Task AddToBasket(int personId, int presentId);
        Task RemoveFromBasket(int personId, int presentId);
        Task<Basket?> GetMyBasket(int personId);
        Task ClearItemCompletely(int personId, int presentId);
    }
}
