using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> CreateCategory(Category category);
        Task<bool> DeleteCategory(Guid categoryId);
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategories();
        Task<Category?> UpdateCategory(Category updatedCategory,Guid categoryId);
        Task<Category?> GetCategoryById(Guid categoryId);
    }
}
