using Application_.DTOs;

namespace Application_.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDTO> CreateTaskAsync(CreateTaskDTO createTaskDTO);
        Task<TaskResponseDTO?> GetTaskByIdAsync(Guid taskId);
        Task<List<TaskResponseDTO>> GetAllTasksAsync();
        Task<List<TaskResponseDTO>> GetTasksByProjectAsync(Guid projectId);
        Task<List<TaskResponseDTO>> GetTasksByUserAsync(Guid userId);
        Task<TaskResponseDTO> UpdateTaskAsync(Guid taskId, UpdateTaskDTO updateTaskDTO);
        Task<bool> DeleteTaskAsync(Guid taskId);
        Task<bool> ChangeTaskStatusAsync(Guid taskId, string newStatus);
    }
} 