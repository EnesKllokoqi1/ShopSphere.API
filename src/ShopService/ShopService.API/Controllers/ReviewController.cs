using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.ReviewDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;

namespace ShopService.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewResponseDTO>> MakeReview([FromBody] MakeReviewDTO makeReviewDTO)
        {
            var review = await _reviewService.CreateReviewAsync(makeReviewDTO);
            if (review is null)
            {
                return BadRequest(new
                {

                    Status = "error",
                    Message = "You have already reviewed this product. Duplicate reviews are not allowed.",
                    Timestamp = DateTime.UtcNow
                });
            }
            return CreatedAtAction(
                nameof(GetReviewById),
                new { reviewId = review.Id },
                new
                {
                    Status = "success",
                    Message = "Review created successfully.",
                    Data = review,
                    Timestamp = DateTime.UtcNow
                }
            );
        }
        [Authorize]
        [HttpDelete("{reviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute] Guid reviewId)
        {
            var isDeleted = await _reviewService.DeleteReviewAsync(reviewId);
            if (!isDeleted)
            {
                return NotFound(new
                {
                    Status = "error",
                    Message = $"Review with ID '{reviewId}' was not found.",
                    Timestamp = DateTime.UtcNow
                });
            }
            return NoContent();
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetAllReviews( [FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var reviews = await _reviewService.GetAllReviewsAsync(pageNumber, pageSize);
            return Ok(new
            {
                Status = "success",
                Message = $"Retrieved {reviews.Count()} reviews.",
                Data = reviews,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });
        }
        [Authorize]
        [HttpGet("{reviewId:guid}")]
        public async Task<ActionResult<ReviewResponseDTO>> GetReviewById([FromRoute] Guid reviewId)
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            if (review is null)
            {
                return NotFound(new
                {
                    Status = "error",
                    Message = $"Review with ID '{reviewId}' was not found.",
                    Timestamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "success",
                Message = "Review retrieved successfully.",
                Data = review,
                Timestamp = DateTime.UtcNow
            });
        }
        [Authorize]
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByUserId([FromRoute] Guid userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(new
            {
                Status = "success",
                Message = reviews.Any()
                    ? $"Retrieved {reviews.Count()} reviews for user ID '{userId}'."
                    : $"No reviews found for user ID '{userId}'.",
                Data = reviews,
                Timestamp = DateTime.UtcNow
            });
        }
        [Authorize]
        [HttpPut("{reviewId:guid}")]
        public async Task<ActionResult<ReviewResponseDTO>> UpdateReview([FromBody] UpdateReviewDTO updateReviewDTO, [FromRoute] Guid reviewId)
        {
            var review = await _reviewService.UpdateReviewAsync(updateReviewDTO, reviewId);
            if (review is null)
            {
                return NotFound(new
                {
                    Status = "error",
                    Message = $"Review with ID '{reviewId}' was not found. Update failed.",
                    Timestamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "success",
                Message = "Review updated successfully.",
                Data = review,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
