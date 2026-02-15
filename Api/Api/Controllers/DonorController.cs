using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly ILogger<DonorController> _logger;
        public DonorController(IDonorService donorService, ILogger<DonorController> logger)
        {
            _donorService = donorService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DonorDTOs>>> GetAllDonors()
        {
            _logger.LogInformation("Request to get all donors received.");
            var donors = await _donorService.GetAllDonors();
            return Ok(donors);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddDonor([FromBody] DonorDTOs donorDto)
        {
            _logger.LogInformation("Request to add a new donor: {Email}", donorDto.Email);
            await _donorService.AddDonor(donorDto);
            return CreatedAtAction(nameof(GetDonorById), new { id = donorDto.Id }, donorDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorById(int id)
        {
            _logger.LogInformation("Request to get donor by ID: {Id}", id);
            var donor = await _donorService.GetDonorById(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDonor(int id, [FromBody] DonorDTOs donorDto)
        {
            _logger.LogInformation("Request to update donor with ID: {Id}", id);
            var existingDonor = await _donorService.GetDonorById(id);
            if (existingDonor == null)
            {
                _logger.LogWarning("Update failed: Donor with ID {Id} not found.", id);
                return NotFound();
            }
            await _donorService.UpdateDonor(id, donorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDonor(int id)
        {
            _logger.LogInformation("Request to delete donor with ID: {Id}", id);
            var existingDonor = await _donorService.GetDonorById(id);
            if (existingDonor == null)
            {
                _logger.LogWarning("Delete failed: Donor with ID {Id} not found.", id);
                return NotFound();
            }
            await _donorService.DeleteDonor(id);
            return NoContent();
        }

        [HttpGet("name/{fullName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorByName(string fullName)
        {
            _logger.LogInformation("Request to get donor by name: {FullName}", fullName);
            var names = fullName.Split(' ');
            if (names.Length != 2)
            {
                _logger.LogWarning("Invalid name format provided: {FullName}", fullName);
                return BadRequest("Full name must include first and last name.");
            }
            var donor = await _donorService.GetDonorByName(names[0], names[1]);
            if (donor == null)
            {
                _logger.LogWarning("Donor with name {FullName} not found.", fullName);
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorByEmail(string email)
        {
            _logger.LogInformation("Request to get donor by email: {Email}", email);
            var donor = await _donorService.GetDonorByEmail(email);
            if (donor == null)
            {
                _logger.LogWarning("Donor with email {Email} not found.", email);
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpGet("present/{presentName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorByPresent(string presentName)
        {
            _logger.LogInformation("Request to get donor by present: {PresentName}", presentName);
            var donor = await _donorService.GetDonorByPresent(presentName);
            if (donor == null)
            {
                _logger.LogWarning("No donor found for present: {PresentName}", presentName);
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpGet("{id}/presents")]
        public async Task<IActionResult> GetDonorsPresents(int id)
        {
            _logger.LogInformation("Request to get presents for donor ID: {Id}", id);
            var presents = await _donorService.GetDonorsPresents(id);
            return Ok(presents);
        }
    }
}