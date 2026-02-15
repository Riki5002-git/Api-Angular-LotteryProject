using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly LotteryDbContext _context;
        public PersonRepository(LotteryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllPeople()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task Register(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task<Person?> Login(string username)
        {
            return await _context.Persons
                .FirstOrDefaultAsync(p => p.UserName == username);
        }

        public async Task<Person?> GetPersonById(int id)
        {
            return await _context.Persons.FindAsync(id);
        }

        public async Task UpdatePerson(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
    }
}