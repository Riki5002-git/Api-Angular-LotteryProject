using Api.Models;

namespace Api.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllPurchasesOfPresent(int presentId);
        Task<List<Present>> GetSortPresentsByPrice();
        Task<List<Present>> GetSortPresentsByPopular();
        Task<List<Purchase>> GetAllBuyersOfPresent();
        Task AddPurchase(int personId, int basketId);
    }
}
