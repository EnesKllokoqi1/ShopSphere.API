using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;

namespace ShopService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDTO>> CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            var category = await _categoryService.CreateCategory(createCategoryDTO);
            if (category is null)
            {
                return Conflict(new
                {
                    Message = "There already exists a category with that name"
                });
            }
            return CreatedAtAction(
                 nameof(GetCategoryById),
                 new { categoryId = category.Id },
                 category
             );
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{categoryId:guid}")]
        public async Task<ActionResult<CategoryResponseDTO>> UpdateCategory([FromBody] UpdateCategoryDTO updateCategoryDTO,[FromRoute] Guid categoryId)
        {
            var updatedCategory = await _categoryService.UpdateCategory(updateCategoryDTO, categoryId);
            if (updatedCategory is null)
            {
                return NotFound(new
                {
                    Message = "Category with that Id has not been found"
                });
            }
            return Ok(updatedCategory);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            var check = await _categoryService.DeleteCategory(categoryId);
            if (!check)
            {
                return NotFound(new { Message= "Category with that Id has not been found" });
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }
        [HttpGet("{categoryId:guid}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById([FromRoute] Guid categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);
            if (category is null)
            {
                return NotFound(new { Message = "Category with that Id not found" });
            }
            return Ok(category);
        }
    }
}
