using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopService.Application.DTOs.UserDTOs;
using ShopService.Domain.Entities;

namespace ShopService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUser(Guid guid);
        Task<User> CreateUser(User user);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> DeleteUser(Guid guid);
        Task<User?> UpdateUser(User user, Guid guid);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByEmailAsync(string email);
    }
}
