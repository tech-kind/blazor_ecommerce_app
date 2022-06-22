using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Client.Util;
using BlazorECommerceApp.Shared.Entities;
using BlazorECommerceApp.Client.Extensions;

namespace BlazorECommerceApp.Client.Services
{
    public interface IPublicProductService
    {
        ValueTask<List<Product>> GetAllAsync();
        ValueTask<Product> GetAsync(int id);
    }

    public class PublicProductService : IPublicProductService
    {
        private readonly HttpClient _httpClient;

        public PublicProductService(PublicHttpClient client)
            => this._httpClient = client.Http;

        public async ValueTask<List<Product>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/product");
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<List<Product>>();
        }

        public async ValueTask<Product> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/product/{id}");
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<Product>();
        }
    }
}
