using Application_.DTOs;
using Application_.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application_.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponseDTO> CreateTaskAsync(CreateTaskDTO createTaskDTO)
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = createTaskDTO.Title,
                Description = createTaskDTO.Description,
                ProjectId = createTaskDTO.ProjectId,
                AssignedUserId = createTaskDTO.AssignedUserId,
                DueDate = createTaskDTO.DueDate,
                Priority = createTaskDTO.Priority,
                Status = "ToDo",
                CreatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.AddAsync(task);
            return await GetTaskResponseDTOAsync(createdTask);
        }

        public async Task<TaskResponseDTO?> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return null;

            return await GetTaskResponseDTOAsync(task);
        }

        public async Task<List<TaskResponseDTO>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var result = new List<TaskResponseDTO>();
            foreach (var task in tasks)
            {
                result.Add(await GetTaskResponseDTOAsync(task));
            }

            return result;
        }

        public async Task<List<TaskResponseDTO>> GetTasksByProjectAsync(Guid projectId)
        {
            var tasks = await _taskRepository.GetTasksByProjectAsync(projectId);
            var result = new List<TaskResponseDTO>();
            foreach (var task in tasks)
            {
                result.Add(await GetTaskResponseDTOAsync(task));
            }

            return result;
        }

        public async Task<List<TaskResponseDTO>> GetTasksByUserAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetTasksByUserAsync(userId);
            var result = new List<TaskResponseDTO>();
            foreach (var task in tasks)
            {
                result.Add(await GetTaskResponseDTOAsync(task));
            }

            return result;
        }

        public async Task<TaskResponseDTO> UpdateTaskAsync(Guid taskId, UpdateTaskDTO updateTaskDTO)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException("Task not found");

            task.Title = updateTaskDTO.Title;
            task.Description = updateTaskDTO.Description;
            task.Status = updateTaskDTO.Status;
            task.AssignedUserId = updateTaskDTO.AssignedUserId;
            task.DueDate = updateTaskDTO.DueDate;
            task.Priority = updateTaskDTO.Priority;

            var updatedTask = await _taskRepository.UpdateAsync(task);
            return await GetTaskResponseDTOAsync(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            return await _taskRepository.DeleteAsync(taskId);
        }

        public async Task<bool> ChangeTaskStatusAsync(Guid taskId, string newStatus)
        {
            return await _taskRepository.ChangeTaskStatusAsync(taskId, newStatus);
        }

        private async Task<TaskResponseDTO> GetTaskResponseDTOAsync(TaskItem task)
        {
            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                ProjectId = task.ProjectId,
                ProjectName = task.Project?.Name ?? "",
                AssignedUserId = task.AssignedUserId,
                AssignedUserName = task.AssignedUser?.FullName ?? task.AssignedUser?.UserName ?? ""
            };
        }
    }
} 