using Application_.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Members)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectWithDetailsAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<List<Project>> GetAllProjectsWithDetailsAsync()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Members)
                .ToListAsync();
        }
    }
} 