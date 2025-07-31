using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application_.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _repository;

        public ProjectMemberService(IProjectMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> DeleteMemberAsync(DeleteProjectMemberDTO deleteProjectMemberDTO)
        {
            try
            {
                var result = await _repository.DeleteMemberAsync(deleteProjectMemberDTO);
                if (!result)
                {
                    return new NotFoundObjectResult("Project member not found");
                }

                return new OkObjectResult("Project member deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error deleting project member: {ex.Message}");
            }
        }

        public async Task<IActionResult> ListProjectMembersAsync(ListProjectMembersDTO listDTO)
        {
            try
            {
                var members = await _repository.ListProjectMembersAsync(listDTO);
                return new OkObjectResult(members);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error listing project members: {ex.Message}");
            }
        }

        public async Task<IActionResult> SearchProjectMembersAsync(SearchProjectMembersRequestDTO searchRequest)
        {
            try
            {
                var response = await _repository.SearchProjectMembersAsync(searchRequest);
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error searching project members: {ex.Message}");
            }
        }

        public async Task<IActionResult> AssignMemberToProjectAsync(AssignProjectMemberDTO assignDTO)
        {
            try
            {
                var newMember = new ProjectMember
                {
                    UserId = assignDTO.MemberId,
                    ProjectId = assignDTO.ProjectId,
                    Role = assignDTO.Role,
                    Team = assignDTO.Team // optional, if you want to include
                };
                var result = await _repository.AssignMemberToProjectAsync(newMember);
                if (result.Success)
                {
                    return new OkObjectResult(result.Message);
                }
                else
                {
                    return new BadRequestObjectResult(result.Message);
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error assigning member: {ex.Message}");
            }
        }
    }
}