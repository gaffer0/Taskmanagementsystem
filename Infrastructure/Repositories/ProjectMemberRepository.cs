using Application_.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> DeleteMemberAsync(DeleteProjectMemberDTO projectMemberDTO)
        {
            var member = await _context.ProjectMembers
                .FirstOrDefaultAsync(m => m.UserId == projectMemberDTO.MemberId && m.ProjectId == projectMemberDTO.ProjectId);

            if (member == null)
                return false;

            _context.ProjectMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProjectMemberResponseDTO>> ListProjectMembersAsync(ListProjectMembersDTO listDTO)
        {
            var members = await _context.ProjectMembers
                .Include(pm => pm.User)
                .Include(pm => pm.Project)
                .Where(pm => pm.ProjectId == listDTO.ProjectId)
                .Select(pm => new ProjectMemberResponseDTO
                {
                    UserId = pm.UserId,
                    UserName = pm.User.UserName ?? "",
                    Email = pm.User.Email ?? "",
                    ProjectId = pm.ProjectId,
                    ProjectName = pm.Project.Name ?? "",
                    Role = pm.Role,
                    Team = pm.Team
                })
                .ToListAsync();

            return members;
        }

        public async Task<SearchProjectMembersResponseDTO> SearchProjectMembersAsync(SearchProjectMembersRequestDTO searchRequest)
        {
            var query = _context.ProjectMembers
                .Include(pm => pm.User)
                .Include(pm => pm.Project)
                .Where(pm => pm.ProjectId == searchRequest.ProjectId);

            if (!string.IsNullOrEmpty(searchRequest.SearchTerm))
            {
                query = query.Where(pm =>
                    pm.User.UserName.Contains(searchRequest.SearchTerm) ||
                    pm.User.Email.Contains(searchRequest.SearchTerm) ||
                    pm.Role.Contains(searchRequest.SearchTerm) ||
                    (pm.Team != null && pm.Team.Contains(searchRequest.SearchTerm))
                );
            }

            if (!string.IsNullOrEmpty(searchRequest.Role))
            {
                query = query.Where(pm => pm.Role == searchRequest.Role);
            }

            if (!string.IsNullOrEmpty(searchRequest.Team))
            {
                query = query.Where(pm => pm.Team == searchRequest.Team);
            }

            var totalCount = await query.CountAsync();

            query = searchRequest.SortBy?.ToLower() switch
            {
                "username" => searchRequest.SortDirection?.ToLower() == "desc"
                    ? query.OrderByDescending(pm => pm.User.UserName)
                    : query.OrderBy(pm => pm.User.UserName),
                "email" => searchRequest.SortDirection?.ToLower() == "desc"
                    ? query.OrderByDescending(pm => pm.User.Email)
                    : query.OrderBy(pm => pm.User.Email),
                "role" => searchRequest.SortDirection?.ToLower() == "desc"
                    ? query.OrderByDescending(pm => pm.Role)
                    : query.OrderBy(pm => pm.Role),
                "team" => searchRequest.SortDirection?.ToLower() == "desc"
                    ? query.OrderByDescending(pm => pm.Team)
                    : query.OrderBy(pm => pm.Team),
                _ => searchRequest.SortDirection?.ToLower() == "desc"
                    ? query.OrderByDescending(pm => pm.User.UserName)
                    : query.OrderBy(pm => pm.User.UserName)
            };

            var skip = (searchRequest.Page - 1) * searchRequest.PageSize;
            var members = await query
                .Skip(skip)
                .Take(searchRequest.PageSize)
                .Select(pm => new ProjectMemberResponseDTO
                {
                    UserId = pm.UserId,
                    UserName = pm.User.UserName ?? "",
                    Email = pm.User.Email ?? "",
                    ProjectId = pm.ProjectId,
                    ProjectName = pm.Project.Name ?? "",
                    Role = pm.Role,
                    Team = pm.Team
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / searchRequest.PageSize);

            return new SearchProjectMembersResponseDTO
            {
                Members = members,
                TotalCount = totalCount,
                CurrentPage = searchRequest.Page,
                TotalPages = totalPages,
                PageSize = searchRequest.PageSize,
                SearchCriteria = searchRequest
            };
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException("Use DeleteMemberAsync for ProjectMember operations. ProjectMember requires both UserId and ProjectId.");
        }

        public override async Task<ProjectMember?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException("ProjectMember requires both UserId and ProjectId. Use specific methods for ProjectMember operations.");
        }

        public async Task<ProjectMember?> GetProjectMemberAsync(Guid userId, Guid projectId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .Include(pm => pm.Project)
                .FirstOrDefaultAsync(pm => pm.UserId == userId && pm.ProjectId == projectId);
        }

        public async Task<List<ProjectMember>> GetProjectMembersByProjectAsync(Guid projectId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .Include(pm => pm.Project)
                .Where(pm => pm.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<ProjectMember>> GetProjectMembersByUserAsync(Guid userId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .Include(pm => pm.Project)
                .Where(pm => pm.UserId == userId)
                .ToListAsync();
        }
        public async Task<(bool Success, string Message)> AssignMemberToProjectAsync(ProjectMember newMember)
        {
            // Check if the member already exists in the project
            var existingMember = await _context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.UserId == newMember.UserId && pm.ProjectId == newMember.ProjectId);
            if (existingMember != null)
            {
                return (false, "Member is already assigned to this project.");
            }

            _context.ProjectMembers.Add(newMember);
            await _context.SaveChangesAsync();
            return (true, "Member assigned successfully.");
        }
    }
}