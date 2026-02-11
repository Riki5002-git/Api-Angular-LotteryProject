using Api.Models;

namespace Api.Interfaces
{
    public interface ILotteryService
    {
        Task<Present> MakeLottery(int presentId);
    }
}
