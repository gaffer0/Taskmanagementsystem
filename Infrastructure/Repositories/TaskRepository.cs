using Application_.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetTasksByUserAsync(Guid userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<bool> ChangeTaskStatusAsync(Guid taskId, string newStatus)
        {
            var task = await GetByIdAsync(taskId);
            if (task == null) return false;

            task.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 