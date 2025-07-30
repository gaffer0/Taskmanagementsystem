using Domain.Entities;

namespace Application_.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetProjectWithDetailsAsync(Guid projectId);
        Task<List<Project>> GetAllProjectsWithDetailsAsync();
    }
} 