using SharedLibrary.Models;
using System.Net;
using System.Net.Http;

namespace WebPerson.Client.Services
{
    public class APIController(HttpClient httpClient, IConfiguration config) : IAPIController
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string baseUrl = config["ApiSettings:BaseUrl"];

        // GET: api/Person
        public async Task<List<Person>> GetPersonsAsync()
        {
            var response = await _httpClient.GetAsync($"{baseUrl}api/Person");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Person>>();
            }
            else
            {
                throw new HttpRequestException($"Error retrieving persons: {response.ReasonPhrase}");
            }
        }

        // GET: api/Person/5
        public async Task<Person> GetPersonAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{baseUrl}api/Person/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Person>();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new HttpRequestException($"Person not found");
            }
            else
            {
                throw new Exception($"Error retrieving person: {response.ReasonPhrase}");
            }
        }

        // PUT: api/Person/5
        public async Task UpdatePersonAsync(Person person)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}api/Person/{person.Id}", person);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error updating person: {response.ReasonPhrase}");
            }
        }

        // POST: api/Person
        public async Task AddPersonAsync(Person person)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}api/Person", person);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding person: {response.ReasonPhrase}");
            }
        }

        // DELETE: api/Person/5
        public async Task DeletePersonAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}api/Person/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error deleting person: {response.ReasonPhrase}");
            }
        }
    }
}
