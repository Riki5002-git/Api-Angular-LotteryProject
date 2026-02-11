using Api.DTOs;

namespace Api.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDTOs>> GetAllPeople();

        Task<PersonDTOs?> GetPersonById(int id);

        Task Register(PersonDTOs personDto);
        Task<string> LoginAsync(string username, string password);

        Task UpdatePerson(int id, PersonDTOs personDto);

        Task DeletePerson(int id);
    }
}