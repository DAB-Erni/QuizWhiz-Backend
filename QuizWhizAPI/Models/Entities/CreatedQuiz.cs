using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWhizAPI.Models.Entities
{
    public class CreatedQuiz
    {
        [Key]
        public int CreatedQuizId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User CreatedBy { get; set; }

        // Navigation property
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<TakeQuiz> TakeQuizzes { get; set; } = new List<TakeQuiz>();

    }
}
