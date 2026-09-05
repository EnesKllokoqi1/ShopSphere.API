using ShopService.Application.DTOs.ReviewDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> CreateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Guid reviewId);
        Task<Review?> UpdateReviewAsync(Review updatedReview,Guid reviewId);
        Task<IEnumerable<ReviewResponseDTO>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(Guid reviewId);
        Task<IEnumerable<ReviewResponseDTO>> GetReviewsByUserIdAsync(Guid userId);
        Task<bool> UserAlreadyReviewedProductAsync(Guid? userId, Guid? productId);
    }
}
