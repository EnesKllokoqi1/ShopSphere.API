using Microsoft.EntityFrameworkCore;
using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.DTOs.ReviewDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _appDbContext;
        public ReviewRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Review?> CreateReviewAsync(Review review)
        {
            _appDbContext.Reviews.Add(review);
            await _appDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId)
        {
            var review = await GetReviewByIdAsync(reviewId);
            if (review is null)
            {
                return false;
            }
            _appDbContext.Reviews.Remove(review);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetAllReviewsAsync()
        {
            return await _appDbContext.Reviews
          .AsNoTracking() 
          .Select(MapToReviewResponseDTO())
          .ToListAsync();
        }

        private static Expression<Func<Review, ReviewResponseDTO>> MapToReviewResponseDTO()
        {
            return review => new ReviewResponseDTO
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UserId = review.UserId,
                UserName = review.User != null ? review.User.FirstName : string.Empty,
                ProductId = review.ProductId,
                UpdatedAt=review.UpdatedAt,
                ProductName = review.Product != null ? review.Product.Name : string.Empty
            };
        }

        public async Task<Review?> GetReviewByIdAsync(Guid reviewId)
        {
            return await _appDbContext.Reviews.FindAsync(reviewId);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetReviewsByUserIdAsync(Guid userId)
        {

            return await _appDbContext.Reviews.AsNoTracking()
                 .Where(e => e.UserId == userId).Select(MapToReviewResponseDTO()).ToListAsync();
        }

        public async Task<Review?> UpdateReviewAsync(Review updatedReview, Guid reviewId)
        {
            var review = await GetReviewByIdAsync(reviewId);
            if (review is null)
            {
                return null;
            }
            review.Rating = updatedReview.Rating;
            review.Comment = updatedReview.Comment;
            await _appDbContext.SaveChangesAsync();
            return review;
        }

        public async Task<bool> UserAlreadyReviewedProductAsync(Guid? userId, Guid? productId)
        {
            return await _appDbContext.Reviews
         .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }
    }
}
