using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponseDTO?> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            var category = MapCategoryFromCreateDTO(createCategoryDTO);
            var result = await _categoryRepository.CreateCategory(category);
            if (result is null)
            {
                return null;
            }
            var createdCategory = await _categoryRepository.GetCategoryById(result.Id);
            return MapCategoryResponseDTO(createdCategory);
        }

        public async Task<bool> DeleteCategory(Guid guid)
        {
            return await _categoryRepository.DeleteCategory(guid);
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategories(int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var allCategories = await _categoryRepository.GetAllCategories(pageNumber,pageSize);
            return allCategories;
        }

        public async Task<CategoryResponseDTO?> GetCategoryById(Guid categoryId)
        {
            var categoryById = await _categoryRepository.GetCategoryById(categoryId);
            if (categoryById is null)
            {
                return null;
            }
            return MapCategoryResponseDTO(categoryById);
        }

        public async Task<CategoryResponseDTO?> UpdateCategory(UpdateCategoryDTO updatedCategoryDTO, Guid categoryId)
        {
            var category = MapCategoryFromUpdateDTO(updatedCategoryDTO);
            var result = await _categoryRepository.UpdateCategory(category,categoryId);
            if (result is null)
            {
                return null;
            }
            var updatedCategory = await _categoryRepository.GetCategoryById(result.Id);
            return MapCategoryResponseDTO(result);
        }
        private Category MapCategoryFromCreateDTO(CreateCategoryDTO createCategoryDTO)
        {
            if (createCategoryDTO == null) throw new ArgumentNullException(nameof(createCategoryDTO));

            return new Category
            {
                Id = Guid.NewGuid(),
                Name = createCategoryDTO.Name,
                Description = createCategoryDTO.Description,
                ParentCategoryId = createCategoryDTO.ParentCategoryId,
                DisplayOrder = createCategoryDTO.DisplayOrder,
                IsActive = createCategoryDTO.IsActive,
                CreatedAt = DateTime.UtcNow
            };
        }

        private CategoryResponseDTO MapCategoryResponseDTO(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                DisplayOrder = category.DisplayOrder,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                ParentCategoryName = category.ParentCategory?.Name,
                SubCategories = category.SubCategories?
                    .Select(subCategory => MapCategoryResponseDTO(subCategory))
                    .ToList() ?? new List<CategoryResponseDTO>()
            };
        }

        private Category MapCategoryFromUpdateDTO(UpdateCategoryDTO updateCategoryDTO)
        {
            if (updateCategoryDTO == null) throw new ArgumentNullException(nameof(updateCategoryDTO));

            return new Category
            {
                Name = updateCategoryDTO.Name,
                Description = updateCategoryDTO.Description,
                ParentCategoryId = updateCategoryDTO.ParentCategoryId,
                DisplayOrder = updateCategoryDTO.DisplayOrder,
                IsActive = updateCategoryDTO.IsActive,
                UpdatedAt = DateTime.UtcNow
            };
        }

    }
}

