using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Client.Extensions;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Client.Services
{
    public interface IOrderService
    {
        ValueTask<List<Order>> GetAllAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async ValueTask<List<Order>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/order");
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }
    }
}
