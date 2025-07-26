namespace Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = "ToDo";
        public DateTime? DueDate { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser AssignedUser { get; set; } = null!;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
