namespace Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "ToDo";
        public string Priority { get; set; } = "Medium";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid AssignedUserId { get; set; }
        public ApplicationUser AssignedUser { get; set; } = null!;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
