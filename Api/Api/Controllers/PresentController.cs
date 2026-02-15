using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresentController : ControllerBase
    {
        private readonly IPresentService _presentService;
        private readonly ILogger<PresentController> _logger;

        public PresentController(IPresentService presentService, ILogger<PresentController> logger)
        {
            _presentService = presentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresentDTOs>>> GetAllPresents()
        {
            _logger.LogInformation("Request received to fetch all presents.");
            var presents = await _presentService.GetAllPresents();
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
            _logger.LogInformation("Present {Name} added successfully.", presentDto.Name);

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
            _logger.LogInformation("Present with ID {Id} updated successfully.", id);

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
            _logger.LogInformation("Present with ID {Id} deleted successfully.", id);

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