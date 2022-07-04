using System;
using BlazorECommerceApp.Client.State;
using BlazorECommerceApp.Shared.Entities;
using Blazored.LocalStorage;

namespace BlazorECommerceApp.Client.Services
{
    public interface ICartService
    {
        ValueTask UpdateAsync(CartStorage cartStorage);
        ValueTask AddAsync(CartStorage cartStorage);
        ValueTask RemoveAsync(int productId);
        ValueTask RemoveAllAsync();
        ValueTask<List<Cart>> GetAllAsync();
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
                if (type is not CartStorageType.Remove)
                {
                    storages.Add(cart);
                    await _storageService.SetItemAsync(CART, storages);
                }
                await _cartState.UpdateAsync();
                return;
            }

            switch(type)
            {
                case CartStorageType.Add:
                    storage.Quantity += cart.Quantity;
                    break;
                case CartStorageType.Update:
                    storage.Quantity = cart.Quantity;
                    break;
                case CartStorageType.Remove:
                    storages.Remove(storage);
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

        public async ValueTask UpdateAsync(CartStorage cart)
        {
            await UpdateCartStorage(cart, CartStorageType.Update);
        }

        public async ValueTask RemoveAsync(int productId)
        {
            await UpdateCartStorage(new CartStorage { ProductId = productId }, CartStorageType.Remove);
        }

        public async ValueTask<List<Cart>> GetAllAsync()
        {
            var storages = await _storageService.GetItemAsync<List<CartStorage>>(CART);
            if (storages is null)
            {
                return new List<Cart>();
            }

            var products = await _publishProductService.FilterAllByIdsAsync(storages.Select(x => x.ProductId).ToArray());

            return products.Select(p => new Cart
            {
                Quantity = storages.FirstOrDefault(s => s.ProductId == p.Id).Quantity,
                Product = p
            }).ToList();
        }

        public async ValueTask RemoveAllAsync()
        {
            await _storageService.RemoveItemAsync(CART);
            await _cartState.UpdateAsync();
        }
    }
}
