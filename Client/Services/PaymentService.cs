using System;
using System.Net.Http.Json;
using BlazorECommerceApp.Client.Extensions;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Client.Services
{
    public interface IPaymentService
    {
        ValueTask<string> GetCheckoutUrlAsync(List<Cart> carts);
    }

    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async ValueTask<string> GetCheckoutUrlAsync(List<Cart> carts)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/payment/create-checkout-session", carts);
            await response.HandleError();

            return await response.Content.ReadFromJsonAsync<string>();
        }
    }
}
