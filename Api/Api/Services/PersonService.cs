using Api.DTOs;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAuthService _authService;
        public PersonService(IPersonRepository personRepository, IAuthService authService)
        {
            _personRepository = personRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<PersonDTOs>> GetAllPeople()
        {
            var people = await _personRepository.GetAllPeople();
            return people.Select(MapToResponseDto);
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

        public async Task Register(PersonDTOs personDto)
        {
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
            var user = await _personRepository.Login(userName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return _authService.GenerateToken(user);
        }

        public async Task<PersonDTOs?> GetPersonById(int id)
        {
            var person = await _personRepository.GetPersonById(id);
            if (person == null) return null;
            return MapToResponseDto(person);
        }

        public async Task UpdatePerson(int id, PersonDTOs personDto)
        {
            var existingPerson = await _personRepository.GetPersonById(id);
            if (existingPerson == null) return;
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
            await _personRepository.DeletePerson(id);
        }
    }
}