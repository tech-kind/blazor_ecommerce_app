using System;
using BlazorECommerceApp.Shared.Entities;
using Blazored.LocalStorage;

namespace BlazorECommerceApp.Client.State
{
    public interface ICartState
    {
        ValueTask UpdateAsync();
        event Action<int> OnQuantityChanged;
    }

    public class CartState : ICartState
    {
        private readonly ILocalStorageService _storageService;

        public event Action<int> OnQuantityChanged;

        public CartState(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }

        public async ValueTask UpdateAsync()
        {
            var carts = await _storageService.GetItemAsync<List<CartStorage>>("cart");
            int count = carts is null ? 0 : carts.Sum(x => x.Quantity);

            OnQuantityChanged?.Invoke(count);
        }
    }
}
