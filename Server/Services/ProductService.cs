using System;
using BlazorECommerceApp.Server.Data;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorECommerceApp.Server.Services
{
    public interface IProductService
    {
        ValueTask<List<Product>> GetAllAsync();
        ValueTask<Product> GetAsync(int id);
        ValueTask<List<Product>> FilterAllByIdsAsync(int[] ids);
    }

    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(IDbContextFactory<DataContext> factory)
            => _context = factory.CreateDbContext();

        public async ValueTask<List<Product>> GetAllAsync()
        {
            // 商品の一覧を取得する
            using(_context)
            {
                return await _context.Products.ToListAsync();
            }
        }

        public async ValueTask<Product> GetAsync(int id)
        {
            using(_context)
            {
                return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async ValueTask<List<Product>> FilterAllByIdsAsync(int[] ids)
        {
            using (_context)
            {
                return await _context.Products.Where(x => ids.Contains(x.Id)).ToListAsync();
            }
        }
    }
}
