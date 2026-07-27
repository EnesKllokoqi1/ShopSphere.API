using Microsoft.EntityFrameworkCore;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<User> CreateUser(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(Guid guid)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e=>e.Id==guid);
            if (user is not null)
            {
                _appDbContext.Users.Remove(user);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLower();
            var check = await _appDbContext.Users.AnyAsync(e=>e.EmailAddress==normalizedEmail);
            if (check) { return true; }
            return false;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _appDbContext.Users.AnyAsync(e => e.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLower();
            var user = await _appDbContext.Users.FirstOrDefaultAsync(e => e.EmailAddress == email);
            if (user is not null)
            {
                return user;
            }
            return null;
        }

        public async Task<User?> GetUser(Guid guid)
        {
            return await _appDbContext.Users.FindAsync(guid);
        }

        public async Task<User?> UpdateUser(User updatedUser, Guid guid)
        {
            var user = await GetUser(guid);
            if (user is null )
            {
                return null;
            }
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.EmailAddress = updatedUser.EmailAddress;
            user.Age = updatedUser.Age;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Gender = updatedUser.Gender;
            await _appDbContext.SaveChangesAsync();
            return user;
        }
    }
}
