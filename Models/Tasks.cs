using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace task_manager.Models
{
    public class Tasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("Users")]
        public required int userId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Title { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        [Required]
        public required Status TaskStatus { get; set; }

        [Required]
        public required Priority TaskPriority { get; set; }

        public Users User { get; set; } = null!;

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
