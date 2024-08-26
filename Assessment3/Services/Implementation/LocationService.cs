using Assessment3.Models;
using System.Net.Http.Json;

namespace Assessment3.Services.Implementation
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient client;
        private string controller = "Location";
        public LocationService(IHttpClientFactory clientFactory) => client = clientFactory.CreateClient("Debug");
        public async Task<bool> CreateAsync(Location entity)
        {
            try
            {
                var result = await client.PostAsJsonAsync($"{controller}/Create", entity);
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return await result.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            try
            {
                var result = await client.GetFromJsonAsync<bool>($"{controller}/DeleteById?Id={Id}");
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Location>> GetAllAsync(int? Id = null)
        {
            try
            {
                var result = await client.GetFromJsonAsync<List<Location>>($"{controller}/GetAll") ?? [];
                return result;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<Location> GetById(int Id)
        {
            try
            {
                var result = await client.GetFromJsonAsync<Location>($"{controller}/GetById?Id={Id}") ?? new();
                return result;
            }
            catch (Exception)
            {
                return new();
            }
        }

        public async Task<bool> UpdateAsync(Location entity)
        {
            try
            {
                var result = await client.PostAsJsonAsync($"{controller}/Update", entity);
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return await result.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
