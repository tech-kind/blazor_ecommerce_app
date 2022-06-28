using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorECommerceApp.Shared.Entities
{
    public class Cart
    {
        public int Quantity { get; set; }

        public Product Product { get; set; } = new ();

        public decimal CalcAmount()
        {
            return Math.Round(Quantity * Product.UnitPrice, MidpointRounding.AwayFromZero);
        }

        public CartStorage ToCartStorage()
        {
            return new CartStorage()
            {
                ProductId = Product.Id,
                Quantity = Quantity
            };
        }
    }
}
