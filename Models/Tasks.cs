namespace task_manager.Models
{
    public class Tasks
    {
        public required int Id { get; set; }
        public required int userId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required Status TaskStatus { get; set; }
        public required Priority TaskPriority { get; set; }

        public enum Priority
        {
            Low,
            Medium,
            High
        }

        public enum Status
        {
            Pending,
            InProgress,
            Done
        }

    }
}
