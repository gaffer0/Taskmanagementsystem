using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application_.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ActivateUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeactivateUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    UserName = u.UserName!,
                    Email = u.Email!,
                    FullName = u.FullName ?? "",
                    Role = u.Role.ToString(),
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserResponseDTO?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return new UserResponseDTO
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FullName = user.FullName ?? "",
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
} 