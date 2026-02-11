using Api.Interfaces;
using Api.Models;
using Api.Repositories;

namespace Api.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly ILotteryRepository _lotteryRepository;
        private readonly IPresentRepository _presentRepository;
        public LotteryService(ILotteryRepository lotteryRepository, IPresentRepository presentRepository)
        {
            _lotteryRepository = lotteryRepository;
            _presentRepository = presentRepository;
        }

        public async Task<Present> MakeLottery(int presentId)
        {
            await _lotteryRepository.MakeLottery(presentId);
            var updatedPresent = await _presentRepository.GetPresentById(presentId);
            return updatedPresent;
        }
    }
}
