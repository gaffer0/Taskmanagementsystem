using Application_.DTOs;

namespace Application_.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponseDTO> CreateProjectAsync(CreateProjectDTO createProjectDTO);
        Task<ProjectResponseDTO?> GetProjectByIdAsync(Guid projectId);
        Task<List<ProjectResponseDTO>> GetAllProjectsAsync();
        Task<ProjectResponseDTO> UpdateProjectAsync(Guid projectId, UpdateProjectDTO updateProjectDTO);
        Task<bool> DeleteProjectAsync(Guid projectId);
    }
} 