using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Client.Extensions;
using BlazorECommerceApp.Client.Util;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Client.Services
{
    public interface IPublicReviewService
    {
        ValueTask<Review> GetAsync(int id);
        ValueTask<List<Review>> FilterByProductIdAsync(int productId);
    }

    public class PublicReviewService : IPublicReviewService
    {
        private readonly HttpClient _httpClient;

        public PublicReviewService(PublicHttpClient publicHttpClient)
        {
            _httpClient = publicHttpClient.Http;
        }

        public async ValueTask<List<Review>> FilterByProductIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"api/review/filter/{productId}");
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<List<Review>>();
        }

        public async ValueTask<Review> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/review/{id}");
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<Review>();
        }
    }
}
