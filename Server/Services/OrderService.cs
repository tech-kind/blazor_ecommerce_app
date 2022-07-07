using System;
using System.Linq;
using BlazorECommerceApp.Server.Data;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorECommerceApp.Server.Services
{
    public interface IOrderService
    {
        ValueTask<List<Order>> GetAllAsync(Guid userId);
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
    }
}
