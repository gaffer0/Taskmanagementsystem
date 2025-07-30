using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;

namespace Application_.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectResponseDTO> CreateProjectAsync(CreateProjectDTO createProjectDTO)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = createProjectDTO.Name,
                Description = createProjectDTO.Description,
                DueDate = createProjectDTO.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            var createdProject = await _projectRepository.AddAsync(project);
            return await GetProjectResponseDTOAsync(createdProject);
        }

        public async Task<ProjectResponseDTO?> GetProjectByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetProjectWithDetailsAsync(projectId);
            if (project == null) return null;

            return await GetProjectResponseDTOAsync(project);
        }

        public async Task<List<ProjectResponseDTO>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllProjectsWithDetailsAsync();
            var result = new List<ProjectResponseDTO>();
            foreach (var project in projects)
            {
                result.Add(await GetProjectResponseDTOAsync(project));
            }

            return result;
        }

        public async Task<ProjectResponseDTO> UpdateProjectAsync(Guid projectId, UpdateProjectDTO updateProjectDTO)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new ArgumentException("Project not found");

            project.Name = updateProjectDTO.Name;
            project.Description = updateProjectDTO.Description;
            project.DueDate = updateProjectDTO.DueDate;

            var updatedProject = await _projectRepository.UpdateAsync(project);
            return await GetProjectResponseDTOAsync(updatedProject);
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            return await _projectRepository.DeleteAsync(projectId);
        }

        private async Task<ProjectResponseDTO> GetProjectResponseDTOAsync(Project project)
        {
            return new ProjectResponseDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                DueDate = project.DueDate,
                CreatedAt = project.CreatedAt,
                TaskCount = project.Tasks?.Count ?? 0,
                MemberCount = project.Members?.Count ?? 0
            };
        }
    }
} 