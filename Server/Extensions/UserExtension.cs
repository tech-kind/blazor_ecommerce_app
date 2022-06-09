using System;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.Graph;

namespace BlazorECommerceApp.Server.Extensions
{
    public static class UserExtension
    {
        public static ShopUser ToShopUser(this User user)
        {
            return new ShopUser
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                MobilePhone = user.MobilePhone
            };
        }
    }
}
