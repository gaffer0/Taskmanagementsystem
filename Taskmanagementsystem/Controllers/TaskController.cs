using Application_.Common;
using Application_.DTOs;
using Application_.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Taskmanagementsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamLead")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDTO createTaskDTO)
        {
            try
            {
                var task = await _taskService.CreateTaskAsync(createTaskDTO);
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task != null)
                return Ok(task);
            return NotFound("Task not found");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProjectAsync(projectId);
            return Ok(tasks);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUser(Guid userId)
        {
            var tasks = await _taskService.GetTasksByUserAsync(userId);
            return Ok(tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDTO updateTaskDTO)
        {
            try
            {
                var task = await _taskService.UpdateTaskAsync(id, updateTaskDTO);
                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,ProjectManager")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (result)
                return Ok("Task deleted successfully");
            return NotFound("Task not found");
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeTaskStatus(Guid id, [FromBody] string newStatus)
        {
            var result = await _taskService.ChangeTaskStatusAsync(id, newStatus);
            if (result)
                return Ok("Task status updated successfully");
            return NotFound("Task not found");
        }
    }
} 