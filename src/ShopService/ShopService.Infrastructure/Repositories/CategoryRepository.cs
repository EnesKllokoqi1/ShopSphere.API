using Microsoft.EntityFrameworkCore;
using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Category?> CreateCategory(Category category)
        {
            bool check = await _appDbContext.Categories.AnyAsync(e => e.Name == category.Name);
            if (check)
            {
                return null;
            }
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            var category = await GetCategoryById(categoryId);
            if (category is null)
            {   
                return false;
            }
            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategories()
        {
            return await _appDbContext.Categories
                .AsNoTracking()
                .Select(MapToCategoryResponseDTO())
                .ToListAsync();
        }

        private static Expression<Func<Category, CategoryResponseDTO>> MapToCategoryResponseDTO()
        {
            return e => new CategoryResponseDTO
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                ParentCategoryId = e.ParentCategoryId,
                DisplayOrder = e.DisplayOrder,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ParentCategoryName = e.ParentCategory != null ? e.ParentCategory.Name : null,
                SubCategories = e.SubCategories.Select(sub => new CategoryResponseDTO
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    Description = sub.Description,
                    ParentCategoryId = sub.ParentCategoryId,
                    DisplayOrder = sub.DisplayOrder,
                    IsActive = sub.IsActive,
                    CreatedAt = sub.CreatedAt,
                    UpdatedAt = sub.UpdatedAt
                })
            };
        }

        public async Task<Category?> GetCategoryById(Guid categoryId)
        {
            return await _appDbContext.Categories.FindAsync(categoryId);
        }

        public async Task<Category?> UpdateCategory(Category updatedCategory, Guid categoryId)
        {
            var theCategory = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (theCategory is null)
            {
                return null;
            }
            if (theCategory.Name != updatedCategory.Name)
            {
                bool nameExists = await _appDbContext.Categories.AnyAsync(c => c.Name == updatedCategory.Name);
                if (nameExists)
                {
                    return null;
                }
            }

            theCategory.Name = updatedCategory.Name;
            theCategory.Description = updatedCategory.Description;
            theCategory.ParentCategoryId = updatedCategory.ParentCategoryId;
            theCategory.DisplayOrder = updatedCategory.DisplayOrder;
            theCategory.IsActive = updatedCategory.IsActive;
            await _appDbContext.SaveChangesAsync();
            return theCategory;
        }
    }
}
