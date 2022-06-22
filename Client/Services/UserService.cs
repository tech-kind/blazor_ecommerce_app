using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Shared.Entities;
using BlazorECommerceApp.Client.Extensions;

namespace BlazorECommerceApp.Client.Services
{
    public interface IUserService
    {
        ValueTask<ShopUser> GetMeAsync();
        ValueTask PutAsync(ShopUser user);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
            => _httpClient = httpClient;

        public async ValueTask<ShopUser> GetMeAsync()
        {
            var response = await _httpClient.GetAsync("api/user/me");
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<ShopUser>();
        }

        public async ValueTask PutAsync(ShopUser user)
        {
            await _httpClient.PutAsJsonAsync($"api/user", user);
        }
    }
}
