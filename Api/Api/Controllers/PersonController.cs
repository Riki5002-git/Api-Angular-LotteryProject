using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonService personService, ILogger<PersonController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PersonDTOs>>> GetAllPeople()
        {
            _logger.LogInformation("Request received to fetch all people.");
            var people = await _personService.GetAllPeople();
            return Ok(people);
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] PersonDTOs personDto)
        {
            _logger.LogInformation("Request received to register a new user: {UserName}", personDto.UserName);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration failed due to invalid model state for user: {UserName}", personDto.UserName);
                return BadRequest(ModelState);
            }

            await _personService.Register(personDto);
            _logger.LogInformation("User {UserName} registered successfully.", personDto.UserName);

            return CreatedAtAction(nameof(GetPersonById), new { id = personDto.Id }, personDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PersonDTOs>> GetPersonById(int id)
        {
            _logger.LogInformation("Request received to fetch person with ID: {Id}", id);
            var person = await _personService.GetPersonById(id);

            if (person == null)
            {
                _logger.LogWarning("Person with ID {Id} was not found.", id);
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] PersonDTOs personDto)
        {
            _logger.LogInformation("Request received to update person with ID: {Id}", id);
            var existingPerson = await _personService.GetPersonById(id);

            if (existingPerson == null)
            {
                _logger.LogWarning("Update failed: Person with ID {Id} not found.", id);
                return NotFound();
            }

            await _personService.UpdatePerson(id, personDto);
            _logger.LogInformation("Person with ID {Id} updated successfully.", id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            _logger.LogInformation("Request received to delete person with ID: {Id}", id);
            var existingPerson = await _personService.GetPersonById(id);

            if (existingPerson == null)
            {
                _logger.LogWarning("Delete failed: Person with ID {Id} not found.", id);
                return NotFound();
            }

            await _personService.DeletePerson(id);
            _logger.LogInformation("Person with ID {Id} deleted successfully.", id);

            return NoContent();
        }
    }
}