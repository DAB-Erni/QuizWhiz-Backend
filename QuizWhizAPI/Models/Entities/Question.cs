using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWhizAPI.Models.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string QuestionAnswer { get; set; } = string.Empty;

        [ForeignKey("CreatedQuiz")]
        public int CreatedQuizId { get; set; }
        public CreatedQuiz CreatedQuiz { get; set; }
        public CheckTest CheckTest { get; set; }

    }
}
