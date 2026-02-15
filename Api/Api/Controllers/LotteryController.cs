using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotteryController : ControllerBase
    {
        private readonly ILotteryService _lotteryService;
        private readonly ILogger<LotteryController> _logger;

        public LotteryController(ILotteryService lotteryService, ILogger<LotteryController> logger)
        {
            _lotteryService = lotteryService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MakeLottery(int presentId)
        {
            _logger.LogInformation("Lottery request initiated for present ID: {PresentId}", presentId);
            try
            {
                var result = await _lotteryService.MakeLottery(presentId);
                if (result == null)
                {
                    _logger.LogWarning("Lottery failed: Present with ID {PresentId} not found", presentId);
                    return NotFound("מתנה לא נמצאה");
                }

                _logger.LogInformation("Lottery completed successfully for present ID: {PresentId}", presentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during lottery for present ID: {PresentId}", presentId);
                return StatusCode(500, ex.Message);
            }
        }
    }
}