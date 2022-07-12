using System;
using System.Linq;
using BlazorECommerceApp.Server.Data;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorECommerceApp.Server.Services
{
    public interface IReviewService
    {
        ValueTask<Review> GetAsync(int id);
        ValueTask<int> PostAsync(Review review, Guid userId);
        ValueTask<List<Review>> FilterByProductIdAsync(int productId);
        ValueTask<int> PutAsync(Review review, Guid userId);
        ValueTask<int> DeleteAsync(int id, Guid userId);
    }

    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;

        public ReviewService(IDbContextFactory<DataContext> factory)
        {
            _context = factory.CreateDbContext();
        }

        public async ValueTask<int> DeleteAsync(int id, Guid userId)
        {
            using (_context)
            {
                var dbReview = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
                if (dbReview is null)
                {
                    return StatusCodes.Status404NotFound;
                }

                if (dbReview.UserId != userId)
                {
                    return StatusCodes.Status400BadRequest;
                }

                _context.Reviews.Remove(dbReview);
                await _context.SaveChangesAsync();

                return StatusCodes.Status204NoContent;
            }
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

        public async ValueTask<Review> GetAsync(int id)
        {
            using (_context)
            {
                return await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
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

        public async ValueTask<int> PutAsync(Review review, Guid userId)
        {
            using (_context)
            {
                var dbReview = await _context.Reviews
                    .FirstOrDefaultAsync(x => x.Id == review.Id);
                
                if (dbReview is null)
                {
                    return StatusCodes.Status404NotFound;
                }

                if (dbReview.UserId != userId)
                {
                    return StatusCodes.Status400BadRequest;
                }

                dbReview.Rating = review.Rating;
                dbReview.Title = review.Title;
                dbReview.ReviewText = review.ReviewText;
                dbReview.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return StatusCodes.Status204NoContent;
            }
        }
    }
}
