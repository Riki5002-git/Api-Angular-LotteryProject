using Api.Models;

namespace Api.Interfaces
{
    public interface ILotteryRepository
    {
        Task<Present> MakeLottery(int presentId);
    }
}
