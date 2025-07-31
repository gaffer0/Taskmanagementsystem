using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}