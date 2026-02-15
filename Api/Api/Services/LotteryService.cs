using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly ILotteryRepository _lotteryRepository;
        private readonly ILogger<LotteryService> _logger;

        public LotteryService(ILogger<LotteryService> logger, ILotteryRepository lotteryRepository)
        {
            _lotteryRepository = lotteryRepository;
            _logger = logger;
        }

        public async Task<Present> MakeLottery(int presentId)
        {
            _logger.LogInformation("Starting lottery process for present ID: {PresentId}", presentId);

            try
            {
                var result = await _lotteryRepository.MakeLottery(presentId);

                if (result == null)
                {
                    _logger.LogWarning("Lottery failed for present {PresentId}. Either it was already drawn, or no participants were found.", presentId);
                    throw new Exception("לא ניתן לבצע הגרלה: או שהמתנה כבר הוגרלה, או שלא נמצאו רוכשים.");
                }

                _logger.LogInformation("Lottery completed successfully for present {PresentId}.", presentId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while performing lottery for present {PresentId}", presentId);
                throw;
            }
        }
    }
}