using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet("buyers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PersonDTOs>>> GetAllBuyersOfPresent()
        {
            var buyers = await _purchaseService.GetAllBuyersOfPresent();
            return Ok(buyers);
        }

        [HttpGet("purchases/{presentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PurchaseDTOs>>> GetAllPurchasesOfPresent(int presentId)
        {
            var purchases = await _purchaseService.GetAllPurchasesOfPresent(presentId);
            return Ok(purchases);
        }

        [HttpGet("presents/sorted-by-price")]
        public async Task<ActionResult<IEnumerable<PresentDTOs>>> GetSortPresentsByPrice()
        {
            var presents = await _purchaseService.GetSortPresentsByPrice();
            return Ok(presents);
        }

        [HttpGet("presents/sorted-by-popular")]
        public async Task<ActionResult<IEnumerable<PresentDTOs>>> GetSortPresentsByPopular()
        {
            var presents = await _purchaseService.GetSortPresentsByPopular();
            return Ok(presents);
        }

        [HttpPost("addPurchase/{personId}/{basketId}")]
        public async Task<IActionResult> AddPurchase(int personId, int basketId)
        {
            await _purchaseService.AddPurchase(personId, basketId);
            return Ok(new { message = "Purchase added successfully" });
        }
    }
}
