using System;
using System.Linq;
using BlazorECommerceApp.Server.Data;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorECommerceApp.Server.Services
{
    public interface IReviewService
    {
        ValueTask<int> PostAsync(Review review, Guid userId);
        ValueTask<List<Review>> FilterByProductIdAsync(int productId);
    }

    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;

        public ReviewService(IDbContextFactory<DataContext> factory)
        {
            _context = factory.CreateDbContext();
        }

        public async ValueTask<List<Review>> FilterByProductIdAsync(int productId)
        {
            using (_context)
            {
                return await _context.Reviews
                    .Where(x => x.ProductId == productId)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            }
        }

        public async ValueTask<int> PostAsync(Review review, Guid userId)
        {
            using (_context)
            {
                var now = DateTime.Now;
                review.CreateDate = now;
                review.UpdateDate = now;
                review.UserId = userId;

                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
                return review.Id;
            }
        }
    }
}
