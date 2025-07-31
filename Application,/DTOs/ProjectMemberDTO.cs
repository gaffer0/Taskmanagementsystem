using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_.DTOs
{
    public class DeleteProjectMemberDTO
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid MemberId { get; set; }
    }

    public class ListProjectMembersDTO
    {
        [Required]
        public Guid ProjectId { get; set; }
    }

    public class SearchProjectMembersDTO
    {
        [Required(ErrorMessage = "Project ID is required")]
        public Guid ProjectId { get; set; }

        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }

        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string? Role { get; set; }

        [StringLength(50, ErrorMessage = "Team cannot exceed 50 characters")]
        public string? Team { get; set; }
    }

    public class SearchProjectMembersRequestDTO
    {
        [Required(ErrorMessage = "Project ID is required")]
        public Guid ProjectId { get; set; }

        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }

        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string? Role { get; set; }

        [StringLength(50, ErrorMessage = "Team cannot exceed 50 characters")]
        public string? Team { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        [StringLength(20, ErrorMessage = "Sort field cannot exceed 20 characters")]
        public string? SortBy { get; set; } = "UserName";

        [RegularExpression("^(asc|desc)$", ErrorMessage = "Sort direction must be 'asc' or 'desc'")]
        public string? SortDirection { get; set; } = "asc";
    }

    public class ProjectMemberResponseDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Team { get; set; }
    }

    public class SearchProjectMembersResponseDTO
    {
        public List<ProjectMemberResponseDTO> Members { get; set; } = new List<ProjectMemberResponseDTO>();

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public SearchProjectMembersRequestDTO SearchCriteria { get; set; } = new SearchProjectMembersRequestDTO();
    }
}