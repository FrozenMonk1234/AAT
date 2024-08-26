using Assessment3.Models;
using System.Net.Http.Json;

namespace Assessment3.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpClient client;
        private string controller = "User";
        public UserService(IHttpClientFactory clientFactory) => client = clientFactory.CreateClient("Debug");
        public async Task<bool> CreateAsync(User entity)
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

        public async Task<List<User>> GetAllAsync(int? Id = null)
        {
            try
            {
                var result = await client.GetFromJsonAsync<List<User>>($"{controller}/GetAll") ?? [];
                return result;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<User> GetById(int Id)
        {
            try
            {
                var result = await client.GetFromJsonAsync<User>($"{controller}/GetById?Id={Id}") ?? new();
                return result;
            }
            catch (Exception)
            {
                return new();
            }
        }

        public async Task<User> Login(User user)
        {
            try
            {
                var result = await client.PostAsJsonAsync($"{controller}/Update", user);
                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                return await result.Content.ReadFromJsonAsync<User>() ?? new();
            }
            catch (Exception)
            {
                return new();
            }
        }

        public async Task<bool> UpdateAsync(User entity)
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
