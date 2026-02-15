using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository personRepository, IAuthService authService, ILogger<PersonService> logger)
        {
            _personRepository = personRepository;
            _authService = authService;
            _logger = logger;
        }

        public async Task<IEnumerable<PersonDTOs>> GetAllPeople()
        {
            _logger.LogInformation("Retrieving all people records.");
            var people = await _personRepository.GetAllPeople();
            return people.Select(MapToResponseDto);
        }

        public async Task Register(PersonDTOs personDto)
        {
            _logger.LogInformation("Registering new user: {UserName}", personDto.UserName);
            var person = new Person
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(personDto.Password),
                UserName = personDto.UserName,
                Email = personDto.Email,
                Phone = personDto.Phone
            };
            await _personRepository.Register(person);
        }

        public async Task<string?> LoginAsync(string userName, string password)
        {
            _logger.LogInformation("Login attempt for user: {UserName}", userName);
            var user = await _personRepository.Login(userName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _logger.LogWarning("Failed login attempt for user: {UserName}", userName);
                throw new Exception("שם משתמש או סיסמה שגויים");
            }

            return _authService.GenerateToken(user);
        }

        public async Task<PersonDTOs?> GetPersonById(int id)
        {
            _logger.LogInformation("Fetching person details for ID: {Id}", id);
            var person = await _personRepository.GetPersonById(id);

            if (person == null)
            {
                _logger.LogWarning("Person with ID {Id} not found.", id);
                throw new Exception("משתמש לא נמצא");
            }

            return MapToResponseDto(person);
        }

        public async Task UpdatePerson(int id, PersonDTOs personDto)
        {
            _logger.LogInformation("Updating person with ID: {Id}", id);
            var existingPerson = await _personRepository.GetPersonById(id);

            if (existingPerson == null)
            {
                _logger.LogWarning("Update failed: Person with ID {Id} not found.", id);
                throw new Exception("עדכון נכשל: משתמש לא נמצא");
            }

            existingPerson.FirstName = personDto.FirstName;
            existingPerson.LastName = personDto.LastName;
            existingPerson.Email = personDto.Email;
            existingPerson.Phone = personDto.Phone;
            existingPerson.UserName = personDto.UserName;

            if (!string.IsNullOrEmpty(personDto.Password))
            {
                existingPerson.Password = BCrypt.Net.BCrypt.HashPassword(personDto.Password);
            }

            await _personRepository.UpdatePerson(existingPerson);
        }

        public async Task DeletePerson(int id)
        {
            _logger.LogInformation("Deleting person with ID: {Id}", id);
            var existingPerson = await _personRepository.GetPersonById(id);

            if (existingPerson == null)
            {
                throw new Exception("מחיקה נכשלה: משתמש לא נמצא");
            }

            await _personRepository.DeletePerson(id);
        }

        private static PersonDTOs MapToResponseDto(Person person)
        {
            return new PersonDTOs
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Password = person.Password,
                UserName = person.UserName,
                Email = person.Email,
                Phone = person.Phone
            };
        }
    }
}