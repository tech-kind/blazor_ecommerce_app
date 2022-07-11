using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Client.Extensions;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Client.Services
{

    public interface IReviewService
    {
        ValueTask<int> PostAsync(Review review);
        ValueTask PutAsync(Review review);
        ValueTask DeleteAsync(int id);
    }

    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;

        public ReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async ValueTask DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/review/{id}");
            await response.HandleError();
        }

        public async ValueTask<int> PostAsync(Review review)
        {
            var result = await _httpClient.PostAsJsonAsync("api/review", review);
            string id = await result.Content.ReadAsStringAsync();
            return int.Parse(id);
        }

        public async ValueTask PutAsync(Review review)
        {
            var response = await _httpClient.PutAsJsonAsync("api/review", review);
            await response.HandleError();
        }
    }
}
