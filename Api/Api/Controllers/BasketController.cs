using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketController> _logger;
    public BasketController(IBasketService basketService, ILogger<BasketController> logger)
    {
        _basketService = basketService;
        _logger = logger;
    }

    [HttpPost("addToBasket/{personId}/{presentId}")]
    public async Task<ActionResult> AddToBasket(int personId, int presentId)
    {
        _logger.LogInformation("Received request to add present {PresentId} to basket for person {PersonId}", presentId, personId);

        try
        {
            await _basketService.AddToBasket(personId, presentId);
            return Ok(new { message = "Successfully added", id = presentId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding present {PresentId} to basket for person {PersonId}", presentId, personId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("removeFromBasket/{personId}/{presentId}")]
    public async Task<ActionResult> RemoveFromBasket(int personId, int presentId)
    {
        _logger.LogInformation("Received request to remove unit of present {PresentId} from basket for person {PersonId}", presentId, personId);
        await _basketService.RemoveFromBasket(personId, presentId);
        return Ok();
    }

    [HttpGet("getMyBasket/{personId}")]
    public async Task<ActionResult<BasketDTOs>> GetMyBasket(int personId)
    {
        _logger.LogInformation("User {PersonId} is requesting their basket content", personId);
        var basket = await _basketService.GetMyBasket(personId);
        if (basket == null)
        {
            _logger.LogWarning("No basket found for person {PersonId}. Returning empty basket.", personId);
            return Ok(new BasketDTOs { Presents = new List<BasketItem>() });
        }
        return Ok(basket);
    }

    [HttpDelete("clearItemCompletely/{personId}/{presentId}")]
    public async Task<ActionResult> ClearItemCompletely(int personId, int presentId)
    {
        _logger.LogInformation("Force clearing present {PresentId} from person {PersonId}'s basket", presentId, personId);
        try
        {
            await _basketService.ClearItemCompletely(personId, presentId);
            return Ok(new { message = "Item removed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clear item {PresentId} for person {PersonId}", presentId, personId);
            return StatusCode(500, "Internal server error");
        }
    }
}