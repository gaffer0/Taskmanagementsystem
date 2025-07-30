using Domain.Entities;

namespace Application_.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId);
        Task<List<TaskItem>> GetTasksByUserAsync(Guid userId);
        Task<bool> ChangeTaskStatusAsync(Guid taskId, string newStatus);
    }
} 