using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        Task<bool> DeleteMemberAsync(DeleteProjectMemberDTO projectMemberDTO);
        Task<List<ProjectMemberResponseDTO>> ListProjectMembersAsync(ListProjectMembersDTO listDTO);
        Task<SearchProjectMembersResponseDTO> SearchProjectMembersAsync(SearchProjectMembersRequestDTO searchRequest);

        // Additional methods for composite key operations
        Task<ProjectMember?> GetProjectMemberAsync(Guid userId, Guid projectId);
        Task<List<ProjectMember>> GetProjectMembersByProjectAsync(Guid projectId);
        Task<List<ProjectMember>> GetProjectMembersByUserAsync(Guid userId);
        Task<(bool Success, string Message)> AssignMemberToProjectAsync(ProjectMember newMember);
    }
}