using System;
using BlazorECommerceApp.Client.State;
using BlazorECommerceApp.Shared.Entities;
using Blazored.LocalStorage;

namespace BlazorECommerceApp.Client.Services
{
    public interface ICartService
    {
        ValueTask AddAsync(CartStorage cartStorage);
    }

    public class CartService : ICartService
    {
        private const string CART = "cart";

        private readonly ILocalStorageService _storageService;
        private readonly IPublicProductService _publishProductService;
        private readonly ICartState _cartState;

        public CartService(ILocalStorageService storageService, IPublicProductService publishProductService, ICartState cartState)
        {
            _storageService = storageService;
            _publishProductService = publishProductService;
            _cartState = cartState;
        }

        private async ValueTask UpdateCartStorage(CartStorage cart, CartStorageType type)
        {
            var storages = await _storageService.GetItemAsync<List<CartStorage>>(CART) ?? new List<CartStorage>();

            var storage = storages.Find(x => x.ProductId == cart.ProductId);
            if (storage is null)
            {
                storages.Add(cart);
                await _storageService.SetItemAsync(CART, storages);
                await _cartState.UpdateAsync();
                return;
            }

            switch(type)
            {
                case CartStorageType.Add:
                    storage.Quantity += cart.Quantity;
                    break;
                default:
                    break;
            }

            await _storageService.SetItemAsync(CART, storages);
            await _cartState.UpdateAsync();
        }

        public async ValueTask AddAsync(CartStorage cart)
        {
            await UpdateCartStorage(cart, CartStorageType.Add);
        }
    }
}
