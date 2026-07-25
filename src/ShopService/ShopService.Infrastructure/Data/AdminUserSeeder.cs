using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopService.Domain.Enums;
using System.Threading.Tasks;
using ShopService.Domain.Entities;

namespace ShopService.Infrastructure.Data
{
    public class AdminUserSeeder
    {
        public static async Task RegisterAdmin(IServiceProvider serviceProvider)
        {
            var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException(
                "Admin credentials not found. Please create a .env file with ADMIN_EMAIL and ADMIN_PASSWORD."
                );
            }
          
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == adminEmail);
            if (existingUser is null)
            {

                var Admin = new User
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    Age = 18,
                    EmailAddress = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    Gender = Gender.Male,
                    IsActive = true,
                    Role = UserRole.Admin,
                    IsEmailVerified = true,
                };
                await context.Users.AddAsync(Admin);
                await context.SaveChangesAsync();
            }
            else if (existingUser.Role != UserRole.Admin)
            {
                existingUser.Role = UserRole.Admin;
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword); 
                await context.SaveChangesAsync();
            }
        }
    }
}
