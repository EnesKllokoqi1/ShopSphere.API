using ShopService.Application.DTOs.ReviewDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<ReviewResponseDTO?> CreateReviewAsync(MakeReviewDTO makeReviewDTO)
        {
            var check = await _reviewRepository.UserAlreadyReviewedProductAsync(makeReviewDTO.UserId, makeReviewDTO.ProductId);
            if (check)
            {
                return null;
            }
            var review = MapMakeReviewDTO(makeReviewDTO);
            var review1 = await _reviewRepository.CreateReviewAsync(review);
            return MapReviewResponseDTO(review1);
           
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId)
        {
            return await _reviewRepository.DeleteReviewAsync(reviewId);
        }

        public Task<IEnumerable<ReviewResponseDTO>> GetAllReviewsAsync(int pageNumber,int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return _reviewRepository.GetAllReviewsAsync(pageNumber,pageSize);
        }

        public async Task<ReviewResponseDTO?> GetReviewByIdAsync(Guid guid)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(guid);
            if (review is null)
            {
                return null;
            }
            return MapReviewResponseDTO(review);
        }

        public async  Task<IEnumerable<ReviewResponseDTO>> GetReviewsByUserIdAsync(Guid userId)
        {

            return await _reviewRepository.GetReviewsByUserIdAsync(userId);

        }

        public async Task<ReviewResponseDTO?> UpdateReviewAsync(UpdateReviewDTO updateReviewDTO, Guid reviewId)
        {
            var updatedReview = MapUpdateReviewDTO(updateReviewDTO);
            var review = await _reviewRepository.UpdateReviewAsync(updatedReview,reviewId);
            if (review is null)
            {
                return null;
            }
            return MapReviewResponseDTO(review);
        }
        private Review MapMakeReviewDTO(MakeReviewDTO makeReviewDTO)
        {
            return new Review
            {
                UserId = makeReviewDTO.UserId,
                ProductId=makeReviewDTO.ProductId,
                Rating=makeReviewDTO.Rating,
                Comment=makeReviewDTO.Comment
            };
        }
        private ReviewResponseDTO MapReviewResponseDTO(Review review)
        {
           return new ReviewResponseDTO
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UserId = review.UserId,
                UserName = review.User != null ? review.User.FirstName : string.Empty,
                ProductId = review.ProductId,
                UpdatedAt = review.UpdatedAt,
                ProductName = review.Product != null ? review.Product.Name : string.Empty
            };
        }
        private Review MapUpdateReviewDTO(UpdateReviewDTO updateReviewDTO)
        {
            return new Review
            {
                Rating = updateReviewDTO.Rating,
                Comment = updateReviewDTO.Comment
            };
        }
    }
}
