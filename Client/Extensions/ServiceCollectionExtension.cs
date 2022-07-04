using System;
using BlazorECommerceApp.Client.Services;
using BlazorECommerceApp.Client.State;

namespace BlazorECommerceApp.Client.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPublicProductService, PublicProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartState, CartState>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
