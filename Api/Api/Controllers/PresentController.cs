using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresentController : ControllerBase
    {
        private readonly IPresentService _presentService;

        public PresentController(IPresentService presentService)
        {
            _presentService = presentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresentDTOs>>> GetAllPresents()
        {
            var presents = await _presentService.GetAllPresents();
            return Ok(presents);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddPresent([FromBody] PresentDTOs presentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _presentService.AddPresent(presentDto);
            return CreatedAtAction(nameof(GetPresentById), new { id = presentDto.Id }, presentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PresentDTOs>> GetPresentById(int id)
        {
            var present = await _presentService.GetPresentById(id);
            if (present == null)
            {
                return NotFound();
            }
            return Ok(present);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatePresent(int id, [FromBody] PresentDTOs presentDto)
        {
            var existingPresent = await _presentService.GetPresentById(id);
            if (existingPresent == null)
            {
                return NotFound();
            }
            await _presentService.UpdatePresent(id, presentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePresent(int id)
        {
            var existingPresent = await _presentService.GetPresentById(id);
            if (existingPresent == null)
            {
                return NotFound();
            }
            await _presentService.DeletePresent(id);
            return NoContent();
        }

        [HttpGet("byname/{name}")]
        public async Task<ActionResult<PresentDTOs>> GetPresentsByPresentName(string name)
        {
            var present = await _presentService.GetPresentsByPresentName(name);
            if (present == null)
            {
                return NotFound();
            }
            return Ok(present);
        }

        [HttpGet("bydonor/{donorName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PresentDTOs>> GetPresentsByDonorName(string donorName)
        {
            var present = await _presentService.GetPresentsByDonorName(donorName);
            if (present == null)
            {
                return NotFound();
            }
            return Ok(present);
        }

        [HttpGet("byamount/{amount}")]
        public async Task<ActionResult<PresentDTOs>> GetPresentsByPurchasesAmount(int amount)
        {
            var present = await _presentService.GetPresentsByPurchasesAmount(amount);
            if (present == null)
            {
                return NotFound();
            }
            return Ok(present);
        }

        [HttpGet("price/{id}")]
        public async Task<ActionResult<int?>> GetPresentPrice(int id)
        {
            var price = await _presentService.GetPresentPrice(id);
            if (price == null)
            {
                return NotFound();
            }
            return Ok(price);
        }

        [HttpPost("add-picture/{presentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddPictureUrl(int presentId, string pictureUrl)
        {
            await _presentService.AddPictureUrl(presentId, pictureUrl);
            return NoContent();
        }

        [HttpGet("donor-of/{presentName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DonorDTOs?>> GetDonorsPresent(string presentName)
        {
            var donor = await _presentService.GetDonorsPresent(presentName);
            if (donor == null)
            {
                return NotFound();
            }
            return Ok(donor);
        }
    }
}
