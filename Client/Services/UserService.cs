using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Client.Services
{
    public interface IUserService
    {
        ValueTask<ShopUser> GetMeAsync();
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
            => _httpClient = httpClient;

        public async ValueTask<ShopUser> GetMeAsync()
        {
            var response = await _httpClient.GetAsync("api/user/me");

            return await response.Content.ReadFromJsonAsync<ShopUser>();
        }
    }
}
