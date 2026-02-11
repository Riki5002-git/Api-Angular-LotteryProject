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
        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DonorDTOs>>> GetAllDonors()
        {
            var donors = await _donorService.GetAllDonors();
            return Ok(donors);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddDonor([FromBody] DonorDTOs donorDto)
        {
            await _donorService.AddDonor(donorDto);
            return CreatedAtAction(nameof(GetDonorById), new { id = donorDto.Id }, donorDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorById(int id)
        {
            var donor = await _donorService.GetDonorById(id);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDonor(int id, [FromBody] DonorDTOs donorDto)
        {
            var existingDonor = await _donorService.GetDonorById(id);
            if (existingDonor == null)
            {
                return NotFound();
            }
            await _donorService.UpdateDonor(id, donorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDonor(int id)
        {
            var existingDonor = await _donorService.GetDonorById(id);
            if (existingDonor == null)
            {
                return NotFound();
            }
            await _donorService.DeleteDonor(id);
            return NoContent();
        }

        [HttpGet("name/{fullName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorByName(string fullName)
        {
            var names = fullName.Split(' ');
            if (names.Length != 2)
            {
                return BadRequest("Full name must include first and last name.");
            }
            var donor = await _donorService.GetDonorByName(names[0], names[1]);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorByEmail(string email)
        {
            var donor = await _donorService.GetDonorByEmail(email);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }

        [HttpGet("present/{presentName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs>> GetDonorByPresent(string presentName)
        {
            var donor = await _donorService.GetDonorByPresent(presentName);
            if (donor == null) return NotFound();
            return Ok(donor);
        }

        [HttpGet("{id}/presents")]
        public async Task<IActionResult> GetDonorsPresents(int id)
        {
            var presents = await _donorService.GetDonorsPresents(id);
            return Ok(presents);
        }
    }
}
