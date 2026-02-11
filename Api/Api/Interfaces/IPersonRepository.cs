using Api.Models;

namespace Api.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPeople();
        Task<Person?> GetPersonById(int id);
        Task Register(Person person);
        Task<Person?> Login(string username);
        Task UpdatePerson(Person person);
        Task DeletePerson(int id);
    }
}
