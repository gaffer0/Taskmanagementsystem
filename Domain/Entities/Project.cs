namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}
