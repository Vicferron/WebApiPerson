using SharedLibrary.Models;

namespace WebPerson.Client.Services
{
    public interface IAPIController
    {
        Task AddPersonAsync(Person person);
        Task DeletePersonAsync(int id);
        Task<Person> GetPersonAsync(int id);
        Task<List<Person>> GetPersonsAsync();
        Task UpdatePersonAsync(Person person);
    }
}