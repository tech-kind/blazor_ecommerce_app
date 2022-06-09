using System;
using Azure.Identity;
using BlazorECommerceApp.Server.Extensions;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.Graph;

namespace BlazorECommerceApp.Server.Services
{
    public interface IUserService
    {
        ValueTask<ShopUser> GetAsync(string userId);
    }

    public class UserService : IUserService
    {
        private readonly GraphServiceClient _graphClient;

        public UserService(IConfiguration configuration)
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(
                configuration["AzureAdB2C:TenantId"],
                configuration["AzureAdB2C:ClientId"],
                configuration["AzureAdB2C:ClientSecret"],
                options);

            _graphClient = new GraphServiceClient(clientSecretCredential);
        }

        public async ValueTask<ShopUser> GetAsync(string userId)
        {
            var user = await _graphClient.Users[userId].Request().GetAsync();
            return user.ToShopUser();
        }
    }
}
