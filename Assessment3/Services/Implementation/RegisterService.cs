using Assessment3.Enums;
using Assessment3.Models;
using System.Net.Http.Json;

namespace Assessment3.Services.Implementation
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient client;
        private string controller = "Register";
        public RegisterService(IHttpClientFactory clientFactory) => client = clientFactory.CreateClient("Debug");
        public async Task<bool> CreateAsync(Register entity)
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

        public async Task<List<Register>> GetAllAsync(int? Id = null)
        {
            try
            {
                var result = await client.GetFromJsonAsync<List<Register>>($"{controller}/GetAll") ?? [];
                return result;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<Register>> GetAllAsync(int? Id, RegisterEnums.RegisterIdType regsiterIdType)
        {
            try
            {
                var result = await client.GetFromJsonAsync<List<Register>>($"{controller}/GetAll?Id={Id}&Type={regsiterIdType}") ?? [];
                return result;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<Register> GetById(int Id)
        {
            try
            {
                var result = await client.GetFromJsonAsync<Register>($"{controller}/GetById?Id={Id}") ?? new();
                return result;
            }
            catch (Exception)
            {
                return new();
            }
        }

        public async Task<bool> UpdateAsync(Register entity)
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
