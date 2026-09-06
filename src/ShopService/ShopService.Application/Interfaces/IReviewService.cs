using ShopService.Application.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO?> CreateReviewAsync(MakeReviewDTO makeReviewDTO);
        Task<ReviewResponseDTO?> UpdateReviewAsync(UpdateReviewDTO updateReviewDTO,Guid reviewId);
        Task<bool> DeleteReviewAsync(Guid reviewId);
        Task<IEnumerable<ReviewResponseDTO>> GetAllReviewsAsync(int pageNumber,int pageSize);
        Task<ReviewResponseDTO?> GetReviewByIdAsync(Guid guid);
        Task<IEnumerable<ReviewResponseDTO>> GetReviewsByUserIdAsync(Guid userId);
    }
}
