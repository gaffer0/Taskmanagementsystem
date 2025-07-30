using Application_.DTOs;

namespace Application_.Interfaces
{
    public interface IUserManagementService
    {
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> ActivateUserAsync(Guid userId);
        Task<bool> DeactivateUserAsync(Guid userId);
        Task<List<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO?> GetUserByIdAsync(Guid userId);
    }
} 