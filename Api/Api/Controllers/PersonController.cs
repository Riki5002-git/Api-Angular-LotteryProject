using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PersonDTOs>>> GetAllPeople()
        {
            var people = await _personService.GetAllPeople();
            return Ok(people);
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] PersonDTOs personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _personService.Register(personDto);
            return CreatedAtAction(nameof(GetPersonById), new { id = personDto.Id }, personDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PersonDTOs>> GetPersonById(int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] PersonDTOs personDto)
        {
            var existingPerson = await _personService.GetPersonById(id);
            if (existingPerson == null)
            {
                return NotFound();
            }
            await _personService.UpdatePerson(id, personDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var existingPerson = await _personService.GetPersonById(id);
            if (existingPerson == null)
            {
                return NotFound();
            }
            await _personService.DeletePerson(id);
            return NoContent();
        }
    }
}
