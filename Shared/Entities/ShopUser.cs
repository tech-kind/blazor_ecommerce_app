using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorECommerceApp.Shared.Entities
{
    public class ShopUser
    {
        public string Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string MobilePhone { get; set; }
    }
}
