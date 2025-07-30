using Application_.Common;
using Application_.DTOs;
using Application_.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Taskmanagementsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,ProjectManager")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _userManagementService.DeleteUserAsync(userId);
            if (result)
                return Ok("User deleted successfully");
            return NotFound("User not found");
        }

        [HttpPost("{userId}/activate")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ActivateUser(Guid userId)
        {
            var result = await _userManagementService.ActivateUserAsync(userId);
            if (result)
                return Ok("User activated successfully");
            return NotFound("User not found");
        }

        [HttpPost("{userId}/deactivate")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeactivateUser(Guid userId)
        {
            var result = await _userManagementService.DeactivateUserAsync(userId);
            if (result)
                return Ok("User deactivated successfully");
            return NotFound("User not found");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userManagementService.GetUserByIdAsync(userId);
            if (user != null)
                return Ok(user);
            return NotFound("User not found");
        }
    }
} 