using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresentController : ControllerBase
    {
        private readonly IPresentService _presentService;
        private readonly ILogger<PresentController> _logger;
        private readonly IDistributedCache _cache;

        public PresentController(IPresentService presentService, ILogger<PresentController> logger, IDistributedCache cache)
        {
            _presentService = presentService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresentDTOs>>> GetAllPresents()
        {
            string cacheKey = "all_presents_list";
            _logger.LogInformation("Request received to fetch all presents.");

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Cache HIT: Fetching presents from Redis.");
                var cachedPresents = JsonSerializer.Deserialize<IEnumerable<PresentDTOs>>(cachedData);
                return Ok(cachedPresents);
            }

            _logger.LogInformation("Cache MISS: Fetching presents from Database.");
            var presents = await _presentService.GetAllPresents();

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            var serializedData = JsonSerializer.Serialize(presents);
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

            return Ok(presents);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddPresent([FromBody] PresentDTOs presentDto)
        {
            _logger.LogInformation("Request to add a new present: {Name}", presentDto.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for adding present: {Name}", presentDto.Name);
                return BadRequest(ModelState);
            }

            await _presentService.AddPresent(presentDto);
            
            await _cache.RemoveAsync("all_presents_list");
            _logger.LogInformation("Present {Name} added successfully. Cache cleared.", presentDto.Name);

            return CreatedAtAction(nameof(GetPresentById), new { id = presentDto.Id }, presentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PresentDTOs>> GetPresentById(int id)
        {
            _logger.LogInformation("Request to fetch present by ID: {Id}", id);
            var present = await _presentService.GetPresentById(id);

            if (present == null)
            {
                _logger.LogWarning("Present with ID {Id} not found.", id);
                return NotFound();
            }

            return Ok(present);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatePresent(int id, [FromBody] PresentDTOs presentDto)
        {
            _logger.LogInformation("Request to update present with ID: {Id}", id);
            var existingPresent = await _presentService.GetPresentById(id);

            if (existingPresent == null)
            {
                _logger.LogWarning("Update failed: Present with ID {Id} not found.", id);
                return NotFound();
            }

            await _presentService.UpdatePresent(id, presentDto);
            
            await _cache.RemoveAsync("all_presents_list");

            _logger.LogInformation("Present with ID {Id} updated successfully. Cache cleared.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePresent(int id)
        {
            _logger.LogInformation("Request to delete present with ID: {Id}", id);
            var existingPresent = await _presentService.GetPresentById(id);

            if (existingPresent == null)
            {
                _logger.LogWarning("Delete failed: Present with ID {Id} not found.", id);
                return NotFound();
            }

            await _presentService.DeletePresent(id);
            
            await _cache.RemoveAsync("all_presents_list");

            _logger.LogInformation("Present with ID {Id} deleted successfully. Cache cleared.", id);
            return NoContent();
        }

        [HttpGet("byname/{name}")]
        public async Task<ActionResult<PresentDTOs>> GetPresentsByPresentName(string name)
        {
            _logger.LogInformation("Request to fetch present by name: {Name}", name);
            var present = await _presentService.GetPresentsByPresentName(name);

            if (present == null)
            {
                _logger.LogWarning("Present with name {Name} not found.", name);
                return NotFound();
            }

            return Ok(present);
        }

        [HttpGet("bydonor/{donorName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PresentDTOs>> GetPresentsByDonorName(string donorName)
        {
            _logger.LogInformation("Request to fetch presents by donor: {DonorName}", donorName);
            var present = await _presentService.GetPresentsByDonorName(donorName);

            if (present == null)
            {
                _logger.LogWarning("No presents found for donor: {DonorName}", donorName);
                return NotFound();
            }

            return Ok(present);
        }

        [HttpGet("byamount/{amount}")]
        public async Task<ActionResult<PresentDTOs>> GetPresentsByPurchasesAmount(int amount)
        {
            _logger.LogInformation("Request to fetch presents with purchase amount: {Amount}", amount);
            var present = await _presentService.GetPresentsByPurchasesAmount(amount);

            if (present == null)
            {
                _logger.LogWarning("No presents found with purchase amount: {Amount}", amount);
                return NotFound();
            }

            return Ok(present);
        }

        [HttpGet("price/{id}")]
        public async Task<ActionResult<int?>> GetPresentPrice(int id)
        {
            _logger.LogInformation("Request to fetch price for present ID: {Id}", id);
            var price = await _presentService.GetPresentPrice(id);

            if (price == null)
            {
                _logger.LogWarning("Price not found for present ID: {Id}", id);
                return NotFound();
            }

            return Ok(price);
        }

        [HttpPost("add-picture/{presentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddPictureUrl(int presentId, string pictureUrl)
        {
            _logger.LogInformation("Request to add picture to present ID: {Id}", presentId);
            await _presentService.AddPictureUrl(presentId, pictureUrl);
            
            await _cache.RemoveAsync("all_presents_list");
            
            return NoContent();
        }

        [HttpGet("donor-of/{presentName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs?>> GetDonorsPresent(string presentName)
        {
            _logger.LogInformation("Request to fetch donor of present: {PresentName}", presentName);
            var donor = await _presentService.GetDonorsPresent(presentName);

            if (donor == null)
            {
                _logger.LogWarning("Donor not found for present: {PresentName}", presentName);
                return NotFound();
            }

            return Ok(donor);
        }
    }
}