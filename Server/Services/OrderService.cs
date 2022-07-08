using System;
using System.Linq;
using BlazorECommerceApp.Server.Data;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace BlazorECommerceApp.Server.Services
{
    public interface IOrderService
    {
        ValueTask<List<Order>> GetAllAsync(Guid userId);
        ValueTask InsertAsync(Session session);
    }

    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(IDbContextFactory<DataContext> factory)
        {
            _context = factory.CreateDbContext();
        }

        public async ValueTask<List<Order>> GetAllAsync(Guid userId)
        {
            using (_context)
            {
                return await _context.Orders
                    .Include(x => x.OrderParticulars)
                    .ThenInclude(p => p.Product)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            }
        }

        public async ValueTask InsertAsync(Session session)
        {
            using (_context)
            {
                var now = DateTime.Now;
                var order = new Order
                {
                    CreateDate = now,
                    Amount = (int)session.AmountTotal,
                    StripePaymentId = session.PaymentIntentId,
                    OrderParticulars = new List<OrderParticular>()
                };

                if (session.Metadata.TryGetValue("userId", out string userId))
                {
                    order.UserId = Guid.Parse(userId);
                }

                var sessionService = new SessionService();
                var lineItems = sessionService.ListLineItems(session.Id);
                var stripeProductService = new Stripe.ProductService();

                foreach (var lineItem in lineItems)
                {
                    var particular = new OrderParticular
                    {
                        CreateDate = now,
                        UserId = order.UserId,
                        Quantity = (int)lineItem.Quantity,
                        UnitPrice = (int)lineItem.Price.UnitAmount
                    };

                    var stripeProduct = stripeProductService.Get(lineItem.Price.ProductId);
                    if (stripeProduct.Metadata.TryGetValue("productId", out string productId))
                    {
                        particular.ProductId = int.Parse(productId);
                    }

                    order.OrderParticulars.Add(particular);
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
