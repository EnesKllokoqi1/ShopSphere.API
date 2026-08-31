using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDTO?> CreateCategory(CreateCategoryDTO createCategoryDTO);
        Task<CategoryResponseDTO?> UpdateCategory(UpdateCategoryDTO updatedCategory,Guid categoryId);
        Task<bool> DeleteCategory(Guid guid);
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategories();
        Task<CategoryResponseDTO?> GetCategoryById(Guid categoryId);
    }
}
