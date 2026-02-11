using Api.DTOs;

namespace Api.Interfaces
{
    public interface IPurchaseService
    {
        Task<List<PurchaseDTOs>> GetAllPurchasesOfPresent(int presentId);
        Task<List<PresentDTOs>> GetSortPresentsByPrice();
        Task<List<PresentDTOs>> GetSortPresentsByPopular();
        Task<List<PurchaseDTOs>> GetAllBuyersOfPresent();
        Task AddPurchase(int personId, int basketId);
    }
}
